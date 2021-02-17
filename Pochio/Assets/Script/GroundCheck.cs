using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool _isGround = false;
    private bool _isEnter = false;
    private bool _isStay = false;
    private bool _isExit = false;

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
        if (collision.tag.Equals(Tag.GROUND))
        {
            _isEnter = true;
        }

        else if (collision.tag.Equals(Tag.MOVE_GROUND))
        {
            _isEnter = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals(Tag.GROUND))
        {
            _isStay = true;
        }
        else if (collision.tag.Equals(Tag.MOVE_GROUND))
        {
            _isStay = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals(Tag.GROUND))
        {
            _isExit = true;
        }
        else if (collision.tag.Equals(Tag.MOVE_GROUND))
        {
            _isExit = true;
        }
    }
}
