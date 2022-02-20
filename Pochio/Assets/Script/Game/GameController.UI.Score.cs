namespace Assets.Script.Game
{
    public partial class GameController
    {
        /// <summary>
        /// 現在のスコア
        /// </summary>
        private int _currentScore = 0;

        /// <summary>
        /// スコアを加算
        /// </summary>
        /// <param name="add">加算値</param>
        public void AddScore(int add = 1)
        {
            _currentScore += add;
        }

        /// <summary>
        /// ゲームクリア判定
        /// </summary>
        /// <returns></returns>
        public bool IsClear()
        {
            return _currentScore >= GoalScore;
        }

        /// <summary>
        /// スコア表示更新
        /// </summary>
        private void UpdateScore()
        {
            TextCurrentScore.text = _currentScore.ToString();
            TextGoalScore.text = $"/{GoalScore}";
        }
    }
}
