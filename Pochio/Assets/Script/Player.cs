using Assets.Script;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class Player : MonoBehaviour
{
    [Header("移動速度")]
    public float SpeedX = 1.0f;
    
    [Header("落下速度")]
    public float Gravity = 1.0f;
    
    [Header("ジャンプ速度")]
    public float JumpSpeed = 1.0f;

    [Header("はしご移動速度")]
    public float LadderSpeed = 1.0f;

    [Header("ジャンプ可能高さ")]
    public float JumpHeight = 0.0f;
    
    [Header("ジャンプ可能時間")]
    public float JumpLimitTime = 0.0f;

    [Header("壁蹴りX速度可能時間")]
    public float WallJumpLimitTIme = 0.5f;

    [Header("連続ジャンプ可能数")]
    public int JumpMaxCount = 1;

    [Header("ダッシュ速度カーブ")]
    public AnimationCurve DashCurve;

    [Header("ジャンプ速度カーブ")]
    public AnimationCurve JumpCurve;

    [Header("落下速度カーブ")]
    public AnimationCurve FallCurve;

    [Header("接地チェック")]
    public GroundCheck Ground;
    
    [Header("頭ぶつけチェック")]
    public GroundCheck Head;

    [Header("前方チェック")]
    public GroundCheck FrontWall;

    [Header("はしご重なりチェック")]
    public LadderCheck Ladder;

    [Header("タイマー")]
    public GameObject Timer;

    [Header("ゴールスコア")]
    public int GoalScore;

    [Header("クリアテキスト")]
    public GameObject ClearText;

    public GameObject GoalText;

    // プレイヤーコンポーネント
    private Animator _anim = null;
    private Rigidbody2D _rigidBody2D;
    private CapsuleCollider2D _capsuleCollider2D;
    
    // 移動床
    private MoveLand _moveLand = null;
    
    // 開始位置
    private Vector2 _startPoint;
    
    // 速度系
    private float _beforeHorizonKey;
    private float _dashTime = 0.0f;

    // 衝突状態
    private bool _isGround = false;
    private bool _isHead = false;
    private bool _isLadder = false;
    private bool _isFrontWall = false;

    // ジャンプ状態
    private bool _isJump = false;
    private bool _isFrontWallJump = false;
    private bool _isReleaseJumpKey = true;
    private bool _canJumpKey = true;
    private float _jumpPos = 0.0f;
    private float _jumpTime = 0.0f;
    private float _wallJumpTime = 0.0f;
    private int _currentJumpCount = 0;

    // 落下状態
    private bool _isFall = false;
    private float _fallTime = 0.0f;

    // キー入力

    // 左アナログスティック
    private InputAction _leftStickInputAction = default(InputAction);

    // 右アナログスティック
    private InputAction _rightStickInputAction = default(InputAction);

    // 上アクションボタン押下
    private InputAction _northInputAction = default(InputAction);

    // 下アクションボタン押下
    private InputAction _southInputAction = default(InputAction);

    // 左アクションボタン押下
    private InputAction _westInputAction = default(InputAction);

    // 右アクションボタン押下
    private InputAction _eastInputAction = default(InputAction);

    // 左手前トリガー押下
    private InputAction _leftShouterInputAction = default(InputAction);

    // 左奥トリガー押下
    private InputAction _leftTriggerInputAction = default(InputAction);

    // 右手前トリガー押下
    private InputAction _rightShouterInputAction = default(InputAction);

    // 右奥トリガー押下
    private InputAction _rightTriggerInputAction = default(InputAction);

    // Optionボタン押下
    private InputAction _optionInputAction = default(InputAction);

    // Shareボタン押下
    private InputAction _shareInputAction = default(InputAction);


    // UI
    private TextManager _jumpText = null;
    private TextManager _speedText = null;
    private TextManager _cherryText = null;
    private TextManager _goalScoreText = null;
    private TimerManager _timerManager = null;
    private int _speedXCount = 1;
    private int _cherryCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        _goalScoreText = GoalText.GetComponent<TextManager>();
        _goalScoreText.SetText($"/{GoalScore.ToString()}");
        _timerManager = Timer.GetComponent<TimerManager>();

        InitializeInputAction();
    }

    /// <summary>
    /// InputSystem初期化
    /// </summary>
    private void InitializeInputAction()
    {
        var playerInput = GetComponent<PlayerInput>();

        playerInput.SwitchCurrentActionMap("Player");
        var actionMap = playerInput.currentActionMap;

        _leftStickInputAction = actionMap["LeftStick"];
        _rightStickInputAction = actionMap["RightStick"];
        _southInputAction = actionMap["SouthButton"];
        _eastInputAction = actionMap["EastButton"];
        _northInputAction = actionMap["NorthButton"];
        _westInputAction = actionMap["WestButton"];
        _leftShouterInputAction = actionMap["LeftShouter"];
        _leftTriggerInputAction = actionMap["LeftTrigger"];
        _rightShouterInputAction = actionMap["RightShouter"];
        _rightTriggerInputAction = actionMap["RightTrigger"];
        _optionInputAction = actionMap["Options"];
        _shareInputAction = actionMap["Share"];
    }

    private void Update()
    {
        // シーンリセット
        if (_shareInputAction.WasPressedThisFrame())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 衝突状態更新
        UpdateCollisionStatus();

        // ダッシュ時間計算
        var xInput = _leftStickInputAction.ReadValue<Vector2>().x;
        _dashTime = CalcDashTime(xInput);

        // 速度計算
        float localSpeedX = CalcXSpeed();
        float localSpeedY = CalcSpeedY();
        _rigidBody2D.velocity = new Vector2(localSpeedX, localSpeedY);

        // アニメーション更新
        UpdateAnimation();
    }

    /// <summary>
    /// 地形接触状態を更新する
    /// </summary>
    private void UpdateCollisionStatus()
    {
        // 接地判定
        _isGround = Ground.IsGround();
        _isFrontWall = FrontWall.IsGround();

        // 天井判定
        _isHead = Head.IsGround();
        if (_isGround && _isHead)
        {
            Head.Reset();
            _isHead = Head.IsGround();
        }

        // はしごつかまり判定
        _isLadder = Ladder.IsLadderOn;
    }

    /// <summary>
    /// X方向速度を計算する
    /// </summary>
    /// <returns>X方向速度</returns>
    private float CalcXSpeed()
    {
        float result = 0.0f;
        bool isPressJumpKey = _southInputAction.IsPressed();
        var xInput = GetInputX();

        // 前方壁蹴り開始
        if (_isFrontWall)
        {
            if (_isFrontWallJump == false)
            {
                if (isPressJumpKey)
                {
                    if (_isReleaseJumpKey)
                    {
                        _wallJumpTime = 0.0f;
                        _isFrontWallJump = true;
                    }
                }
            }
        }

        // 壁蹴り中
        if (_isFrontWallJump)
        {
            // 壁蹴りによる反動時間切れ
            if (_wallJumpTime > WallJumpLimitTIme)
            {
                _isFrontWallJump = false;
            }

            // 壁蹴りジャンプ継続
            else
            {
                _wallJumpTime += Time.deltaTime;
            }
        }
        
        // 右移動
        if (xInput > 0)
        {
            _dashTime += Time.deltaTime;
            if (_isFrontWallJump)
            {
                result = SpeedX * -0.3f;
            }
            else
            {
                result = SpeedX;
            }
        }

        // 左移動
        else if (xInput < 0)
        {
            _dashTime += Time.deltaTime;
            if (_isFrontWallJump)
            {
                result = SpeedX * 0.3f;
            }
            else
            {
                result = SpeedX * -1;
            }
        }

        // 入力なし
        else
        {
            result = 0.0f;
            _dashTime = 0.0f;
        }

        result *= DashCurve.Evaluate(_dashTime);

        if (_moveLand != null)
        {
            result += _moveLand.GetVelocity().x;
        }

        return result;
    }

    /// <summary>
    /// Y方向速度を計算する
    /// </summary>
    /// <returns>Y方向速度</returns>
    private float CalcSpeedY()
    {
        float result = -Gravity;

        // ジャンプキーを押しているか
        bool isPressJumpKey = _southInputAction.IsPressed();

        var verticalKey = GetInputY();

        if (_isGround || _isFrontWall)
        {
            _currentJumpCount = 0;
            if (isPressJumpKey)
            {
                if (_canJumpKey)
                {
                    result = JumpSpeed;
                    _jumpPos = transform.position.y;
                    _isJump = true;
                    _jumpTime = 0.0f;
                    _currentJumpCount++;
                    _isReleaseJumpKey = false;
                }
            }
            else
            {
                _isJump = false;
                _isFall = false;
                _canJumpKey = true;
                _isReleaseJumpKey = true;
            }
        }
        else if (_isJump)
        {
            if (isPressJumpKey)
            {
                if (_isReleaseJumpKey)
                {
                    if (_currentJumpCount < JumpMaxCount)
                    {
                        result = JumpSpeed;
                        _jumpPos = transform.position.y;
                        _jumpTime = 0.0f;
                        _isJump = true;
                        _currentJumpCount++;
                        _isReleaseJumpKey = false;
                    }
                }
            }
            else
            {
                _isReleaseJumpKey = true;
            }

            _canJumpKey = false;

            //現在の高さが飛べる高さより下か

            bool canHeight = _jumpPos + JumpHeight > transform.position.y;

            //ジャンプ時間が長くなりすぎてないか
            bool canTime = JumpLimitTime > _jumpTime;

            if (isPressJumpKey && canHeight && canTime && _isHead == false)
            {
                result = JumpSpeed;
                _jumpTime += Time.deltaTime;
                _isFall = false;
                _fallTime = 0.0f;
            }
            else
            {
                _isJump = false;
                _isFall = true;
                _jumpTime = 0.0f;
            }
        }
        else if (_isFall)
        {
            if (isPressJumpKey)
            {
                if (_isReleaseJumpKey)
                {
                    if (_currentJumpCount < JumpMaxCount)
                    {
                        result = JumpSpeed;
                        _jumpPos = transform.position.y;
                        _jumpTime = 0.0f;
                        _isJump = true;
                        _currentJumpCount++;
                        _isReleaseJumpKey = false;
                    }
                }
            }
            else
            {
                _isReleaseJumpKey = true;
            }

            // はしごに捕まっている間は落下速度0
            if (_isLadder)
            {
                _fallTime = 0.0f;
            }
            else
            {
                _fallTime += Time.deltaTime;
            }
        }
        else
        {
            _isFall = true;
            _fallTime = 0.0f;
        }

        if (_isJump)
        {
            result *= JumpCurve.Evaluate(_jumpTime);
        }

        else if (_isFall)
        {
            result *= FallCurve.Evaluate(_fallTime);
        }

        // はしご中は速度一定
        if (_isLadder)
        {
            if (verticalKey > 0)
            {
                result = LadderSpeed;
            }
            else if (verticalKey < 0)
            {
                result = -LadderSpeed;
            }
            else
            {
                result = 0.0f;
            }
        }

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="horizontalKey"></param>
    private float CalcDashTime(float horizontalKey)
    {
        var result = _dashTime;
        if (horizontalKey > 0 && _beforeHorizonKey < 0)
        {
            result = 0.0f;
        }
        else if (horizontalKey < 0 && _beforeHorizonKey > 0)
        {
            result = 0.0f;
        }
        _beforeHorizonKey = horizontalKey;

        return result;
    }

    /// <summary>
    /// アニメーション更新
    /// </summary>
    /// <param name="horizontalKey">横ボタン</param>
    private void UpdateAnimation()
    {
        var absLocalScaleX = Math.Abs(transform.localScale.x);
        var absLocalScaleY = Math.Abs(transform.localScale.y);

        var horizonKey = GetInputX();
        var verticalKey = GetInputY();

        // ジャンプ中はジャンプモーション
        if (_isJump)
        {
            _anim.Play("player_jump");
        }
        else if (_isLadder)
        {
            if (verticalKey != 0)
            {
                _anim.Play("player_clumb");
            }
            else
            {
                _anim.Play("player_clumb_stop");
            }
        }
        else if (_isFall)
        {
            _anim.Play("player_fall");
        }
        else if (_isGround)
        {
            // 地面にいて横入力がない場合は待ち
            if (verticalKey == 0)
            {
                _anim.Play("player_stand");
            }
            else
            {
                _anim.Play("player_run");
            }
        }
        else
        {
            _anim.Play("player_fall");
        }

        // 向き更新
        if (horizonKey > 0)
        {
            transform.localScale = new Vector2(absLocalScaleX, absLocalScaleY);
        }
        else if (horizonKey < 0)
        {
            transform.localScale = new Vector2(-absLocalScaleX, absLocalScaleY);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == Tag.MOVE_GROUND)
        {
            var stepOnHeight = _capsuleCollider2D.size.y * 0.9f;
            float judgPosY = transform.position.y - (_capsuleCollider2D.size.y / 2f) + stepOnHeight;
            foreach (var p in collision.contacts)
            {
                // 動く地面に乗っていれば取得
                if (p.point.y < judgPosY)
                {
                    _moveLand = collision.gameObject.GetComponent<MoveLand>();
                }
            }
        }

        else if (collision.collider.tag == Tag.DEAD)
        {
            transform.position = _startPoint;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == Tag.MOVE_GROUND)
        {
            _moveLand = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Tag.JUMP_ITEM)
        {
            if (collision.enabled == false)
            {
                return;
            }
            collision.enabled = false;

            JumpMaxCount++;

            if (_jumpText == null)
            {
                _jumpText = GameObject.Find("JumpCount").GetComponent<TextManager>();
            }
            _jumpText.SetText(JumpMaxCount.ToString());

            Destroy(collision.gameObject);
        }

        else if (collision.tag == Tag.SPEED_ITEM)
        {
            if (collision.enabled == false)
            {
                return;
            }
            collision.enabled = false;

            _speedXCount++;
            SpeedX += 2f;

            if (_speedText == null)
            {
                _speedText = GameObject.Find("PlayerSpeed").GetComponent<TextManager>();
            }
            _speedText.SetText(_speedXCount.ToString());

            Destroy(collision.gameObject);
        }

        else if (collision.tag == Tag.CHERRY_ITEM)
        {
            if (collision.enabled == false)
            {
                return;
            }
            collision.enabled = false;
            
            _cherryCount++;

            if (_cherryText == null)
            {
                _cherryText = GameObject.Find("CherryCountCurrent").GetComponent<TextManager>();
            }
            _cherryText.SetText(_cherryCount.ToString());

            Destroy(collision.gameObject);

            // げーむくりあ
            if (_cherryCount >= GoalScore)
            {
                _timerManager.TimerStop();
                ClearText.GetComponent<Text>().enabled = true;
            }
        }

        else if (collision.tag == Tag.RESTART_ITEM)
        {
            _startPoint = collision.gameObject.transform.position;
        }
    }

    private float GetInputX()
    {
        return _leftStickInputAction.ReadValue<Vector2>().x;
    }

    private float GetInputY()
    {
        return _leftStickInputAction.ReadValue<Vector2>().y;
    }
}
