using System;
using UnityEngine;

namespace Assets.Script.Player
{
    public partial class Player
    {
        /// <summary>
        /// アニメーション更新
        /// </summary>
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
    }
}
