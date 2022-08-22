namespace Abu
{
    using UnityEngine;

    /// <summary>
    /// Tutorial hole that corresponds with rect transform provided by user.
    /// </summary>
    public class RectTransformTutorialHole : TutorialHole
    {
        /// <summary>
        /// Creates RectTransformTutorialHole
        /// </summary>
        /// <param name="rectTransform">RectTransform which will be highlighted</param>
        /// <param name="isAutoUpdate">Should world rect be auto updated every frame, or developer will manually update it to achieve better performance.</param>
        public RectTransformTutorialHole(RectTransform rectTransform, bool isAutoUpdate = true) : base(isAutoUpdate)
        {
            RectTransform = rectTransform;
            WorldRect = RectTransform.TransformRect(RectTransform.rect);
        }
        
        /// <summary>
        /// RectTransform which will be highlighted.
        /// </summary>
        public RectTransform RectTransform { get; }
        
        /// <summary>
        /// Rect in world
        /// </summary>
        Rect WorldRect { get; set; }

        /// <summary>
        /// Returns current hole rect in world.
        /// </summary>
        /// <returns>Will return Rect.zero if RectTransform is null</returns>
        public override Rect GetWorldRect() => WorldRect;

        /// <summary>
        /// Updates WorldRect by provided RectTransform.
        /// </summary>
        public override void UpdateRect()
        {
            Rect rect = RectTransform.TransformRect(RectTransform.rect);

            if (WorldRect == rect)
                return;

            WorldRect = rect;
            InvokeRectChanged();
        }
    }
}