using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    [Header("取得時効果音")]
    public AudioClip AudioClip = null;

    public GameObject Parent;

    private Collider2D _collider;
    private Renderer _renderer;
    private AudioSource _audioSource;

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
