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
    public static PlayerData LoadPlayer(PlayerBuild build, PlayerEconomy economy, PlayerGameTime gameTime)
    {
        string path = Application.persistentDataPath + "/player.ffw";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();


            // Rebuild the player build placements
            for (int i = 0; i < data.buildPositions.Count; i++)
            {
                Vector3Int position = data.buildPositions[i].ToVector3Int();
                string buildingName = data.buildingData[i];

                // Load the ScriptableObject by name (from Resources, AssetDatabase, etc.)
                BuildData buildData = Resources.Load<BuildData>("Buildings/" + buildingName); // TODO: RESOURCE FOLDER Assuming the SO is in Resources folder
                if (buildingName != null)
                {
                    // Instantiate the building using the prefab in the ScriptableObject
                    GameObject building = Object.Instantiate(buildData.prefabs, position, Quaternion.identity);
                    building.GetComponent<Build>().data = buildData;
                    build.buildPlacement.Add(position, building);
                }

            }

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
}
