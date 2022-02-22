using UnityEngine;

namespace Assets.Script.Player
{
    public partial class Player
    {
        /// <summary>
        /// プレイヤージャンプオーディオ再生
        /// </summary>
        private void PlayPlayerJumpAudio()
        {
            if (JumpAudioSouce == null)
            {
                return;
            }

            JumpAudioSouce.Play();
        }

        /// <summary>
        /// プレイヤー歩行オーディオ再生
        /// </summary>
        private void PlayPlayerRunAudio()
        {
            if (RunAudioSouce == null)
            {
                return;
            }

            if (RunAudioSouce.isPlaying)
            {
                return;
            }

            RunAudioSouce.Play();
        }

        /// <summary>
        /// プレイヤー歩行オーディオ停止
        /// </summary>
        private void StopPlayerRunAudio()
        {
            if (RunAudioSouce == null)
            {
                return;
            }

            if (RunAudioSouce.isPlaying == false)
            {
                return;
            }

            RunAudioSouce.Stop();
        }
    }
}
