using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearBlock : MonoBehaviour
{
    private BoxCollider2D _boxCollider2d = null;
    private Rigidbody2D _rididBody2d = null;
    private SpriteRenderer _spriteRenderer = null;

    private float _pointLeft, _pointRight;

    private void Start()
    {
        _boxCollider2d = GetComponent<BoxCollider2D>();
        _rididBody2d = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.enabled = false;

        var adjustPoint = _spriteRenderer.bounds.size.x / 5;
        _pointLeft = _rididBody2d.transform.position.x + adjustPoint;
        _pointRight = _rididBody2d.transform.position.x + _spriteRenderer.bounds.size.x - adjustPoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            var collisionRidid = collision.GetComponent<Rigidbody2D>();
            var sprite = collision.GetComponent<SpriteRenderer>();

            // 下からの衝突判定
            var collisionPosition = collisionRidid.transform.position;
            var collisionDirection = transform.InverseTransformPoint(collisionPosition);

            // ブロックの下面か判定
            var isXRange = _pointLeft <= collisionPosition.x + sprite.size.x && collisionPosition.x <= _pointRight;

            Debug.Log($"x={collisionDirection.x}, y={collisionDirection.y}");

            if (collisionDirection.y <= 0 && isXRange)
            {
                this.tag = "Ground";
                _spriteRenderer.enabled = true;
                _boxCollider2d.isTrigger = false;
            }
        }
    }
}
