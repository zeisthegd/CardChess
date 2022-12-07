using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.Events;

using Penwyn.Game;

namespace Penwyn.Tools
{
    [CreateAssetMenu(menuName = "Util/Input Reader")]
    public class InputReader : SingletonScriptableObject<InputReader>, PlayerInput.IGameplayActions
    {
        #region Gameplay Input Events

        //Movement
        public event UnityAction<Vector2> Move;

        //Skills Using
        public event UnityAction NormalAttackPressed;
        public event UnityAction NormalAttackReleased;

        public event UnityAction SpecialAttackPressed;
        public event UnityAction SpecialAttackReleased;

        public event UnityAction GlidePressed;
        public event UnityAction GlideReleased;

        public event UnityAction JumpPressed;
        public event UnityAction JumpReleased;

        public event UnityAction ChangeMouseVisibilityPressed;
        public event UnityAction ChangeMouseVisibilityReleased;

        public event UnityAction GameplayInputEnabled;
        public event UnityAction GameplayInputDisabled;

        #endregion

        #region Logic Variables

        public bool IsHoldingNormalAttack { get; set; }
        public bool IsHoldingSpecialAttack { get; set; }
        public bool IsHoldingJump { get; set; }
        public bool IsHoldingGlide { get; set; }
        public bool IsHoldinghangeMouseVisibility { get; set; }

        #endregion

        private PlayerInput playerinput;

        void OnEnable()
        {
            if (playerinput == null)
            {
                playerinput = new PlayerInput();
                playerinput.Gameplay.SetCallbacks(this);
            }
        }

        public void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                MoveInput = context.ReadValue<Vector2>();
                Move?.Invoke(MoveInput);
            }
            else
                MoveInput = Vector2.zero;
        }

        public void OnNormalAttack(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.started)
            {
                IsHoldingNormalAttack = true;
                NormalAttackPressed?.Invoke();
            }
            else if (context.phase == UnityEngine.InputSystem.InputActionPhase.Canceled)
            {
                IsHoldingNormalAttack = false;
                NormalAttackReleased?.Invoke();
            }

        }

        public void OnSpecialAttack(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.started)
            {
                IsHoldingSpecialAttack = true;
                SpecialAttackPressed?.Invoke();
            }
            else if (context.phase == UnityEngine.InputSystem.InputActionPhase.Canceled)
            {
                IsHoldingSpecialAttack = false;
                SpecialAttackReleased?.Invoke();
            }
        }

        public void OnGlide(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GlidePressed?.Invoke();
                IsHoldingGlide = true;
            }
            else if (context.phase == UnityEngine.InputSystem.InputActionPhase.Canceled)
            {
                GlideReleased?.Invoke();
                IsHoldingGlide = false;
            }
        }

        public void OnJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {

            if (context.started)
            {
                JumpPressed?.Invoke();
                IsHoldingJump = true;
            }
            else if (context.phase == UnityEngine.InputSystem.InputActionPhase.Canceled)
            {
                JumpReleased?.Invoke();
                IsHoldingJump = false;
            }
        }


        public void OnChangeCursorVisibility(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.started)
            {
                ChangeMouseVisibilityPressed?.Invoke();
                IsHoldinghangeMouseVisibility = true;
            }
            else if (context.phase == UnityEngine.InputSystem.InputActionPhase.Canceled)
            {
                ChangeMouseVisibilityReleased?.Invoke();
                IsHoldinghangeMouseVisibility = false;
            }
        }

        public void OnLook(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {

        }

        public void EnableGameplayInput()
        {
            playerinput.Gameplay.Enable();
            GameplayInputEnabled?.Invoke();

        }

        public void DisableGameplayInput()
        {
            playerinput.Gameplay.Disable();
            GameplayInputDisabled?.Invoke();
        }

        void OnDisable()
        {
            DisableGameplayInput();
        }


        public Vector2 MoveInput { get; set; }
    }
}
