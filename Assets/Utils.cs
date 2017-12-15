using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    static int maxHeight = 150;
    static float smooth = 0.01f;
    static int octaves = 3;
    static float persistence = 0.3f;

    public static int GenerateHeight(float x, float z)
    {
        float height = Mathf.Lerp(0, maxHeight, fBM(x * smooth, z * smooth, octaves, persistence));
        return (int)height;
    }

    static float fBM(float x, float z, int oct, float pers)
    {
        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0;

        for (int i = 0; i < oct; i++)
        {
            total += Mathf.PerlinNoise(x * frequency, z * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= pers;
            frequency *= 2;
        }

        return total / maxValue;
    }
}
