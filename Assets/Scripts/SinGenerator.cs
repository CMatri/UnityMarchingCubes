﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinGenerator : ITerrainGenerator
{
    public float genVal(float x, float y, float z)
    {
        //return (x * x) + (y * y) + z * z - 10;
        return Mathf.Sin(x*y + x*z + y*z) + Mathf.Sin(x*y) + Mathf.Sin(y*z) + Mathf.Sin(x*z) - 1;
        //return 0.5f;
    }
}
