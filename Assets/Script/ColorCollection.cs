using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ColorCollection", menuName = "ScriptableObjects/ColorCollection", order = 1)]
[System.Serializable]
public class ColorCollection : ScriptableObject
{
    public Color[] colors;

    public static ColorCollection Instance;

    public ColorCollection() 
    {
        Instance = this;
    }
}
