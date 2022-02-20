using UnityEngine;

namespace Assets.Script.Player
{
    public partial class Player
    {
        /// <summary>
        /// ジャンプ制御
        /// </summary>
        private bool _isJump = false;
        private bool _canJumpKey = true;
        private bool _isReleaseJumpKey = true;
        private float _jumpPos = 0.0f;
        private float _jumpTime = 0.0f;
        private int _currentJumpCount = 0;

        /// <summary>
        ///  壁蹴り制御
        /// </summary>
        private bool _canWallJump = false;      // 壁蹴り可能フラグ
        private bool _isFrontWallJump = false;
        private float _wallJumpTime = 0.0f;

        /// <summary>
        /// ダッシュ制御
        /// </summary>
        private float _beforeHorizonKey;
        private float _dashTime = 0.0f;

        /// <summary>
        /// 落下制御
        /// </summary>
        private bool _isFall = false;
        private float _fallTime = 0.0f;

        /// <summary>
        /// 速度更新
        /// </summary>
        private void UpdateVelocity()
        {
            // ダッシュ時間計算
            var xInput = _leftStickInputAction.ReadValue<Vector2>().x;
            _dashTime = CalcDashTime(xInput);

            // 速度計算
            float localSpeedX = CalcXSpeed();
            float localSpeedY = CalcSpeedY();
            _rigidBody2D.velocity = new Vector2(localSpeedX, localSpeedY);
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
        /// X方向速度を計算する
        /// </summary>
        /// <returns>X方向速度</returns>
        private float CalcXSpeed()
        {
            bool isPressJumpKey = _southInputAction.IsPressed();
            var xInput = GetInputX();

            // 前方壁蹴り開始
            if (_isFrontWall)
            {
                // 壁蹴り可
                if (_canWallJump)
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

            float result;

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

            // はしご中にアナログスティックだと横に動きすぎるので調整
            if (_isLadder && GetInputY() >= 0.5)
            {
                result *= 0.2f;
            }

            return result;
        }

        /// <summary>
        /// Y方向速度を計算する
        /// </summary>
        /// <returns>Y方向速度</returns>
        private float CalcSpeedY()
        {
            float result = -1 * Gravity;

            var inputY = GetInputY();
            var isPressed = _southInputAction.IsPressed();
            var wasPressedThisFrame = _southInputAction.WasPressedThisFrame();

            // 接地中
            var isTouchGround = false;
            {
                // 壁蹴りあり
                if (_canWallJump)
                {
                    isTouchGround = _isGround || _isFrontWall;
                }

                // 壁蹴りなし
                else
                {
                    isTouchGround = _isGround;
                }
            }
            
            // 接地中
            if (isTouchGround)
            {
                _currentJumpCount = 0;
                _isFall = false;

                // ジャンプ開始
                if (isPressed)
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

                // 接地中
                else
                {
                    _isJump = false;
                    _isFall = false;
                    _canJumpKey = true;
                    _isReleaseJumpKey = true;
                }
            }

            // ジャンプ中
            else if (_isJump)
            {
                // ジャンプキー押下開始
                if (isPressed)
                {
                    if (_isReleaseJumpKey)
                    {
                        // 空中ジャンプ開始
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

                if (isPressed && canHeight && canTime && _isHead == false)
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

            // 落下中
            else if (_isFall)
            {
                if (isPressed)
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

            // 速度調整
            {
                if (_isJump)
                {
                    result *= JumpCurve.Evaluate(_jumpTime);
                }
                else if (_isFall)
                {
                    result *= FallCurve.Evaluate(_fallTime);
                }
            }
            
            // はしご中は速度一定
            if (_isLadder)
            {
                if (inputY > 0)
                {
                    result = LadderSpeed;
                }
                else if (inputY < 0)
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
    }
}
