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
    
    [Header("ジャンプ可能高さ")]
    public float JumpHeight = 0.0f;
    
    [Header("ジャンプ可能時間")]
    public float JumpLimitTime = 0.0f;
    
    [Header("ダッシュ速度カーブ")]
    public AnimationCurve DashCurve;

    [Header("ジャンプ速度カーブ")]
    public AnimationCurve JumpCurve;
    
    [Header("接地チェック")]
    public GroundCheck Ground;
    
    [Header("頭ぶつけチェック")]
    public GroundCheck Head;

    private bool _isJump = false;
    private float _jumpPos = 0.0f;   
    private float _dashTime = 0.0f;
    private float _jumpTime = 0.0f;
    private bool _canJumpKey = true;
    private bool _isDown = false;
    private float _beforeKey;
    private Animator _anim = null;
    private Rigidbody2D _rigidBody2D;
    private CapsuleCollider2D _capsuleCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 入力キー取得
        float horizontalKey = Input.GetAxis("Horizontal");
        float jumpKey = Input.GetAxis("JoyPadCross");

        // 接地判定
        bool isGround = Ground.IsGround();
        bool isHead = Head.IsGround();
        if (isGround && isHead)
        {
            Head.Reset();
            isHead = Head.IsGround();
        }

        // ダッシュ時間計算
        _dashTime = CalcDashTime(horizontalKey);

        // 速度計算
        float localSpeedX = CalcXSpeed(horizontalKey);
        float localSpeedY = CalcSpeedY(jumpKey, isGround, isHead);
        _rigidBody2D.velocity = new Vector2(localSpeedX, localSpeedY);
        
        // アニメーション更新
        UpdateAnimation(horizontalKey, isGround);
    }

    /// <summary>
    /// X方向速度を計算する
    /// </summary>
    /// <returns>X方向速度</returns>
    private float CalcXSpeed(float horizontalKey)
    {
        float result = 0.0f;

        if (horizontalKey > 0)
        {
            _dashTime += Time.deltaTime;
            result = SpeedX;
        }
        else if (horizontalKey < 0)
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
    private float CalcSpeedY(float verticalKey, bool isGround, bool isHead)
    {
        float result = -Gravity;

        if (isGround)
        {
            if (verticalKey > 0)
            {
                if (_canJumpKey)
                {
                    result = JumpSpeed;
                    _jumpPos = transform.position.y;
                    _isJump = true;
                    _jumpTime = 0.0f;
                }
            }
            else
            {
                _isJump = false;
                _canJumpKey = true;
            }
        }
        else if (_isJump)
        {
            _canJumpKey = false;

            //上方向キーを押しているか
            bool isPushUpKey = verticalKey > 0;

            //現在の高さが飛べる高さより下か

            bool canHeight = _jumpPos + JumpHeight > transform.position.y;

            //ジャンプ時間が長くなりすぎてないか
            bool canTime = JumpLimitTime > _jumpTime;

            if (isPushUpKey && canHeight && canTime && isHead == false)
            {
                result = JumpSpeed;
                _jumpTime += Time.deltaTime;
            }
            else
            {
                _isJump = false;
                _jumpTime = 0.0f;
                //Debug.Log("落下します");
            }
        }

        if (_isJump)
        {
            result *= JumpCurve.Evaluate(_jumpTime);
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
    private void UpdateAnimation(float horizontalKey, bool isGround)
    {
        var absLocalScaleX = Math.Abs(transform.localScale.x);
        var absLocalScaleY = Math.Abs(transform.localScale.y);

        // ジャンプ中はジャンプモーション
        if (_isJump)
        {
            _anim.Play("player_jump");
        }
        else if (isGround)
        {
            // 地面にいて横入力がない場合は待ち
            if (horizontalKey == 0)
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
        if (horizontalKey > 0)
        {
            transform.localScale = new Vector2(absLocalScaleX, absLocalScaleY);
        }
        else if (horizontalKey < 0)
        {
            transform.localScale = new Vector2(-absLocalScaleX, absLocalScaleY);
        }
    }
}
