namespace UnityEngine
{
    public static class ColorExtention
    {
        public static string ToRGBA(this Color color)
        {
            var result = "#";/*
            result += $"{(color.r * 255):X2}";
            result += $"{(color.g * 255):X2}";
            result += $"{(color.b * 255):X2}";*/
            int number= (int)(color.r * 255);
            result += number.ToString("x2");

            number = (int)(color.g * 255);
            result += number.ToString("x2");

            number = (int)(color.b * 255);
            result += number.ToString("x2");

            number = (int)(color.a * 255);
            result += number.ToString("x2");

            return result;
        }

        public static string ToRGB(this Color color)
        {
            var result = "#";/*
            result += $"{(color.r * 255):X2}";
            result += $"{(color.g * 255):X2}";
            result += $"{(color.b * 255):X2}";*/
            int number = (int)(color.r * 255);
            result += number.ToString("x2");

            number = (int)(color.g * 255);
            result += number.ToString("x2");

            number = (int)(color.b * 255);
            result += number.ToString("x2");

            return result;
        }
    }
}
