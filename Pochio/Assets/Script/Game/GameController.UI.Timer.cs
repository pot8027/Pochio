using System;
using System.Diagnostics;

namespace Assets.Script.Game
{
    public partial class GameController
    {
        /// <summary>
        /// タイマー用ストップウォッチ
        /// </summary>
        private Stopwatch _stopWatch = new Stopwatch();

        /// <summary>
        /// タイマー開始
        /// </summary>
        public void TimerStart()
        {
            _stopWatch.Start();
        }

        /// <summary>
        /// タイマー停止
        /// </summary>
        public void TimerStop()
        {
            _stopWatch.Stop();
        }

        /// <summary>
        /// タイマー情報取得
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetSpan()
        {
            return _stopWatch.Elapsed;
        }

        /// <summary>
        /// タイマー更新
        /// </summary>
        private void UpdateTimer()
        {
            var span = _stopWatch.Elapsed;
            TextHHmm.text = $"{string.Format("{0:00}", span.Minutes)}:{string.Format("{0:00}", span.Seconds)}";
            Textmmm.text = $"{string.Format("{0:00}", span.Milliseconds)}";
        }
    }
}
