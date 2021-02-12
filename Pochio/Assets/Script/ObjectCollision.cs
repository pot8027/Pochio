using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollision : MonoBehaviour
{
    [Header("踏んだときの跳ねる高さ")]
    public float BountHeight;

    public bool IsPlayerStepOn { get; }
}
