using UnityEngine;

public static class Int32Extention
{
    public static int GetDigitNumber(this int digit, int number)
    {
        return (int)((digit % Mathf.Pow(10, number)) / Mathf.Pow(10, number - 1));
    }
}
