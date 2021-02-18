using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲーム制御クラス（シングルトン）
/// </summary>
public class GameController : MonoBehaviour
{
    private static GameController _gameController;
    public static GameController GetInstance 
    {
        get { return _gameController; } 
    }

    void Awake()
    {
        if (_gameController == null)
        {
            _gameController = this;
        }
    }

    private void Update()
    {
    }

    /// <summary>
    /// ジャンプ回数表示を設定する
    /// </summary>
    /// <param name="count"></param>
    public void SetJumpCount(string count)
    {
    }
}
