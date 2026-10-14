using UnityEngine;
using UnityEngine.UI;

public class ProjectionSurface : MaskableGraphic
{
    [Header("Texture")]
    public Texture texture;

    [Header("Corners - normalized 0 to 1")]
    public Vector2 topLeft = new Vector2(0, 1);
    public Vector2 topRight = new Vector2(1, 1);
    public Vector2 bottomLeft = new Vector2(0, 0);
    public Vector2 bottomRight = new Vector2(1, 0);

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        Rect rect = rectTransform.rect;

        Vector2 tl = NormalizedToLocal(topLeft, rect);
        Vector2 tr = NormalizedToLocal(topRight, rect);
        Vector2 bl = NormalizedToLocal(bottomLeft, rect);
        Vector2 br = NormalizedToLocal(bottomRight, rect);

        UIVertex vertex = UIVertex.simpleVert;

        // Top Left
        vertex.position = tl;
        vertex.uv0 = new Vector2(0, 1);
        vertex.color = color;
        vh.AddVert(vertex);

        // Top Right
        vertex.position = tr;
        vertex.uv0 = new Vector2(1, 1);
        vh.AddVert(vertex);

        // Bottom Right
        vertex.position = br;
        vertex.uv0 = new Vector2(1, 0);
        vh.AddVert(vertex);

        // Bottom Left
        vertex.position = bl;
        vertex.uv0 = new Vector2(0, 0);
        vh.AddVert(vertex);

        // Two triangles
        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(2, 3, 0);
    }

    Vector2 NormalizedToLocal(Vector2 point, Rect rect)
    {
        return new Vector2(
            Mathf.Lerp(rect.xMin, rect.xMax, point.x),
            Mathf.Lerp(rect.yMin, rect.yMax, point.y)
        );
    }

    public override Texture mainTexture
    {
        get
        {
            return texture;
        }
    }

    public void SetCorners(
        Vector2 tl,
        Vector2 tr,
        Vector2 bl,
        Vector2 br)
    {
        topLeft = tl;
        topRight = tr;
        bottomLeft = bl;
        bottomRight = br;

        SetVerticesDirty();
    }
}