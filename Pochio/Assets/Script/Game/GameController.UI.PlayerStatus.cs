using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Game
{
    public partial class GameController
    {
        /// <summary>
        /// 速度レベルテキストを設定する
        /// </summary>
        /// <param name="speedLevel"></param>
        public void SetSpeedLevelText(int speedLevel)
        {
            var current = Convert.ToInt32(TexSpeedLevel.text);
            if (current != speedLevel)
            {
                TexSpeedLevel.text = $"{speedLevel}";
            }
        }

        /// <summary>
        /// ジャンプレベルテキストを設定する
        /// </summary>
        /// <param name="jumpLevel"></param>
        public void SetJumpLevelText(int jumpLevel)
        {
            var current = Convert.ToInt32(TexJumpLevel.text);
            if (current != jumpLevel)
            {
                TexJumpLevel.text = $"{jumpLevel}";
            }
        }

        /// <summary>
        /// 壁蹴りスキル表示を設定する
        /// </summary>
        /// <param name="enabled"></param>
        public void SetWallJumpImage(bool enabled)
        {
            if (ImageWallJump.enabled != enabled)
            {
                ImageWallJump.enabled = enabled;
            }
        }
    }
}
