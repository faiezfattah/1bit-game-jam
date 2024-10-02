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
        build.buildPlacement.Clear();
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

            gameTime.dayCount = data.dayCount;
            gameTime.dayTime = data.dayTime;
            gameTime.gameTime = data.totalGameTime;

            economy.ironCount = data.ironCount;
            economy.coalCount = data.coalCount;

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
            BuildData buildData = Resources.Load<BuildData>($"Assets/Turrets/Resources/{buildingName}");

            if (buildData != null) {
                build.buildPlacement.Add(position, buildData);
            }

            else {
                Debug.LogWarning($"BuildData not found for {buildingName}");
            }
        }
    }
}
