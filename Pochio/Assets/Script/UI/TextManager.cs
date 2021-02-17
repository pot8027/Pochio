using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// テキスト制御
/// </summary>
public class TextManager : MonoBehaviour
{
    [Header("テキストオブジェクト")]
    public GameObject Obj = null;

    private Text _text = null;

    /// <summary>
    /// テキストを設定する
    /// </summary>
    /// <param name="newText">設定するテキスト</param>
    public void SetText(string newText)
    {
        if (Obj != null)
        {
            if (_text == null)
            {
                _text = Obj.GetComponent<Text>();
            }
            _text.text = newText;
        }
    }
}
