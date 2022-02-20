using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script.Game
{
    public partial class GameController
    {
        [Header("ゴールスコア")]
        public int GoalScore;

        [Header("TextCurrentScore")]
        public Text TextCurrentScore;

        [Header("TextGoalScore")]
        public Text TextGoalScore;

        [Header("TextHHmm")]
        public Text TextHHmm;

        [Header("Textmmm")]
        public Text Textmmm;

        [Header("TextJumpLevel")]
        public Text TexJumpLevel;

        [Header("TextSpeedLevel")]
        public Text TexSpeedLevel;

        [Header("ImageWallJump")]
        public Image ImageWallJump;
    }
}
