using UnityEngine;

namespace Assets.Script.Collider
{
    public class BlockTriggerColliderController : ColliderController
    {
        [Header("管理対象")]
        public GameObject Target;

        private AudioSource _audioSource;
        private SpriteRenderer _spriteRend;
        private Collider2D _collider;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _spriteRend = GetComponent<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_collider.enabled == false)
            {
                return;
            }

            if (collision.tag == Tag.PLAYER)
            {
                if (_audioSource == null)
                {
                    return;
                }

                _audioSource.Play();

                if (Target != null)
                {
                    Target.SetActive(true);
                }

                _spriteRend.enabled = false;
                _collider.enabled = false;
            }
        }
    }
}
