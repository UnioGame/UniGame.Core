namespace UniModules.UniCore.GizmosTools.Shapes
{
    using System.Diagnostics;
    using UnityEngine;

    public static class GizmosShape
    {
        [Conditional("UNITY_EDITOR")]
        public static void DrawBox(Vector3 center, float width, float height)
        {
            var   zeroPoint = center;
            float w         = 0;
            float h         = 0;
        
            w = width / 2f;
            h = height / 2f;
            Gizmos.DrawLine(new Vector3(zeroPoint.x - w, zeroPoint.y + 0.1f, zeroPoint.z - h), new Vector3(zeroPoint.x - w, zeroPoint.y + 0.1f, zeroPoint.z + h));
            Gizmos.DrawLine(new Vector3(zeroPoint.x + w, zeroPoint.y + 0.1f, zeroPoint.z - h), new Vector3(zeroPoint.x + w, zeroPoint.y + 0.1f, zeroPoint.z + h));
            Gizmos.DrawLine(new Vector3(zeroPoint.x - w, zeroPoint.y + 0.1f, zeroPoint.z - h), new Vector3(zeroPoint.x + w, zeroPoint.y + 0.1f, zeroPoint.z - h));
            Gizmos.DrawLine(new Vector3(zeroPoint.x - w, zeroPoint.y + 0.1f, zeroPoint.z + h), new Vector3(zeroPoint.x + w, zeroPoint.y + 0.1f, zeroPoint.z + h));
        
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawElipse(Vector3 center, float w, float h)
        {
            float theta   = 0;
            var   x       = w * Mathf.Cos(theta);
            var   y       = h * Mathf.Sin(theta);
            var   pos     = center + new Vector3(x, 0, y);
            var   newPos  = pos;
            var   lastPos = pos;
            for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
            {
                x      = w * Mathf.Cos(theta);
                y      = h * Mathf.Sin(theta);
                newPos = center + new Vector3(x, 0, y);
                Gizmos.DrawLine(pos, newPos);
                pos = newPos;
            }

            Gizmos.DrawLine(pos, lastPos);
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawRadius(Vector3 center, float radius)
        {
            DrawElipse(center, radius, radius);
        }
    
        [Conditional("UNITY_EDITOR")]
        public static void DrawCircle(Vector3 position,Quaternion rotation, float radius, Color color)
        {
            var transformMatrix = Matrix4x4.TRS(position, rotation, UnityEditor.Handles.matrix.lossyScale);
            
            using (new UnityEditor.Handles.DrawingScope(transformMatrix))
            {
                var defaultColor = UnityEditor.Handles.color;
                UnityEditor.Handles.color =color;
                UnityEditor.Handles.DrawWireDisc(Vector3.zero, Vector3.up, radius);
                UnityEditor.Handles.color = defaultColor;
            }
        }
        
        [Conditional("UNITY_EDITOR")]
        public static void DrawCircle(Vector3 position, float radius, Color color)
        {
            DrawCircle(position, Quaternion.identity, radius, color);
        }

    }
}
