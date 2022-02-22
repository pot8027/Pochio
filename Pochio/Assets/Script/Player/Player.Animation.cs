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
                PlayAnimJump();
            }
            else if (_isLadder)
            {
                if (verticalKey != 0)
                {
                    PlayAnimClimb();
                }
                else
                {
                    PlayAnimClimbStop();
                }
            }
            else if (_isFall)
            {
                PlayAnimFall();
            }
            else if (_isGround)
            {
                // 地面にいて横入力がない場合は待ち
                if (GetInputX() == 0)
                {
                    PlayAnimStand();
                }
                else
                {
                    PlayAnimRun();
                }
            }
            else
            {
                PlayAnimFall(); ;
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

        private void PlayAnimJump()
        {
            // TODO:
            // ジャンプキー押下時のみ音を鳴らす
            if (_canPlayJumpAudio == true)
            {
                PlayPlayerJumpAudio();
                _canPlayJumpAudio = false;
            }

            StopPlayerRunAudio();
            _anim.Play("player_jump");
        }

        private void PlayAnimClimb()
        {
            StopPlayerRunAudio();
            _anim.Play("player_clumb");
        }

        private void PlayAnimClimbStop()
        {
            StopPlayerRunAudio();
            _anim.Play("player_clumb_stop"); ;
        }

        private void PlayAnimFall()
        {
            StopPlayerRunAudio();
            _anim.Play("player_fall");
        }

        private void PlayAnimStand()
        {
            StopPlayerRunAudio();
            _anim.Play("player_stand");
        }

        private void PlayAnimRun()
        {
            PlayPlayerRunAudio();
            _anim.Play("player_run");
        }
    }
}
