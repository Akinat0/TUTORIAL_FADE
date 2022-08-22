namespace Abu
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Component which will render added holes in UI.
    /// </summary>
    [AddComponentMenu("UI/Tutorial Fade Image")]
    public class TutorialFadeImage : Image
    {
        /// <summary>
        /// Id for holes rect array property in UITutorialFade.shared.
        /// </summary>
        static readonly int HolesID = Shader.PropertyToID("_Holes");
        /// <summary>
        /// Id for FadeImage's aspect ration property in UITutorialFade.shared.
        /// </summary>
        static readonly int AspectID = Shader.PropertyToID("_Aspect");
        /// <summary>
        /// Id for count of active holes property in UITutorialFade.shared.
        /// </summary>
        static readonly int HolesLengthID = Shader.PropertyToID("_HolesLength");
        /// <summary>
        /// Id for hole's edge smoothness (i.e. size of the hole) property in UITutorialFade.shared.
        /// </summary>
        static readonly int SmoothnessID = Shader.PropertyToID("_Smoothness");

        /// <summary>
        /// Max hole size. Could be changed, however you should change the "5" value in UITutorialFade.shared file also.
        /// </summary>
        const int HolesSize = 5;
        
        readonly List<TutorialHole> Holes = new List<TutorialHole>(HolesSize);
        
        Material cachedMaterial;

        /// <summary>
        /// Material to render shader. Shader must support all parameters described above.
        /// </summary>
        public override Material material =>
            cachedMaterial ? cachedMaterial : cachedMaterial = new Material(Shader.Find("UI/TutorialFade"));
        
        bool isDirty;

        readonly Vector4[] holesBuffer = new Vector4[HolesSize];

        [SerializeField, Range(0, 1), Tooltip("Hole edge smoothness (i.e. size of the hole)")] float smoothness = 0.01f;

        /// <summary>
        /// Hole edge smoothness (i.e. size of the hole). Should be greater then 0 
        /// </summary>
        public float Smoothness
        {
            get => smoothness;
            set
            {
                if (Mathf.Approximately(smoothness, value))
                    return;

                smoothness = value;
                SetDirtyMaterial();
            }
        }

        void LateUpdate()
        {
            foreach (TutorialHole hole in Holes.Where(hole => hole.IsAutoUpdateEnabled))
                hole.UpdateRect();
            
            UpdateMaterialData();
        }
        
        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (cachedMaterial != null)
                DestroyImmediate(cachedMaterial);
        }
        
        protected override void OnDidApplyAnimationProperties()
        {
            base.OnDidApplyAnimationProperties();

            SetDirtyMaterial();
        }

#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();

            SetDirtyMaterial();
        }

        [UnityEditor.MenuItem("GameObject/UI/Tutorial Fade Image", false)]
        static void Create()
        {
            GameObject gameObject = new GameObject("Tutorial Fade", typeof(TutorialFadeImage));
            gameObject.transform.SetParent(UnityEditor.Selection.activeTransform);
            
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            TutorialFadeImage image = gameObject.GetComponent<TutorialFadeImage>();
            image.color = new Color(0f, 0f, 0f, 0.39f);
        }

#endif

        /// <summary>
        /// Each added tutorial hole will be rendered on this FadeImage. 
        /// </summary>
        /// <param name="hole">The hole which will be rendered on fade image. Shouldn't be null.</param>
        public void AddHole(TutorialHole hole)
        {
            if(hole == null || Holes.Contains(hole))
                return;
            
            Holes.Add(hole);
            hole.RectChanged += SetDirtyMaterial;
            SetDirtyMaterial();
        }

        /// <summary>
        /// Removed tutorial hole won't rendered on this FadeImage.  
        /// </summary>
        /// <param name="hole">The hole which won't be rendered on this fade image.</param>
        public void RemoveHole(TutorialHole hole)
        {
            if(hole == null || !Holes.Contains(hole))
                return;
            
            hole.RectChanged -= SetDirtyMaterial;
            Holes.Remove(hole);
            SetDirtyMaterial();
        }
        
        /// <summary>
        /// Used to pass events within holes and prevent other events. 
        /// </summary>
        /// <param name="eventPosition"></param>
        /// <param name="eventCamera"></param>
        /// <returns></returns>
        public override bool IsRaycastLocationValid(Vector2 eventPosition, Camera eventCamera)
        {
            Vector3 worldEventPosition;
            
            if (!RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, eventPosition, eventCamera, out worldEventPosition))
                return false;
            
            return !Holes.Any(hole =>
                hole.GetWorldRect().Contains(worldEventPosition));
        }
        
        /// <summary>
        /// Dirty materials will be Updated in LateUpdate 
        /// </summary>
        void SetDirtyMaterial()
        {
            isDirty = true;
        }

        /// <summary>
        /// Updates dirty material.
        /// </summary>
        void UpdateMaterialData()
        {
            if (!isDirty)
                return;

            if (Holes.Count > HolesSize)
                Debug.LogError($"Max holes size is {HolesSize}");

            material.SetInt(HolesLengthID, Holes.Count);
            material.SetFloat(SmoothnessID, smoothness);

            Rect worldRect = rectTransform.TransformRect(rectTransform.rect);

            for (int i = 0; i < HolesSize; i++)
            {
                if (i < Holes.Count)
                    holesBuffer[i] = GetRectVectorRelative(Holes[i].GetWorldRect(), worldRect);
                else
                    holesBuffer[i] = Vector4.zero;
            }

            float aspect = worldRect.width / worldRect.height;

            material.SetFloat(AspectID, aspect);
            material.SetVectorArray(HolesID, holesBuffer);

            isDirty = false;
        }

        /// <summary>
        /// Returns rect relative to FadeImage rect in world.
        /// </summary>
        /// <param name="holeRect">World rect of the hole.</param>
        /// <param name="worldRect">World rect of fade image</param>
        /// <returns>holeRect, but local for FadeImage's world rect.</returns>
        Vector4 GetRectVectorRelative(Rect holeRect, Rect worldRect)
        {
            float xMin = Remap(holeRect.x, worldRect.x, worldRect.x + worldRect.width, 0, 1);
            float yMin = Remap(holeRect.y, worldRect.y, worldRect.y + worldRect.height, 0, 1);
            float xMax = Remap(holeRect.x + holeRect.width, worldRect.x, worldRect.x + worldRect.width, 0, 1);
            float yMax = Remap(holeRect.y + holeRect.height, worldRect.y, worldRect.y + worldRect.height, 0, 1);

            return new Vector4(xMin, yMin, xMax, yMax);
        }

        /// <summary>
        /// Remaps value in linear space.
        /// </summary>
        static float Remap(float value, float from1, float to1, float from2, float to2)
            => (value - from1) / (to1 - from1) * (to2 - from2) + from2;

    }
}

