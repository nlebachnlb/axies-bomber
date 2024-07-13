using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Utils
{
    public class RandomUtils
    {
        public static T RandomElement<T>(List<T> elements)
        {
            int index = Random.Range(0, elements.Count);
            return elements[index];
        }
    }
}
