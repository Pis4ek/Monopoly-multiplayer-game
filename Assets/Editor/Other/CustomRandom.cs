using System.Collections;
using UnityEngine;

public static class CustomRandom
{
    public static bool TryWithChance(float chance)
    {
        if(Random.value * 100 < chance)
        {
            return true;
        }
        return false;
    }

    public static T GetRandomElement<T>(T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }


}