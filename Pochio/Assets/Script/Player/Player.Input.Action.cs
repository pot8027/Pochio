using Assets.Script.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Player
{
    public partial class Player
    {
        private void InputAction()
        {
            if (_eastInputAction.IsPressed())
            {
                OpenKeyWall();
            }
        }

        /// <summary>
        /// 鍵壁を開く
        /// </summary>
        private void OpenKeyWall()
        {
            // 解錠対象接触中
            if (_isTouchingKeyWall)
            {
                var keywallScript = KeyWall.GroundObject.GetComponent<KeyWall>();
                if (keywallScript != null)
                {
                    // 必要数満たしていれば解錠
                    var keyNum = keywallScript.KeyNumber;
                    if (GameController.GetInstance.Score >= keyNum)
                    {
                        // スコア減算
                        GameController.GetInstance.RemoveScore(keyNum);

                        // 音を鳴らす
                        var openAudioClip = keywallScript.OpenAudioClip;
                        if (openAudioClip != null)
                        {
                            OneShotAudioSouce.PlayOneShot(openAudioClip);
                        }

                        // 破棄
                        Destroy(KeyWall.GroundObject);
                    }
                }
            }
        }
    }
}
