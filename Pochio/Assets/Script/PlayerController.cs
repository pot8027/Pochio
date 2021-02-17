using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidBody2D;
    //private float _jumpPower = 200.0f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidBody2D.AddForce(new Vector2(0, 200.0f));
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
              _rigidBody2D.AddForce(new Vector2(-100.0f, 0));
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _rigidBody2D.AddForce(new Vector2(100.0f, 0));
        }
    }
}
