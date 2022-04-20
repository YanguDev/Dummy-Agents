using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomColor
{
    public static Color GetColor(){
        float r = Random.Range(0f, 1f);
        float g = Random.Range(0f, 1f);
        float b = Random.Range(0f, 1f);
        
        return new Color(r, g, b);
    }
}
