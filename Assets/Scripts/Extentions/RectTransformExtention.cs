namespace UnityEngine
{
    public static class RectTransformExtention
    {
        public static Rect GetWorldRect(this RectTransform t)
        {
            Vector3[] corners = new Vector3[4];
            t.GetWorldCorners(corners);

            float xMin = corners[0].x;
            float xMax = corners[2].x;
            float yMin = corners[0].y;
            float yMax = corners[1].y;

            return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        public static bool TryGetOffsetToEnterOtherTransform(this RectTransform r, 
            RectTransform otherTransform, out Vector3 offset)
        {
            var rect = r.GetWorldRect();
            var otherRect = otherTransform.GetWorldRect();

            offset = Vector3.zero;
            if (rect.CanBeInsideOtherRect(otherRect))
            {
                if (rect.xMin < otherRect.xMin)
                {
                    offset.x = otherRect.xMin - rect.xMin;
                }
                else if (rect.xMax > otherRect.xMax)
                {
                    offset.x = otherRect.xMax - rect.xMax;
                }

                if (rect.yMin < otherRect.yMin)
                {
                    offset.y = otherRect.yMin - rect.yMin;
                }
                else if (rect.yMax > otherRect.yMax)
                {
                    offset.y = otherRect.yMax - rect.yMax;
                }
                return true;
            }
            return false;
        }
    }
}
