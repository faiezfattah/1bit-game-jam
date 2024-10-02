using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // resources
    public int ironCount = 0;
    public int coalCount = 0;

    // gametime
    public float totalGameTime = 0;
    public float dayTime = 0;
    public int dayCount = 0;

    // builds
    public List<Vector3IntSerializable> buildPositions = new List<Vector3IntSerializable>();
    public List<string> buildingData = new List<string>();

    public PlayerData(PlayerBuild build, PlayerEconomy economy, PlayerGameTime gameTime)
    {
        ironCount = economy.ironCount;
        coalCount = economy.coalCount;

        totalGameTime = gameTime.gameTime;
        dayTime = gameTime.dayTime;
        dayCount = gameTime.dayCount;

        foreach (var entry in build.buildPlacement)
        {
            buildPositions.Add(new Vector3IntSerializable(entry.Key));
            buildingData.Add((entry.Value.name));
        }
    }
}
[System.Serializable]
public class Vector3IntSerializable
{
    public int x, y, z;

    public Vector3IntSerializable(Vector3Int vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3Int ToVector3Int()
    {
        return new Vector3Int(x, y, z);
    }
}
