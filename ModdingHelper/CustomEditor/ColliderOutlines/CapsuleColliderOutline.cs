using System.Collections.Generic;
using UnityEngine;

namespace ModdingHelper.CustomEditor.ColliderOutlines
{
    internal class CapsuleColliderOutline : ColliderOutlineBase
    {
        private CapsuleCollider capsuleCollider;
        private List<LineRenderer> lineRenderers = new List<LineRenderer>();
        private const int Segments = 16;

        protected override void InitializeOutline()
        {
            capsuleCollider = GetComponent<CapsuleCollider>();
            if (capsuleCollider == null) return;

            Material lineMat = CreateDefaultMaterial();

            // We maken 4 lussen: 2 ringen (boven/onder) en 2 kruisbogen
            for (int i = 0; i < 4; i++)
            {
                GameObject loopObj = new GameObject($"CapsuleLoop_{i}");
                loopObj.transform.SetParent(lineChildContainer.transform, false);

                LineRenderer lr = loopObj.AddComponent<LineRenderer>();
                lr.useWorldSpace = true;
                lr.startWidth = 0.03f;
                lr.endWidth = 0.03f;
                lr.material = lineMat;
                lr.startColor = DefaultLineColor;
                lr.endColor = DefaultLineColor;

                lineRenderers.Add(lr);
            }
        }

        protected override void UpdateOutline()
        {
            if (capsuleCollider == null || lineRenderers.Count < 4) return;

            float radius = capsuleCollider.radius;
            float height = Mathf.Max(capsuleCollider.height, radius * 2f);
            float halfHeight = (height / 2f) - radius;
            Vector3 center = capsuleCollider.center;

            // Bepaal de assen op basis van CapsuleCollider.direction (0 = X, 1 = Y, 2 = Z)
            Vector3 dirUp = Vector3.up;
            Vector3 dirRight = Vector3.right;
            Vector3 dirForward = Vector3.forward;

            if (capsuleCollider.direction == 0) // X-as
            {
                dirUp = Vector3.right;
                dirRight = Vector3.up;
                dirForward = Vector3.forward;
            }
            else if (capsuleCollider.direction == 2) // Z-as
            {
                dirUp = Vector3.forward;
                dirRight = Vector3.right;
                dirForward = Vector3.up;
            }

            Vector3 topCenter = center + dirUp * halfHeight;
            Vector3 bottomCenter = center - dirUp * halfHeight;

            // Ring Boven
            DrawCircle(lineRenderers[0], topCenter, dirRight, dirForward, radius);
            // Ring Onder
            DrawCircle(lineRenderers[1], bottomCenter, dirRight, dirForward, radius);
            // Lengteboog 1
            DrawCapsuleSide(lineRenderers[2], topCenter, bottomCenter, dirRight, dirUp, radius);
            // Lengteboog 2
            DrawCapsuleSide(lineRenderers[3], topCenter, bottomCenter, dirForward, dirUp, radius);
        }

        private void DrawCircle(LineRenderer lr, Vector3 center, Vector3 axisA, Vector3 axisB, float radius)
        {
            lr.positionCount = Segments + 1;
            for (int i = 0; i <= Segments; i++)
            {
                float angle = (i / (float)Segments) * Mathf.PI * 2f;
                Vector3 localPoint = center + (axisA * Mathf.Cos(angle) + axisB * Mathf.Sin(angle)) * radius;
                lr.SetPosition(i, transform.TransformPoint(localPoint));
            }
        }

        private void DrawCapsuleSide(LineRenderer lr, Vector3 top, Vector3 bottom, Vector3 sideAxis, Vector3 upAxis, float radius)
        {
            int halfSeg = Segments / 2;
            lr.positionCount = (halfSeg + 1) * 2 + 1;
            int idx = 0;

            // Bovenste halve cirkel
            for (int i = 0; i <= halfSeg; i++)
            {
                float angle = (i / (float)halfSeg) * Mathf.PI;
                Vector3 localPoint = top + (sideAxis * Mathf.Cos(angle) + upAxis * Mathf.Sin(angle)) * radius;
                lr.SetPosition(idx++, transform.TransformPoint(localPoint));
            }

            // Onderste halve cirkel
            for (int i = 0; i <= halfSeg; i++)
            {
                float angle = Mathf.PI + (i / (float)halfSeg) * Mathf.PI;
                Vector3 localPoint = bottom + (sideAxis * Mathf.Cos(angle) + upAxis * Mathf.Sin(angle)) * radius;
                lr.SetPosition(idx++, transform.TransformPoint(localPoint));
            }

            // Sluit de lijn
            Vector3 startPoint = top + sideAxis * radius;
            lr.SetPosition(idx, transform.TransformPoint(startPoint));
        }
    }
}