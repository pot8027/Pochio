using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// はしご重なりチェック
/// </summary>
public class LadderCheck : MonoBehaviour
{
    public bool IsLadderOn { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ladder")
        {
            IsLadderOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ladder")
        {
            IsLadderOn = false;
        }
    }
}
