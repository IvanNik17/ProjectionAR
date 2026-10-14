#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CornerHandle))]
public class CornerHandleEditor : Editor
{
    void OnSceneGUI()
    {
        CornerHandle handle = (CornerHandle)target;
        ProjectionSurface projection = handle.projection;

        if (projection == null)
            return;

        Vector2 point;

        switch (handle.corner)
        {
            case CornerHandle.Corner.TopLeft:
                point = projection.topLeft;
                break;

            case CornerHandle.Corner.TopRight:
                point = projection.topRight;
                break;

            case CornerHandle.Corner.BottomLeft:
                point = projection.bottomLeft;
                break;

            default:
                point = projection.bottomRight;
                break;
        }

        RectTransform canvas = projection.transform.parent as RectTransform;
        Rect rect = canvas.rect;

        Vector3 worldPos = canvas.TransformPoint(
            new Vector3(
                Mathf.Lerp(rect.xMin, rect.xMax, point.x),
                Mathf.Lerp(rect.yMin, rect.yMax, point.y),
                0
            )
        );

        EditorGUI.BeginChangeCheck();

        Vector3 newWorldPos = Handles.PositionHandle(
            worldPos,
            Quaternion.identity
        );

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(projection, "Move Projection Corner");

            Vector3 localPos = canvas.InverseTransformPoint(newWorldPos);

            float x = Mathf.InverseLerp(rect.xMin, rect.xMax, localPos.x);
            float y = Mathf.InverseLerp(rect.yMin, rect.yMax, localPos.y);

            Vector2 newPoint = new Vector2(x, y);

            switch (handle.corner)
            {
                case CornerHandle.Corner.TopLeft:
                    projection.topLeft = newPoint;
                    break;

                case CornerHandle.Corner.TopRight:
                    projection.topRight = newPoint;
                    break;

                case CornerHandle.Corner.BottomLeft:
                    projection.bottomLeft = newPoint;
                    break;

                case CornerHandle.Corner.BottomRight:
                    projection.bottomRight = newPoint;
                    break;
            }

            projection.SetVerticesDirty();

            EditorUtility.SetDirty(projection);
        }
    }
}

#endif