using UnityEngine;
using UnityEngine.UI;

using Assets.Script.Game;

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

            // ジャンプ回数アイテム
            if (collision.tag == Tag.JUMP_ITEM)
            {
                collision.enabled = false;
                JumpLevel++;
                Destroy(collision.gameObject);
            }

            // スピードアップアイテム
            else if (collision.tag == Tag.SPEED_ITEM)
            {
                collision.enabled = false;
                SpeedLevel++;
                SpeedX += 2f;
                Destroy(collision.gameObject);
            }

            // 壁蹴りスキルアイテム
            else if (collision.tag == Tag.WALL_JUMP_ITEM)
            {
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
                    var audioClip = colliderController.AudioClip;
                    if (audioClip != null)
                    {
                        ItemAudioSrc.PlayOneShot(audioClip);
                    }
                }

                collision.enabled = false;
                GameController.GetInstance.AddScore();
                Destroy(collision.gameObject);

                // げーむくりあ
                if (GameController.GetInstance.IsClear())
                {
                    GameController.GetInstance.TimerStop();
                    ClearText.GetComponent<Text>().enabled = true;
                }
            }

            // 再スタートアイテム
            else if (collision.tag == Tag.RESTART_ITEM)
            {
                _startPoint = collision.gameObject.transform.position;
            }
        }
    }
}
