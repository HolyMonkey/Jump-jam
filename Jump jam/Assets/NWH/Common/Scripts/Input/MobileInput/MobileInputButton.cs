using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NWH.Common.Input
{
    /// <summary>
    ///     Adds clicked and pressed flags to the standard Unity UI Button.
    /// </summary>
    public class MobileInputButton : Button
    {
        public bool hasBeenClicked;
        public bool isPressed;


        private void LateUpdate()
        {
            hasBeenClicked = false;

            isPressed = false;
            for (int i = 0; i < UnityEngine.Input.touches.Length; i++)
            {
                isPressed |= ButtonContainsPosition(UnityEngine.Input.touches[i].position);
                if (isPressed)
                {
                    break;
                }
            }

            isPressed |= ButtonContainsPosition(UnityEngine.Input.mousePosition) && UnityEngine.Input.GetMouseButton(0);
        }


        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            hasBeenClicked = true;
        }


        public bool ButtonContainsPosition(Vector2 xPos)
        {
            RectTransform rt    = GetComponent<RectTransform>();
            float         fMinX = rt.position.x - rt.sizeDelta.x * 0.5f;
            float         fMaxX = rt.position.x + rt.sizeDelta.x * 0.5f;
            float         fMinY = rt.position.y - rt.sizeDelta.y * 0.5f;
            float         fMaxY = rt.position.y + rt.sizeDelta.y * 0.5f;

            if (xPos.x <= fMaxX && xPos.x >= fMinX)
            {
                if (xPos.y <= fMaxY && xPos.y >= fMinY)
                {
                    return true;
                }
            }

            return false;
        }
    }
}