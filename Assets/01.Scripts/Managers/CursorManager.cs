using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

using Penwyn.Tools;
namespace Penwyn.Game
{
    public class CursorManager : SingletonMonoBehaviour<CursorManager>
    {
        public Texture2D NormalCursor;
        public Texture2D TargettingCursor;

        protected bool _isPermanentlyHide;

        public virtual void ShowMouse()
        {
            ChangeMouseMode(true);
        }

        public virtual void HideMouse()
        {
            if (_isPermanentlyHide)
                ChangeMouseMode(false);
        }

        protected virtual void ChangeMouseMode(bool isVisible, CursorLockMode lockMode = CursorLockMode.Confined)
        {
            Cursor.visible = isVisible;
            Cursor.lockState = lockMode;
        }

        public Vector3 GetMousePosition()
        {
            Vector3 mousePos = Mouse.current.position.ReadValue();
            mousePos.z = Camera.main.farClipPlane * 0.5F;
            return Camera.main.ScreenToWorldPoint(mousePos);
        }

        public RaycastHit GetRayHitUnderMouse()
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.transform.position, GetMousePosition() - Camera.main.transform.position, out hit);
            Debug.DrawRay(Camera.main.transform.position, hit.point - Camera.main.transform.position, Color.red);
            Debug.DrawRay(hit.point, Vector3.up * 1000, Color.green);
            return hit;
        }

        public void ResetCursor()
        {
            ChangeCursorSprite(NormalCursor);
        }

        public void ChangeCursorSprite(Texture2D cursorSprite)
        {
            Cursor.SetCursor(cursorSprite, Vector3.zero * cursorSprite.height / 2f, CursorMode.Auto);
        }


        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (SceneManager.Instance != null)
            {
                if (scene.name == SceneManager.Instance.MatchSceenName)
                {
                    _isPermanentlyHide = true;
                    HideMouse();
                }
                else
                {
                    _isPermanentlyHide = false;
                    ShowMouse();
                }
            }
        }

        public virtual void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
            InputReader.Instance.ChangeMouseVisibilityPressed += ShowMouse;
            InputReader.Instance.ChangeMouseVisibilityReleased += HideMouse;
        }

        public virtual void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
            InputReader.Instance.ChangeMouseVisibilityPressed -= ShowMouse;
            InputReader.Instance.ChangeMouseVisibilityReleased -= HideMouse;
        }
    }
}

