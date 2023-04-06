using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Penwyn.Tools;
namespace Penwyn.Game
{
    [CreateAssetMenu(menuName = "Util/InputReader")]
    public class InputReader : SingletonScriptableObject<InputReader>, PlayerInput.IPlayCardActions
    {

        public event UnityAction OnChooseStarted;
        public event UnityAction OnChooseCancelled;

        public event UnityAction OnCancelStarted;
        public event UnityAction OnCancelCancelled;

        public event UnityAction OnOpenIGMenuStarted;
        public event UnityAction OnOpenIGMenuCancelled;

        private PlayerInput _playerInput;


        private void OnEnable()
        {
            if (_playerInput == null)
            {
                _playerInput = new PlayerInput();
                _playerInput.PlayCard.SetCallbacks(this);
                EnablePlayCardInput();
            }
        }

        public void OnChoose(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.started)
                OnChooseStarted?.Invoke();
            if (context.canceled)
                OnChooseCancelled?.Invoke();
        }

        public void OnCancel(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.started)
                OnCancelStarted?.Invoke();
            if (context.canceled)
                OnCancelCancelled?.Invoke();
        }

        public void EnablePlayCardInput()
        {
            _playerInput.PlayCard.Enable();
        }

        public void DisablePlayCardInput()
        {
            _playerInput.PlayCard.Disable();
        }

        public void OnOpenIGMenu(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.started)
                OnOpenIGMenuStarted?.Invoke();
            if (context.canceled)
                OnOpenIGMenuCancelled?.Invoke();
        }
    }

}
