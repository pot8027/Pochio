using UnityEngine;

namespace Assets.Script.Player.Controll
{
    public class KeyWallCheck : GroundCheck
    {
        protected override bool IsTarget(Collider2D collision)
        {
            if (collision.tag == Tag.GROUND)
            {
                var keyWallScript = collision.GetComponent<KeyWall>();
                if (keyWallScript != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
