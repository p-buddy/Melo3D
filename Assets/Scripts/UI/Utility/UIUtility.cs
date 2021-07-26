using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI
{
    public static class UIUtility
    {
        public static Vector2 GetSizeOfActiveChildren(Transform transform)
        {
            float width = 0f;
            float height = 0f;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                if (!childTransform.gameObject.activeSelf)
                {
                    continue;
                }
                RectTransform childRectTransform = childTransform.GetComponent<RectTransform>();
                if (childRectTransform)
                {
                    Rect childRect = childRectTransform.rect;
                    width += childRect.width;
                    height += childRect.height;
                }
            }

            return new Vector2(width, height);
        }

        public static RectTransform[] GetActiveChildRectTransforms(Transform transform)
        {
            List<RectTransform> childRectTransforms = new List<RectTransform>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform childTransform = transform.GetChild(i);
                if (!childTransform.gameObject.activeSelf)
                {
                    continue;
                }
                childRectTransforms.AddRange(childTransform.GetComponentsInChildren<RectTransform>(false));
            }
            return childRectTransforms.ToArray();
        }

        public static RectTransform GetTallestChildRectTransform(Transform transform)
        {
            RectTransform[] childRectTransforms = GetActiveChildRectTransforms(transform);
            if (childRectTransforms.Length == 0)
            {
                return null;
            }
            RectTransform tallest = childRectTransforms[0];

            for (var index = 1; index < childRectTransforms.Length; index++)
            {
                RectTransform childRectTransform = childRectTransforms[index];
                if (!childRectTransform.gameObject.activeSelf)
                {
                    continue;
                }
                if (childRectTransform.rect.height > tallest.rect.height)
                {
                    tallest = childRectTransform;
                }
            }

            return tallest;
        }

        public static float GetTallestChildHeight(Transform transform)
        {
            RectTransform tallestRectTransform = GetTallestChildRectTransform(transform);
            if (!tallestRectTransform)
            {
                return 0f;
            }
            return tallestRectTransform.rect.height;
        }

        public static bool TryGetFloat(TMP_InputField inputField, out float value)
        {
            string text = inputField.text;
            bool valid = float.TryParse(text, out float result);
            if (valid)
            {
                value = result;
                return true;
            }

            value = default;
            return false;
        }

        public static void InvalidateInput(TMP_InputField inputField)
        {
            
        }
        
        public static bool ProcessAndValidateFloatInput(TMP_InputField inputField, out float value)
        {
            if (TryGetFloat(inputField, out value))
            {
                return true;
            }
            
            InvalidateInput(inputField);
            return false;
        }
    }
}