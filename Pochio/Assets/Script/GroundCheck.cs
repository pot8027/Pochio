using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private static readonly string GROUND_TAG = "Ground";

    private bool _isGround = false;
    private bool _isEnter = false;
    private bool _isStay = false;
    private bool _isExit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsGround()
    {
        if (_isEnter || _isStay)
        {
            _isGround = true;
        }
        else if (_isExit)
        {
            _isGround = false;
        }

        _isEnter = false;
        _isStay = false;
        _isExit = false;

        return _isGround;
    }

    public void Reset()
    {
        _isGround = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals(GROUND_TAG))
        {
            //Debug.Log("何かが入った");
            _isEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals(GROUND_TAG))
        {
            //Debug.Log("何かが入っている");
            _isStay = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals(GROUND_TAG))
        {
            //Debug.Log("何かが出た");
            _isExit = true;
        }
        
    }
}
