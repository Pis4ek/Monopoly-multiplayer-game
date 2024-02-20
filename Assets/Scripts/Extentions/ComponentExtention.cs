namespace UnityEngine
{
    public static class ComponentExtention
    {
        public static void Activate(this Component c)
        {
            c.gameObject.SetActive(true);
        }

        public static void Disactivate(this Component c)
        {
            c.gameObject.SetActive(false);
        }

        public static bool IsActive(this Component c)
        {
            return c.gameObject.activeInHierarchy;
        }
    }
}
