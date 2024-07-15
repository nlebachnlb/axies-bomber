using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static int[] Distribute(int value, int slots)
    {
        int[] result = new int[slots];
        for (int i = 0; i < slots; ++i)
        {
            bool isLastSlot = i == slots - 1;
            int valueToAssign = isLastSlot ? value : Random.Range(0, value);
            result[i] = valueToAssign;
            value -= valueToAssign;
        }
        return result;
    }

    public static Color WithAlpha(this Color color, float alpha)
    {
        color.a = alpha;
        return color;
    }
}
