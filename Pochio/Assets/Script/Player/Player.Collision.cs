using UnityEngine;

namespace Assets.Script.Player
{
    public partial class Player
    {
        /// <summary>
        /// 正面壁衝突判定
        /// </summary>
        private bool _isTouchingFrontWall = false;
        private void UpdateFrontWallStatus()
        {
            _isTouchingFrontWall = FrontWall.IsGround();
        }

        /// <summary>
        /// 地面衝突判定
        /// </summary>
        private bool _isTouchingGround = false;
        private void UpdateGroundStatus()
        {
            _isTouchingGround = Ground.IsGround();
        }

        /// <summary>
        /// 鍵壁衝突判定
        /// </summary>
        private bool _isTouchingKeyWall = false;
        private void UpdateKeyWallStatus()
        {
            _isTouchingKeyWall = KeyWall.IsGround();
        }

        /// <summary>
        /// 天井衝突判定
        /// </summary>
        private bool _isHead = false;
        private void UpdateHeadStatus()
        {
            _isHead = Head.IsGround();
            if (_isTouchingGround && _isHead)
            {
                Head.Reset();
                _isHead = Head.IsGround();
            }
        }

        /// <summary>
        /// はしご衝突判定
        /// </summary>
        private bool _isLadder = false;
        private void UpdateLadderStatus()
        {
            _isLadder = Ladder.IsLadderOn;
        }

        /// <summary>
        /// 衝突開始通知
        /// </summary>
        /// <param name="collision"></param>
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


        /// <summary>
        /// 衝突終了通知
        /// </summary>
        /// <param name="collision"></param>
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.tag == Tag.MOVE_GROUND)
            {
                _moveLand = null;
            }
        }
    }
}
