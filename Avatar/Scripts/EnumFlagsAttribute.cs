using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnumFlagsAttribute : PropertyAttribute
{
    public EnumFlagsAttribute() { }

    /// <summary>
    /// Returns a list with the indecies of all toggled elements in given enum
    /// </summary>
    /// <param name="en"></param>
    /// <returns></returns>
    public static List<int> ReturnSelectedElements(Enum en)
    {
        object obj = en;
        List<int> selectedElements = new List<int>();
        for (int i = 0; i < System.Enum.GetValues(en.GetType()).Length; i++)
        {
            int layer = 1 << i;
            if (((int)obj & layer) != 0)
            {
                selectedElements.Add(i);
            }
        }
        return selectedElements;
    }


}
