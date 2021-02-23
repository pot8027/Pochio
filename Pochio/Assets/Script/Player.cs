﻿using Assets.Script;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("左壁チェック")]
    public GroundCheck LeftWall;

    [Header("右壁チェック")]
    public GroundCheck RightWall;

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
    private bool _isLeftWall = false;
    private bool _isRightWall = false;

    // ジャンプ状態
    private bool _isJump = false;
    private bool _isReleaseJumpKey = true;
    private bool _canJumpKey = true;
    private float _jumpPos = 0.0f;
    private float _jumpTime = 0.0f;
    private int _currentJumpCount = 0;

    // 落下状態
    private bool _isFall = false;
    private float _fallTime = 0.0f;

    // キー入力
    private float _horizontalKey;
    private float _verticalKey;
    private float _jumpKey;

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
    }

    private void Update()
    {
        // キー入力取得
        {
            _horizontalKey = Input.GetAxis("Horizontal");
            _verticalKey = Input.GetAxis("Vertical");
            _jumpKey = Input.GetAxis("JoyPadCross");
        }
        
        // シーンリセット
        if (Input.GetAxis("Reset") != 0)
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
        _dashTime = CalcDashTime(_horizontalKey);

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

        if (_horizontalKey > 0)
        {
            _dashTime += Time.deltaTime;
            result = SpeedX;
        }
        else if (_horizontalKey < 0)
        {
            _dashTime += Time.deltaTime;
            result = SpeedX * -1;
        }
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
        bool isPressJumpKey = _jumpKey > 0;

        if (_isGround)
        {
            _currentJumpCount = 0;
            if (_jumpKey > 0)
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
            if (_verticalKey > 0)
            {
                result = LadderSpeed;
            }
            else if (_verticalKey < 0)
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

        // ジャンプ中はジャンプモーション
        if (_isJump)
        {
            _anim.Play("player_jump");
        }
        else if (_isLadder)
        {
            if (_verticalKey != 0)
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
            if (_horizontalKey == 0)
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
        if (_horizontalKey > 0)
        {
            transform.localScale = new Vector2(absLocalScaleX, absLocalScaleY);
        }
        else if (_horizontalKey < 0)
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
}
