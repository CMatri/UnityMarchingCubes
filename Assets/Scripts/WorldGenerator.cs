using UnityEngine;

public class WorldGenerator : ITerrainGenerator
{
    int worldSize;
    public WorldGenerator(int worldSize)
    {
        this.worldSize = worldSize;
    }

    public float genVal(float x, float y, float z)
    {
        return Mathf.Sin(x + z);
    }
}
