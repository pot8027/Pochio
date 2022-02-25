using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script
{
    public class KeyWall : MonoBehaviour
    {
        [Header("必要なアイテム数")]
        public int KeyNumber = 0;

        [Header("開場時音")]
        public AudioClip OpenAudioClip;

        [Header("表示テキスト")]
        public Text DisplayText;

        private void Start()
        {
            DisplayText.text = $"{KeyNumber}";
        }
    }
}
