using UnityEngine;

namespace Assets.Script.Player
{
    public partial class Player
    {
        /// <summary>
        /// アイテム効果音を再生する
        /// </summary>
        /// <param name="audioClip"></param>
        private void PlayItemSound(AudioClip audioClip)
        {
            if (audioClip == null)
            {
                return;
            }

            ItemAudioSrc.PlayOneShot(audioClip);
        }

        /// <summary>
        /// プレイヤー効果音を再生する
        /// </summary>
        /// <param name="audioClip"></param>
        private void PlayPlayerSound(AudioClip audioClip)
        {
            if (audioClip == null)
            {
                return;
            }

            PlayerAudioSrc.PlayOneShot(audioClip);
        }
    }
}
