using System;
using System.Collections.Generic;
using UnityEngine;

namespace ModdingHelper.CustomEditor.ColliderOutlines
{
    internal class MeshColliderOutline : ColliderOutlineBase
    {
        private MeshCollider meshCollider;
        private List<LineRenderer> lineRenderers = new List<LineRenderer>();
        private List<Edge> meshEdges = new List<Edge>();

        private struct Edge
        {
            public int v1;
            public int v2;

            public Edge(int a, int b)
            {
                v1 = Math.Min(a, b);
                v2 = Math.Max(a, b);
            }

            public override bool Equals(object obj) => obj is Edge other && v1 == other.v1 && v2 == other.v2;
            public override int GetHashCode() => (v1 * 397) ^ v2;
        }

        protected override void InitializeOutline()
        {
            meshCollider = GetComponent<MeshCollider>();
            if (meshCollider == null || meshCollider.sharedMesh == null) return;

            ExtractMeshEdges(meshCollider.sharedMesh);

            Material lineMat = CreateDefaultMaterial();

            foreach (var edge in meshEdges)
            {
                GameObject segmentObj = new GameObject("EdgeSegment");
                segmentObj.transform.SetParent(lineChildContainer.transform, false);

                LineRenderer lr = segmentObj.AddComponent<LineRenderer>();
                lr.useWorldSpace = true;
                lr.startWidth = 0.03f;
                lr.endWidth = 0.03f;
                lr.positionCount = 2;
                lr.material = lineMat;
                lr.startColor = DefaultLineColor;
                lr.endColor = DefaultLineColor;

                lineRenderers.Add(lr);
            }
        }

        private void ExtractMeshEdges(Mesh mesh)
        {
            int[] triangles = mesh.triangles;
            HashSet<Edge> uniqueEdges = new HashSet<Edge>();

            for (int i = 0; i < triangles.Length; i += 3)
            {
                uniqueEdges.Add(new Edge(triangles[i], triangles[i + 1]));
                uniqueEdges.Add(new Edge(triangles[i + 1], triangles[i + 2]));
                uniqueEdges.Add(new Edge(triangles[i + 2], triangles[i]));
            }

            meshEdges.AddRange(uniqueEdges);
        }

        protected override void UpdateOutline()
        {
            if (meshCollider == null || meshCollider.sharedMesh == null) return;

            Vector3[] vertices = meshCollider.sharedMesh.vertices;

            for (int i = 0; i < meshEdges.Count; i++)
            {
                Edge edge = meshEdges[i];
                LineRenderer lr = lineRenderers[i];

                Vector3 worldV1 = transform.TransformPoint(vertices[edge.v1]);
                Vector3 worldV2 = transform.TransformPoint(vertices[edge.v2]);

                lr.SetPosition(0, worldV1);
                lr.SetPosition(1, worldV2);
            }
        }
    }
}