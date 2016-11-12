using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public static class HelperMethods
{
    public static void Shuffle<T>(this IList<T> aList)
    {
        for (int i = 0; i < aList.Count; i++)
        {
            int k = Random.Range(0, aList.Count);
            T value = aList[k];
            aList[k] = aList[i];
            aList[i] = value;
        }
    }

}
