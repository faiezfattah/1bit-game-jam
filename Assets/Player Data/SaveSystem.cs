using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerBuild build, PlayerEconomy economy, PlayerGameTime gameTime)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.ffw";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(build, economy, gameTime);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static void ResetPlayer(PlayerBuild build, PlayerEconomy economy, PlayerGameTime gameTime) {
        PlayerData data = new PlayerData(build, economy, gameTime);

        gameTime.ResetGameTime();
        economy.ResetEconomy();
        build.ResetBuild();
        build.ResetTileResource();
    }
    public static PlayerData LoadPlayer(PlayerBuild build, PlayerEconomy economy, PlayerGameTime gameTime)
    {
        string path = Application.persistentDataPath + "/player.ffw";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            RebuildBuildDictionary(data, build);
            RebuildTileResourceDictionary(data, build);

            gameTime.dayCount = data.dayCount;
            gameTime.dayTime = data.dayTime;
            gameTime.gameTime = data.totalGameTime;

            economy.ironCount = data.ironCount;
            economy.coalCount = data.coalCount;

            build.currentTowerHealth = data.towerHP;

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    private static void RebuildBuildDictionary(PlayerData data, PlayerBuild build) {
        build.buildPlacement.Clear();
        for (int i = 0; i < data.buildPositions.Count; i++) {
            Vector3Int position = data.buildPositions[i].ToVector3Int();
            string buildingName = data.buildingData[i];
            BuildData buildData = Resources.Load<BuildData>($"{buildingName}");

            if (buildData != null) {
                build.buildPlacement.Add(position, buildData);
                Debug.Log("buildPlacement Updated " + position + " " + buildData);
            }

            else {
                Debug.LogWarning($"BuildData not found for {buildingName}");
            }
        }
    }
    private static void RebuildTileResourceDictionary(PlayerData data, PlayerBuild build) {
        for (int i = 0; i < data.tilePosition.Count; i++) {

            Vector3Int position = data.tilePosition[i].ToVector3Int();
            int resourceAmount = data.resourceAmount[i];

            build.tileResource.Add(position, resourceAmount);
        }
    }
}
