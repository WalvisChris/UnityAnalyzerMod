using UnityEngine;

namespace ModdingHelper.CustomEditor.ColliderOutlines
{
    internal abstract class ColliderOutlineBase : MonoBehaviour
    {
        protected GameObject lineChildContainer;
        protected static readonly Color DefaultLineColor = Color.green;

        protected virtual void Awake()
        {
            lineChildContainer = new GameObject($"{GetType().Name}_Container");
            lineChildContainer.transform.SetParent(transform, false);

            InitializeOutline();
            UpdateOutline();
        }

        protected abstract void InitializeOutline();
        protected abstract void UpdateOutline();
        protected virtual void LateUpdate()
        {
            UpdateOutline();
        }

        protected Material CreateDefaultMaterial()
        {
            Material lineMat = new Material(Shader.Find("Sprites/Default"));
            lineMat.color = DefaultLineColor;
            return lineMat;
        }

        protected virtual void OnDestroy()
        {
            if (lineChildContainer != null)
            {
                Destroy(lineChildContainer);
            }
        }
    }
}