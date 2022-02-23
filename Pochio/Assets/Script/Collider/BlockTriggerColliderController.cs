using UnityEngine;

namespace Assets.Script.Collider
{
    public class BlockTriggerColliderController : ColliderController
    {
        [Header("管理対象")]
        public GameObject Target;

        private AudioSource _triggerAudioSource;
        private SpriteRenderer _spriteRend;
        private Collider2D _triggerCollider;

        private void Start()
        {
            _triggerAudioSource = GetComponent<AudioSource>();
            _spriteRend = GetComponent<SpriteRenderer>();
            _triggerCollider = GetComponent<Collider2D>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_triggerCollider.enabled == false)
            {
                return;
            }

            if (collision.tag == Tag.PLAYER)
            {
                if (_triggerAudioSource == null)
                {
                    return;
                }

                _triggerAudioSource.Play();

                if (Target != null)
                {
                    Target.SetActive(true);
                }

                _spriteRend.enabled = false;
                _triggerCollider.enabled = false;
            }
        }
    }
}
