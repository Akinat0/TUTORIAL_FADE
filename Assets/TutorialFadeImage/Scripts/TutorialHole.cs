namespace Abu
{
    using System;
    using UnityEngine;

    public abstract class TutorialHole
    {
        protected TutorialHole(bool isAutoUpdate = true)
        {
            IsAutoUpdateEnabled = isAutoUpdate;
        }

        public bool IsAutoUpdateEnabled { get; set; }

        public abstract void UpdateRect();
        
        public event Action RectChanged;

        public abstract Rect GetWorldRect();

        protected void InvokeRectChanged()
        {
            RectChanged?.Invoke();
        }
    }

    public class RectTutorialHole : TutorialHole
    {
        public RectTutorialHole(Rect worldRect, bool isAutoUpdate = true) : base(isAutoUpdate)
        {
            WorldRect = worldRect;
        }

        public Rect WorldRect { get; private set; }

        public void SetWorldRect(Rect worldRect)
        {
            if (worldRect == WorldRect)
                return;

            WorldRect = worldRect;
            InvokeRectChanged();
        }

        public override void UpdateRect()
        {
        }

        public override Rect GetWorldRect() => WorldRect;

    }

    public class RectTransformTutorialHole : TutorialHole
    {
        public RectTransformTutorialHole(RectTransform rectTransform, bool isAutoUpdate = true) : base(isAutoUpdate)
        {
            RectTransform = rectTransform;
            WorldRect = RectTransform.TransformRect(RectTransform.rect);
        }

        public RectTransform RectTransform { get; }
        Rect WorldRect { get; set; }

        public override Rect GetWorldRect() => WorldRect;

        public override void UpdateRect()
        {
            Rect rect = RectTransform.TransformRect(RectTransform.rect);
            
            if(WorldRect == rect)
                return;
            
            WorldRect = rect;
            InvokeRectChanged();
        }
    }
    
    public class RendererTutorialHole : TutorialHole
    {
        public RendererTutorialHole(Renderer renderer, TutorialFadeImage fadeImage, bool isAutoUpdate = true) : base(isAutoUpdate)
        {
            Renderer = renderer;
            FadeImage = fadeImage;
            WorldRect = CalculateRendererRect();
        }

        public Renderer Renderer { get; }
        public TutorialFadeImage FadeImage { get; }
        Rect WorldRect { get; set; }

        public override Rect GetWorldRect() => WorldRect;
        
        public override void UpdateRect()
        {
            Rect rect = CalculateRendererRect();
            
            if(WorldRect == rect)
                return;

            WorldRect = rect;
            InvokeRectChanged();
        }

        Rect CalculateRendererRect()
        {
            if(FadeImage == null || FadeImage.canvas == null)
                return Rect.zero;

            Bounds bounds = Renderer.bounds;

            Vector2 min = bounds.min;
            Vector2 max = bounds.max;

            if (FadeImage.canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                min = RectTransformUtility.WorldToScreenPoint(FadeImage.canvas.worldCamera, bounds.min);
                max = RectTransformUtility.WorldToScreenPoint(FadeImage.canvas.worldCamera, bounds.max);
            }

            return new Rect(min, new Vector2(max.x - min.x, max.y - min.y));
        }
    }
}