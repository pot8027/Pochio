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

            // ジャンプ回数アイテム
            if (collision.tag == Tag.JUMP_ITEM)
            {
                collision.enabled = false;
                JumpMaxCount++;
                Destroy(collision.gameObject);
            }

            // スピードアップアイテム
            else if (collision.tag == Tag.SPEED_ITEM)
            {
                collision.enabled = false;
                SpeedX += 2f;
                Destroy(collision.gameObject);
            }

            // スコアアイテム
            else if (collision.tag == Tag.CHERRY_ITEM)
            {
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
