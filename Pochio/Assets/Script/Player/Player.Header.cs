﻿using UnityEngine;

namespace Assets.Script.Player
{
    public partial class Player
    {
        [Header("移動速度")]
        public float SpeedX = 1.0f;

        [Header("落下速度")]
        public float Gravity = 1.0f;

        [Header("ジャンプ速度")]
        public float JumpSpeed = 1.0f;

        [Header("はしご移動速度")]
        public float LadderSpeed = 1.0f;

        [Header("ジャンプ可能高さ")]
        public float JumpHeight = 0.0f;

        [Header("ジャンプ可能時間")]
        public float JumpLimitTime = 0.0f;

        [Header("壁蹴りX速度可能時間")]
        public float WallJumpLimitTIme = 0.5f;

        [Header("連続ジャンプ可能数")]
        public int JumpMaxCount = 1;

        [Header("ダッシュ速度カーブ")]
        public AnimationCurve DashCurve;

        [Header("ジャンプ速度カーブ")]
        public AnimationCurve JumpCurve;

        [Header("落下速度カーブ")]
        public AnimationCurve FallCurve;

        [Header("接地チェック")]
        public GroundCheck Ground;

        [Header("頭ぶつけチェック")]
        public GroundCheck Head;

        [Header("前方チェック")]
        public GroundCheck FrontWall;

        [Header("はしご重なりチェック")]
        public LadderCheck Ladder;

        [Header("クリアテキスト")]
        public GameObject ClearText;
    }
}