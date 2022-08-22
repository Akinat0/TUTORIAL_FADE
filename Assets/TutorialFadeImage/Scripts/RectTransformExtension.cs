namespace Abu
{
    using UnityEngine;

    /// <summary>
    /// Extension class for RectTransform.
    /// </summary>
    public static class RectTransformExtension
    {
        /// <summary>
        ///   Transforms Rect from local space to world space.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="rect">Rect in local coordiantes.</param>>
        /// <returns>Rect in world coordinates</returns>
        public static Rect TransformRect(this Transform transform, Rect rect)
        {
            Vector3 lossyScale = transform.lossyScale;
            Vector3 position = transform.position;

            return new Rect(
                rect.x * lossyScale.x + position.x,
                rect.y * lossyScale.y + position.y,
                rect.width * lossyScale.x,
                rect.height * lossyScale.y
            );
        }
    }
}