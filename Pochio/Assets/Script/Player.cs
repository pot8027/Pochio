using System;
using UnityEngine;

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
    
    [Header("はしご重なりチェック")]
    public LadderCheck Ladder;

    private bool _isJump = false;
    private bool _isFall = false;
    private float _jumpPos = 0.0f;   
    private float _dashTime = 0.0f;
    private float _jumpTime = 0.0f;
    private float _fallTime = 0.0f;
    private bool _canJumpKey = true;
    private bool _isLadder = false;
    private int _currentJumpCount = 0;
    private bool _releaseJumpKey = true;
    private float _beforeKey;
    private Animator _anim = null;
    private Rigidbody2D _rigidBody2D;
    private CapsuleCollider2D _capsuleCollider2D;

    // キー入力
    private float _horizontalKey;
    private float _verticalKey;
    private float _jumpKey;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        _horizontalKey = Input.GetAxis("Horizontal");
        _verticalKey = Input.GetAxis("Vertical");
        _jumpKey = Input.GetAxis("JoyPadCross");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 接地判定
        bool isGround = Ground.IsGround();
        
        // 天井判定
        bool isHead = Head.IsGround();
        if (isGround && isHead)
        {
            Head.Reset();
            isHead = Head.IsGround();
        }

        // はしごつかまり判定
        _isLadder = Ladder.IsLadderOn;

        // ダッシュ時間計算
        _dashTime = CalcDashTime(_horizontalKey);

        // 速度計算
        float localSpeedX = CalcXSpeed();
        float localSpeedY = CalcSpeedY(isGround, isHead);
        _rigidBody2D.velocity = new Vector2(localSpeedX, localSpeedY);
        
        // アニメーション更新
        UpdateAnimation(isGround);
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

        return result;
    }

    /// <summary>
    /// Y方向速度を計算する
    /// </summary>
    /// <returns>Y方向速度</returns>
    private float CalcSpeedY(bool isGround, bool isHead)
    {
        float result = -Gravity;

        // ジャンプキーを押しているか
        bool isPressJumpKey = _jumpKey > 0;

        if (isGround)
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
                    _releaseJumpKey = false;
                }
            }
            else
            {
                _isJump = false;
                _isFall = false;
                _canJumpKey = true;
                _releaseJumpKey = true;
            }
        }
        else if (_isJump)
        {
            if (isPressJumpKey)
            {
                if (_releaseJumpKey)
                {
                    if (_currentJumpCount < JumpMaxCount)
                    {
                        result = JumpSpeed;
                        _jumpPos = transform.position.y;
                        _jumpTime = 0.0f;
                        _isJump = true;
                        _currentJumpCount++;
                        _releaseJumpKey = false;
                    }
                }
            }
            else
            {
                _releaseJumpKey = true;
            }

            _canJumpKey = false;

            //現在の高さが飛べる高さより下か

            bool canHeight = _jumpPos + JumpHeight > transform.position.y;

            //ジャンプ時間が長くなりすぎてないか
            bool canTime = JumpLimitTime > _jumpTime;

            if (isPressJumpKey && canHeight && canTime && isHead == false)
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
                if (_releaseJumpKey)
                {
                    if (_currentJumpCount < JumpMaxCount)
                    {
                        result = JumpSpeed;
                        _jumpPos = transform.position.y;
                        _jumpTime = 0.0f;
                        _isJump = true;
                        _currentJumpCount++;
                        _releaseJumpKey = false;
                    }
                }
            }
            else
            {
                _releaseJumpKey = true;
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
        if (horizontalKey > 0 && _beforeKey < 0)
        {
            result = 0.0f;
        }
        else if (horizontalKey < 0 && _beforeKey > 0)
        {
            result = 0.0f;
        }
        _beforeKey = horizontalKey;

        return result;
    }

    /// <summary>
    /// アニメーション更新
    /// </summary>
    /// <param name="horizontalKey">横ボタン</param>
    private void UpdateAnimation(bool isGround)
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
            _anim.Play("player_jump");
        }
        else if (isGround)
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
            _anim.Play("player_jump");
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
}
