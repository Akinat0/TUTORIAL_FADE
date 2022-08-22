namespace Abu
{
    using UnityEngine;

    /// <summary>
    /// Tutorial hole which calculates world rect based on Renderer.bounds. <b>It's not recommended to use overlay canvas for this type of tutorial hole.</b>
    /// For world rect calculation FadeImage should be attached to canvas with camera.
    /// If you're using canvas Overlay render mode you still should serialize camera via debug inspector.
    /// </summary>
    public class RendererTutorialHole : TutorialHole
    {
        /// <summary>
        /// Creates RendererTutorialHole
        /// </summary>
        /// <param name="renderer">Renderer which bounds will be used for world rect calculation.</param>
        /// <param name="fadeImage">Fade image which will be used for hole rendering. For world rect calculation FadeImage should be attached to canvas with camera.
        /// If you're using canvas Overlay render mode you still should serialize camera via debug inspector. However it's not recommended to use overlay canvas for this type of tutorial hole</param>
        /// <param name="isAutoUpdate">Should world rect be auto updated every frame, or developer will manually update it to achieve better performance.</param>
        public RendererTutorialHole(Renderer renderer, TutorialFadeImage fadeImage, bool isAutoUpdate = true) : base(
            isAutoUpdate)
        {
            Renderer = renderer;
            FadeImage = fadeImage;
            WorldRect = CalculateRendererRect();
        }

        /// <summary>
        /// Renderer which bounds will be used for world rect calculation.
        /// </summary>
        public Renderer Renderer { get; }
        
        /// <summary>
        /// Fade image which will be used for hole rendering. For world rect calculation FadeImage should be attached to canvas with camera.
        /// If you're using canvas Overlay render mode you still should serialize camera via debug inspector.
        /// </summary>
        public TutorialFadeImage FadeImage { get; }
        
        /// <summary>
        /// Rect in world
        /// </summary>
        Rect WorldRect { get; set; }

        /// <summary>
        /// Returns current hole rect in world.
        /// </summary>
        /// <returns>Will return Rect.zero if Renderer or FadeImage or FadeImage.canvas or FadeImage.canvas.worldCamera are null.</returns>
        public override Rect GetWorldRect() => WorldRect;

        /// <summary>
        /// Updates WorldRect by provided Renderer and FadeImage.
        /// </summary>
        public override void UpdateRect()
        {
            Rect rect = CalculateRendererRect();

            if (WorldRect == rect)
                return;

            WorldRect = rect;
            InvokeRectChanged();
        }

        /// <summary>
        /// Calculates world rect.
        /// </summary>
        /// <returns>Rect in world. Will return Rect.zero if Renderer or FadeImage or FadeImage.canvas or FadeImage.canvas.worldCamera are null.</returns>
        Rect CalculateRendererRect()
        {
            if (FadeImage == null || FadeImage.canvas == null || FadeImage.canvas.worldCamera == null)
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