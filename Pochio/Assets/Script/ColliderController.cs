using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    public GameObject Parent;

    private Collider2D _collider;
    private Renderer _renderer;

    private void Awake()
    {
        _collider = Parent.GetComponent<Collider2D>();
        _renderer = Parent.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        _collider.enabled = false;
    }

    private void OnWillRenderObject()
    {
        if (Camera.current.name == "Main Camera")
        {
            _collider.enabled = true;
        }
    }
}
