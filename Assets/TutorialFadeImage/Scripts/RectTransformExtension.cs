namespace Abu
{
    using UnityEngine;

    public static class RectTransformExtension
    {
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