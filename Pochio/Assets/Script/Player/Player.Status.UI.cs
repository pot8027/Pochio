using Assets.Script.Game;

namespace Assets.Script.Player
{
    public partial class Player
    {
        private void UpdatePlayerStatusUI()
        {
            GameController.GetInstance.SetJumpLevelText(JumpLevel);
            GameController.GetInstance.SetSpeedLevelText(SpeedLevel);
            GameController.GetInstance.SetWallJumpImage(CanWallJump);
        }
    }
}
