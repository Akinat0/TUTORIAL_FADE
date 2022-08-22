namespace Abu
{
    using System;
    using UnityEngine;

    /// <summary>
    /// The base class for all tutorial holes.
    /// </summary>
    public abstract class TutorialHole
    {
        /// <summary>
        /// Creates new tutorial hole.
        /// </summary>
        /// <param name="isAutoUpdate">Should world rect be auto updated every frame, or developer will manually update it to achieve better performance.</param>
        protected TutorialHole(bool isAutoUpdate = true)
        {
            IsAutoUpdateEnabled = isAutoUpdate;
        }

        /// <summary>
        /// Should world rect be auto updated every frame, or developer will manually update it to achieve better performance.
        /// </summary>
        public bool IsAutoUpdateEnabled { get; set; }

        /// <summary>
        /// Updates WorldRect.
        /// </summary>
        public abstract void UpdateRect();
        
        /// <summary>
        /// Called when WorldRect changed.
        /// </summary>
        public event Action RectChanged;

        /// <summary>
        /// Returns current hole rect in world.
        /// </summary>
        /// <returns>Could return Rect.zero if some parameters was not provided</returns>
        public abstract Rect GetWorldRect();

        /// <summary>
        /// Invokes RectChanged event.
        /// </summary>
        protected void InvokeRectChanged()
        {
            RectChanged?.Invoke();
        }
    }
}