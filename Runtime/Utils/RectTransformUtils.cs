using System;

namespace UniGame.Utils
{
    using System.Collections.Generic;
    using UnityEngine;

    public static class RectTransformUtils
    {
        public static readonly Vector2 Center = new Vector2(.5f, .5f);
        public static readonly Vector2 Left = new Vector2(0f, .5f);
        public static readonly Vector2 Right = new Vector2(1f, .5f);
        public static readonly Vector2 Top = new Vector2(.5f, 1f);
        public static readonly Vector2 Bottom = new Vector2(.5f, 0f);
        public static readonly Vector2 TopLeft = new Vector2(0f, 1f);
        public static readonly Vector2 TopRight = new Vector2(1f, 1f);
        public static readonly Vector2 BottomLeft = new Vector2(0f, 0f);
        public static readonly Vector2 BottomRight = new Vector2(1f, 0f);

        public static IEnumerable<(string, Vector2)> GetAnchorPresets()
        {
            return new List<(string, Vector2)>
            {
                ("Center", new Vector2(.5f, .5f)),
                ("Left", new Vector2(0f, .5f)),
                ("Right", new Vector2(1f, .5f)),
                ("Top", new Vector2(.5f, 1f)),
                ("Bottom", new Vector2(.5f, 0f)),
                ("TopLeft", new Vector2(0f, 1f)),
                ("TopRight", new Vector2(1f, 1f)),
                ("BottomLeft", new Vector2(0f, 0f)),
                ("BottomRight", new Vector2(1f, 0f))
            };
        }
        
        
        private static Vector3[] corners = new Vector3[4];

        public static Rect GetScreenPositionFromRect(this RectTransform rt, Camera camera)
        {
            // getting the world corners
            rt.GetWorldCorners(corners);
             
            // getting the screen corners
            for (var i = 0; i < corners.Length; i++)
                corners[i] = camera.WorldToScreenPoint(corners[i]);
             
            // getting the top left position of the transform
            var position = (Vector2) corners[1];
            // inverting the y axis values, making the top left corner = 0.
            position.y = Screen.height - position.y;
            // calculate the siz, width and height, in pixle format
            var size = corners[2] - corners[0];
             
            return new Rect(position, size);
        }


        public static void ResetAnchors(this RectTransform rectTransform)
        {
            rectTransform.anchorMin = Center;
            rectTransform.anchorMax = Center;
            rectTransform.pivot = Center;
        }

        public static void CreateRectTransformData(
            this Transform transform,
            out RectTransformData data)
        {
            data = new RectTransformData();
            if (transform is not RectTransform rectTransform)
                return;

            data.isRectTransform = true;
            data.rectTransform = rectTransform;
            data.anchoredPosition = rectTransform.anchoredPosition;
            data.anchoredPosition3D = rectTransform.anchoredPosition3D;
            data.anchorMax = rectTransform.anchorMax;
            data.anchorMin = rectTransform.anchorMin;
            data.offsetMax = rectTransform.offsetMax;
            data.offsetMin = rectTransform.offsetMin;
            data.pivot = rectTransform.pivot;
            data.rect = rectTransform.rect;
            data.sizeDelta = rectTransform.sizeDelta;
        }

        public static void ApplyRectTransformData(this Transform transform, ref RectTransformData data)
        {
            if (transform is not RectTransform rectTransform)
                return;
            
            rectTransform.anchoredPosition = data.anchoredPosition;
            rectTransform.anchoredPosition3D = data.anchoredPosition3D;
            rectTransform.anchorMax = data.anchorMax;
            rectTransform.anchorMin = data.anchorMin;
            rectTransform.offsetMax = data.offsetMax;
            rectTransform.offsetMin = data.offsetMin;
            rectTransform.pivot = data.pivot;
            rectTransform.sizeDelta = data.sizeDelta;
        }
    }

    [Serializable]
    public struct RectTransformData
    {
        public RectTransform rectTransform;
        public bool isRectTransform;

        public Vector2
            anchoredPosition; //The position of the pivot of this RectTransform relative to the anchor reference point.

        public Vector3
            anchoredPosition3D; //The 3D position of the pivot of this RectTransform relative to the anchor reference point.

        public Vector2
            anchorMax; //The normalized position in the parent RectTransform that the upper right corner is anchored to.

        public Vector2
            anchorMin; //The normalized position in the parent RectTransform that the lower left corner is anchored to.

        public Vector2
            offsetMax; //The offset of the upper right corner of the rectangle relative to the upper right anchor.

        public Vector2
            offsetMin; //The offset of the lower left corner of the rectangle relative to the lower left anchor.

        public Vector2 pivot; //The normalized position in this RectTransform that it rotates around.
        public Rect rect; //The calculated rectangle in the local space of the Transform.
        public Vector2 sizeDelta; //The size of this RectTransform relative to the distances between the anchors.
    }
}