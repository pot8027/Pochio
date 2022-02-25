using Assets.Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpCollider : MonoBehaviour
{
    [Header("ワープ先")]
    public GameObject WarpTarget;

    private AudioSource _warpAudioSrc;

    private void Start()
    {
        _warpAudioSrc = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Tag.PLAYER)
        {
            if (WarpTarget == null)
            {
                return;
            }

            WarpTarget.SetActive(true);

            if (_warpAudioSrc != null)
            {
                _warpAudioSrc.Play();
            }

            // ワープ
            var targetPosition = WarpTarget.transform.position;
            collision.transform.position = new Vector3(targetPosition.x + collision.transform.localScale.x, targetPosition.y, targetPosition.z);
        }
    }
}
