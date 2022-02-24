using UnityEngine;
using UnityEngine.UI;

using Assets.Script.Game;
using Assets.Script.Collider;

namespace Assets.Script.Player
{
    public partial class Player
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.enabled == false)
            {
                return;
            }

            var colliderController = collision.gameObject.GetComponent<ColliderController>();
            AudioClip itemAudioClip = null;

            // ジャンプ回数アイテム
            if (collision.tag == Tag.JUMP_ITEM)
            {
                // アイテム取得音再生
                if (colliderController != null)
                {
                    itemAudioClip = colliderController.AudioClip;
                }

                collision.enabled = false;
                JumpLevel++;
                Destroy(collision.gameObject);
            }

            // スピードアップアイテム
            else if (collision.tag == Tag.SPEED_ITEM)
            {
                // アイテム取得音再生
                if (colliderController != null)
                {
                    itemAudioClip = colliderController.AudioClip;
                }

                collision.enabled = false;
                SpeedLevel++;
                SpeedX += 1.0f;
                Destroy(collision.gameObject);
            }

            // 壁蹴りスキルアイテム
            else if (collision.tag == Tag.WALL_JUMP_ITEM)
            {
                // アイテム取得音再生
                if (colliderController != null)
                {
                    itemAudioClip = colliderController.AudioClip;
                }

                collision.enabled = false;
                CanWallJump = true;
                Destroy(collision.gameObject);
            }

            // スコアアイテム
            else if (collision.tag == Tag.CHERRY_ITEM)
            {
                // アイテム取得音再生
                if (colliderController != null)
                {
                    itemAudioClip = colliderController.AudioClip;
                }

                collision.enabled = false;
                GameController.GetInstance.AddScore();
                Destroy(collision.gameObject);

                //// げーむくりあ
                //if (GameController.GetInstance.IsClear())
                //{
                //    GameController.GetInstance.TimerStop();
                //    ClearText.GetComponent<Text>().enabled = true;
                //}
            }

            // 再スタートアイテム
            else if (collision.tag == Tag.RESTART_ITEM)
            {
                _startPoint = collision.gameObject.transform.position;
            }

            // アイテムオーディオ再生
            if (itemAudioClip != null)
            {
                PlayItemSound(itemAudioClip);
            }
        }
    }
}
