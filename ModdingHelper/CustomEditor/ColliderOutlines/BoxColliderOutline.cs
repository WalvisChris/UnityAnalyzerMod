using UnityEngine;

namespace ModdingHelper.CustomEditor.ColliderOutlines
{
    internal class BoxColliderOutline : ColliderOutlineBase
    {
        private BoxCollider boxCollider;
        private LineRenderer lineRenderer;

        private static readonly Vector3[] BoxVertices = new Vector3[]
        {
            new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),   new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),  new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),  new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),    new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),    new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, -0.5f, 0.5f),  new Vector3(-0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f)
        };

        protected override void InitializeOutline()
        {
            boxCollider = GetComponent<BoxCollider>();
            if (boxCollider == null) return;

            lineRenderer = lineChildContainer.AddComponent<LineRenderer>();
            lineRenderer.useWorldSpace = true;
            lineRenderer.startWidth = 0.03f;
            lineRenderer.endWidth = 0.03f;
            lineRenderer.positionCount = BoxVertices.Length;
            lineRenderer.material = CreateDefaultMaterial();
            lineRenderer.startColor = DefaultLineColor;
            lineRenderer.endColor = DefaultLineColor;
        }

        protected override void UpdateOutline()
        {
            if (boxCollider == null || lineRenderer == null) return;

            Vector3 center = boxCollider.center;
            Vector3 size = boxCollider.size;

            for (int i = 0; i < BoxVertices.Length; i++)
            {
                Vector3 localPoint = center + Vector3.Scale(BoxVertices[i], size);
                Vector3 worldPoint = transform.TransformPoint(localPoint);
                lineRenderer.SetPosition(i, worldPoint);
            }
        }
    }
}