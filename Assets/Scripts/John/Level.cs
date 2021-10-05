using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Level
{
    public static int ExpToNextLevel
    {
        get
        {
            return Mathf.FloorToInt(Mathf.Pow((GetLevelInfo().Level <= 0 ? 1 : GetLevelInfo().Level), 2f) * 5);
        }
    }

    public static void AddExperience(int amount)
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (!File.Exists(Application.persistentDataPath + "/SaveData.LevelInfo"))
        {
            Initialize();
        }

        if (File.Exists(Application.persistentDataPath + "/SaveData.LevelInfo"))
        {
            FileStream openedFile = File.Open(Application.persistentDataPath + "/SaveData.LevelInfo", FileMode.Open);
            LevelInfo levelInfo = (LevelInfo)bf.Deserialize(openedFile);
            openedFile.Close();

            levelInfo.Experience += amount;

            while (levelInfo.Experience >= ExpToNextLevel)
            {
                levelInfo.Experience -= ExpToNextLevel;
                levelInfo.Level++;
            }

            FileStream createdFile = File.Create(Application.persistentDataPath + "/SaveData.LevelInfo");
            bf.Serialize(createdFile, levelInfo);
            createdFile.Close();
            Debug.Log("Level Saved");
        }
        else
        {
            Debug.LogError("Yo, your initialization is dogshit");
        }
    }

    public static LevelInfo GetLevelInfo()
    {
        if (!File.Exists(Application.persistentDataPath + "/SaveData.LevelInfo"))
        {
            Initialize();
        }

        if (File.Exists(Application.persistentDataPath + "/SaveData.LevelInfo"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream openedFile = File.Open(Application.persistentDataPath + "/SaveData.LevelInfo", FileMode.Open);
            LevelInfo levelInfo = (LevelInfo)bf.Deserialize(openedFile);
            openedFile.Close();

            return levelInfo;
        }
        else
        {
            Debug.LogError("Yo, your initialization is dogshit");
            return new LevelInfo(0, 0);
        }

    }

    private static void Initialize()
    {
        BinaryFormatter bf = new BinaryFormatter();

        LevelInfo levelInfo = new LevelInfo(0, 0);

        FileStream createdFile = File.Create(Application.persistentDataPath + "/SaveData.LevelInfo");
        bf.Serialize(createdFile, levelInfo);
        createdFile.Close();
        Debug.Log("Level Initialized");
    }
}

[System.Serializable]
public struct LevelInfo
{
    public int Level;
    public int Experience;

    public LevelInfo(int level, int exp)
    {
        Level = level;
        Experience = exp;
    }
}
