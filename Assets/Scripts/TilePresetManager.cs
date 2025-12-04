using UnityEngine;
using System.Collections.Generic;

public class TilePresetManager : MonoBehaviour
{
    private readonly List<Vector2> PresetLocationsX = new() { new(25, 33), new(35, 38), new(40, 49), new(51, 54), new(56, 65) };
    private readonly List<Vector2> PresetLocationsY = new() { new(0, -9), new(0, -9), new(0, -9), new(0, -3), new(0, -3) };

    public Vector2 GetPresetX(int i)
    {
        return PresetLocationsX[i];
    }

    public int GetPresetXStart(int i)
    {
        return (int)GetPresetX(i).x;
    }

    public int GetPresetXEnd(int i)
    {
        return (int)GetPresetX(i).y;
    }

    public Vector2 GetPresetY(int i)
    {
        return PresetLocationsY[i];
    }

    public int GetPresetYStart(int i)
    {
        return (int)GetPresetY(i).x;
    }

    public int GetPresetYEnd(int i)
    {
        return (int)GetPresetY(i).y;
    }
}