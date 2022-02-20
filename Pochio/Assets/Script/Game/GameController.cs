using UnityEngine;

namespace Assets.Script.Game
{
    /// <summary>
    /// ゲーム制御クラス（シングルトン）
    /// </summary>
    public partial class GameController : MonoBehaviour
    {
        /// <summary>
        /// 自身のインスタンス
        /// </summary>
        private static GameController _gameController;
        public static GameController GetInstance
        {
            get { return _gameController; }
        }

        private void Start()
        {
            if (_gameController == null)
            {
                _gameController = this;
            }

            TimerStart();
        }

        private void Update()
        {
            UpdateTimer();
            UpdateScore();
        }

        /// <summary>
        /// ジャンプ回数表示を設定する
        /// </summary>
        /// <param name="count"></param>
        public void SetJumpCount(string count)
        {
        }
    }
}