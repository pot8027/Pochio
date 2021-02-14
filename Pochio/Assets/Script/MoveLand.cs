using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 移動する地面スクリプト
/// </summary>
public class MoveLand : MonoBehaviour
{
    [Header("移動ポイント")]
    public List<GameObject> MovePointObjectList = new List<GameObject>();

    [Header("移動速度")]
    public float MoveSpeed;

    /// <summary>移動するポイントリスト</summary>
    private List<Vector2> _movePointList;
    private Rigidbody2D _rididBody2d;

    private Vector2 _myVelocity;

    void Start()
    {
        _rididBody2d = GetComponent<Rigidbody2D>();
        _movePointList = MovePointObjectList.Select(r => new Vector2(r.transform.position.x, r.transform.position.y)).ToList();
        StartCoroutine(nameof(MoveCor));
    }

    IEnumerator MoveCor()
    {
        var isNextPoint = true;
        var currentPointIndex = 0;
        var movePointCount = _movePointList.Count;
        var oldPoint = _rididBody2d.position;

        while (true)
        {
            var currentPoint = _rididBody2d.position;
            var nextPoint = _movePointList[currentPointIndex];

            // 目的地に到達しているか判定
            if (Vector2.Distance(currentPoint, nextPoint) <= 0.1f)
            {
                // 次のポイントのインデックス計算
                if (isNextPoint)
                {
                    if (currentPointIndex == movePointCount - 1)
                    {
                        isNextPoint = false;
                        currentPointIndex--;
                    }
                    else
                    {
                        currentPointIndex++;
                    }
                }

                // 前のポイントのインデックス計算
                else
                {
                    if (currentPointIndex == 0)
                    {
                        isNextPoint = true;
                        currentPointIndex++;
                    }
                    else
                    {
                        currentPointIndex--;
                    }
                }
            }
            else
            {
                var toVector = Vector2.MoveTowards(currentPoint, nextPoint, MoveSpeed * Time.deltaTime);
                _rididBody2d.MovePosition(toVector);
            }

            _myVelocity = (currentPoint - oldPoint) / Time.deltaTime;

            oldPoint = _rididBody2d.position;

            yield return new WaitForFixedUpdate();
        }
    }

    /// <summary>
    /// X方向速度を取得する
    /// </summary>
    /// <returns></returns>
    public Vector2 GetVelocity()
    {
        return _myVelocity;
    }
}
