using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
namespace Penwyn.Tools
{

    [CreateAssetMenu(menuName = "Util/Cursor")]
    public class CursorUtility : SingletonScriptableObject<CursorUtility>
    {
        [SerializeField] Texture2D normalCursor;
        [SerializeField] Texture2D targettingCursor;
        [SerializeField] Texture2D inspectingCursor;

        public static Vector3 GetMousePosition()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            mousePos.z = 0;
            return mousePos;
        }

        public GameObject GetObjectUnderMouse()
        {
            Vector3 mousePos = GetMousePosition();
            Vector2 ray = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D hit2D = Physics2D.Raycast(ray, ray);
            if (hit2D.collider != null)
            {
                return hit2D.collider.gameObject;
            }
            return null;
        }

        public void ResetCursor()
        {
            ChangeCursorSprite(normalCursor);
        }

        public void ChangeToInspectingCursor()
        {
            ChangeCursorSprite(inspectingCursor);
        }

        public void ChangeToTargetCursor()
        {
            ChangeCursorSprite(targettingCursor);
        }

        public void ChangeCursorSprite(Texture2D cursorSprite)
        {
            Cursor.SetCursor(cursorSprite, Vector3.zero * cursorSprite.height / 2f, CursorMode.Auto);
        }

        public void ShowCursor()
        {
            Cursor.visible = true;
        }

        public void HideCursor()
        {
            Cursor.visible = false;
        }

        public Texture2D CurrentCursor { set { ChangeCursorSprite(value); } }

    }
}
