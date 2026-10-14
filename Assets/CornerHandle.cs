using UnityEngine;
using UnityEngine.EventSystems;

public class CornerHandle : MonoBehaviour, IDragHandler
{
    public ProjectionSurface projection;

    public enum Corner
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public Corner corner;

    public void OnDrag(PointerEventData eventData)
    {
        RectTransform canvas = projection.transform.parent as RectTransform;

        Vector2 localPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas,
            eventData.position,
            eventData.pressEventCamera,
            out localPos
        );

        Rect rect = canvas.rect;

        float x = Mathf.InverseLerp(rect.xMin, rect.xMax, localPos.x);
        float y = Mathf.InverseLerp(rect.yMin, rect.yMax, localPos.y);

        Vector2 point = new Vector2(x, y);

        switch (corner)
        {
            case Corner.TopLeft:
                projection.topLeft = point;
                break;

            case Corner.TopRight:
                projection.topRight = point;
                break;

            case Corner.BottomLeft:
                projection.bottomLeft = point;
                break;

            case Corner.BottomRight:
                projection.bottomRight = point;
                break;
        }

        projection.SetVerticesDirty();

        RectTransform handle = transform as RectTransform;
        handle.position = eventData.position;
    }
}