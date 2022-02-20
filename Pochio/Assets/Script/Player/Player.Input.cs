using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Script.Player
{
    public partial class Player
    {
        // 左アナログスティック
        private InputAction _leftStickInputAction = default(InputAction);

        // 右アナログスティック
        private InputAction _rightStickInputAction = default(InputAction);

        // 上アクションボタン押下
        private InputAction _northInputAction = default(InputAction);

        // 下アクションボタン押下
        private InputAction _southInputAction = default(InputAction);

        // 左アクションボタン押下
        private InputAction _westInputAction = default(InputAction);

        // 右アクションボタン押下
        private InputAction _eastInputAction = default(InputAction);

        // 左手前トリガー押下
        private InputAction _leftShouterInputAction = default(InputAction);

        // 左奥トリガー押下
        private InputAction _leftTriggerInputAction = default(InputAction);

        // 右手前トリガー押下
        private InputAction _rightShouterInputAction = default(InputAction);

        // 右奥トリガー押下
        private InputAction _rightTriggerInputAction = default(InputAction);

        // Optionボタン押下
        private InputAction _optionInputAction = default(InputAction);

        // Shareボタン押下
        private InputAction _shareInputAction = default(InputAction);

        private void InitializeInputAction()
        {
            var playerInput = GetComponent<PlayerInput>();

            playerInput.SwitchCurrentActionMap("Player");
            var actionMap = playerInput.currentActionMap;

            _leftStickInputAction = actionMap["LeftStick"];
            _rightStickInputAction = actionMap["RightStick"];
            _southInputAction = actionMap["SouthButton"];
            _eastInputAction = actionMap["EastButton"];
            _northInputAction = actionMap["NorthButton"];
            _westInputAction = actionMap["WestButton"];
            _leftShouterInputAction = actionMap["LeftShouter"];
            _leftTriggerInputAction = actionMap["LeftTrigger"];
            _rightShouterInputAction = actionMap["RightShouter"];
            _rightTriggerInputAction = actionMap["RightTrigger"];
            _optionInputAction = actionMap["Options"];
            _shareInputAction = actionMap["Share"];
        }

        private float GetInputX()
        {
            return _leftStickInputAction.ReadValue<Vector2>().x;
        }

        private float GetInputY()
        {
            return _leftStickInputAction.ReadValue<Vector2>().y;
        }
    }
}
