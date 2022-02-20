using UnityEngine;

namespace Assets.Script.Player
{
    public partial class Player
    {
        private Animator _anim = null;
        private Rigidbody2D _rigidBody2D;
        private CapsuleCollider2D _capsuleCollider2D;

        private void InitializeComponents()
        {
            _anim = GetComponent<Animator>();
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        }
    }
}
