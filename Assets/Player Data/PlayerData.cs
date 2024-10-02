using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // resources
    public int ironCount;
    public int coalCount;

    // gametime
    public float totalGameTime;
    public float dayTime;
    public int dayCount;

    // builds
    public List<Vector3IntSerializable> buildPositions = new List<Vector3IntSerializable>();
    public List<string> buildingData = new List<string>();

    public float[] vector3int = new float[3];
    public string[] building = new string[2]; // turret name, lvl ["Miner", "2"]

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
            buildingData.Add(entry.Value.GetComponent<Build>().data.name);
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
