using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public GameObject GroundObject { get; private set; }

    protected bool _isGround = false;
    protected bool _isEnter = false;
    protected bool _isStay = false;
    protected bool _isExit = false;

    protected Transform _transform;
    protected Vector3 _defaultScale;

    public void Awake()
    {
        _transform = transform;
        _defaultScale = new Vector3(_transform.localScale.x, _transform.localScale.y, _transform.localScale.z);
    }

    public void ResetSlace()
    {
        transform.localScale = _defaultScale;
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

    protected virtual bool IsTarget(Collider2D collision)
    {
        if (collision.tag.Equals(Tag.GROUND))
        {
            return true;
        }

        else if (collision.tag.Equals(Tag.MOVE_GROUND))
        {
            return true;
        }

        return false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsTarget(collision))
        {
            _isEnter = true;
            GroundObject = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (IsTarget(collision))
        {
            _isStay = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (IsTarget(collision))
        {
            _isExit = true;
            GroundObject = null;
        }
    }
}
