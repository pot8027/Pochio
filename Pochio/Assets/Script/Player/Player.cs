using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Script.Player
{
    public partial class Player : MonoBehaviour
    {
        public GameObject GoalText;

        // 移動床
        private MoveLand _moveLand = null;

        // 開始位置
        private Vector2 _startPoint;

        /// <summary>
        /// 初回フレーム実行時イベント
        /// </summary>
        void Start()
        {
            InitializeComponents();
            InitializeInputAction();
        }

        private void Update()
        {
            // シーンリセット
            if (_shareInputAction.WasPressedThisFrame())
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        /// <summary>
        /// フレームイベント
        /// </summary>
        void FixedUpdate()
        {
            // 衝突情報更新
            {
                UpdateGroundStatus();
                UpdateFrontWallStatus();
                UpdateHeadStatus();
                UpdateLadderStatus();
            }

            // 移動更新
            {
                UpdateVelocity();
            }

            // アニメーション更新
            {
                UpdateAnimation();
            }
        }

        private void OnGUI()
        {
            //GUILayout.Label($"wasPressedThisFrame:{_southInputAction.WasPressedThisFrame()}");
            //GUILayout.Label($"isPressed:{_southInputAction.IsPressed()}");
            //GUILayout.Label($"_isGround:{_isGround}");
            //GUILayout.Label($"_isJump:{_isJump}");
            //GUILayout.Label($"_isFall:{_isFall}");
            //GUILayout.Label($"_canJumpKey:{_canJumpKey}");
            GUILayout.Label($"SpeedX:{GetInputX()}");
            GUILayout.Label($"SpeedY:{GetInputY()}");
        }
    }
}