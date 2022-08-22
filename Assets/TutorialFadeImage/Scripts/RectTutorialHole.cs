namespace Abu
{
    using UnityEngine;

    /// <summary>
    /// Tutorial hole that receives world rect, provided by user.
    /// </summary>
    public class RectTutorialHole : TutorialHole
    {
        /// <summary>
        /// Creates RectTutorialHole
        /// </summary>
        /// <param name="worldRect">Rect in world to create hole.</param>
        /// <param name="isAutoUpdate">Should world rect be auto updated every frame, or developer will manually update it to achieve better performance.</param>
        public RectTutorialHole(Rect worldRect, bool isAutoUpdate = true) : base(isAutoUpdate)
        {
            WorldRect = worldRect;
        }

        /// <summary>
        /// Rect in world
        /// </summary>
        public Rect WorldRect { get; private set; }

        /// <summary>
        /// Sets rect in world.
        /// </summary>
        /// <param name="worldRect">Rect in world coordinates.</param>
        public void SetWorldRect(Rect worldRect)
        {
            if (worldRect == WorldRect)
                return;

            WorldRect = worldRect;
            InvokeRectChanged();
        }

        /// <summary>
        /// Does nothing. rect should be provided by user manually.
        /// </summary>
        public override void UpdateRect()
        {
        }

        /// <summary>
        /// Returns current hole rect in world.
        /// </summary>
        /// <returns>Could return Rect.zero if some parameters was not provided</returns>
        public override Rect GetWorldRect() => WorldRect;

    }
}