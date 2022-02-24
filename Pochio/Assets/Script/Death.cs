using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Script
{
    public class Death : MonoBehaviour
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
            var movePointCount = _movePointList.Count;
            var oldPoint = _rididBody2d.position;
            Vector2 nextPoint = _movePointList[0];

            while (true)
            {
                var currentPoint = _rididBody2d.position;
                isNextPoint = Vector2.Distance(currentPoint, nextPoint) <= 0.1f;
                if (isNextPoint)
                {
                    var random = UnityEngine.Random.Range(0, _movePointList.Count - 1);
                    nextPoint = _movePointList[random];                    
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
    }
}
