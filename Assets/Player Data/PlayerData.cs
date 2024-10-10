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
    public int towerHP;

    public List<Vector3IntSerializable> buildPositions = new List<Vector3IntSerializable>();
    public List<string> buildingData = new List<string>();

    public List<Vector3IntSerializable> tilePosition = new List<Vector3IntSerializable>();
    public List<int> resourceAmount = new List<int>();

    public PlayerData(PlayerBuild build, PlayerEconomy economy, PlayerGameTime gameTime)
    {
        ironCount = economy.ironCount;
        coalCount = economy.coalCount;

        totalGameTime = gameTime.gameTime;
        dayTime = gameTime.dayTime;
        dayCount = gameTime.dayCount;

        towerHP = build.currentTowerHealth;

        foreach (var entry in build.buildPlacement)
        {
            buildPositions.Add(new Vector3IntSerializable(entry.Key));
            buildingData.Add((entry.Value.name));
        }
        foreach (var entry in build.tileResource) {
            tilePosition.Add(new Vector3IntSerializable(entry.Key));
            resourceAmount.Add((entry.Value));
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
