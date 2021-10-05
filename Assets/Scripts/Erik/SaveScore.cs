using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public static class SaveScore
{
    public static void SaveGame(int newScore, string newName) // Don't want to override the save file evertime we save, we want to add to it, specifically the score we just got
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + "/SaveData.Highscores"))
        {
            FileStream openedFile = File.Open(Application.persistentDataPath + "/SaveData.Highscores", FileMode.Open);
            Save save = (Save)bf.Deserialize(openedFile);
            openedFile.Close();

            for (int i = 0; i < 9; i++)
            {
                for (int g = 0; g < save.scores.Count; g++)
                {
                    var savedScore = save.scores[g];
                    if (newScore > savedScore)
                    {
                        int scoreWeAreRemovingLol = save.scores[g];
                        string nameWeAreRemovingLol = save.names[g];

                        save.scores[g] = newScore;
                        save.names[g] = newName;

                        newScore = scoreWeAreRemovingLol;
                        newName = nameWeAreRemovingLol;
                        break; // We want to break out of the loop if we replaced an item for two reasons, 1. We're done 2. We just modified the collection and need to quickly flee the scene 
                    }
                }
            }
            
            // We make a new file and put the saved values from the last in there
            FileStream createdFile = File.Create(Application.persistentDataPath + "/SaveData.Highscores");

            bf.Serialize(createdFile, save);
            createdFile.Close();
            Debug.Log("Game is saved");
        }
        else // The only time there wont exist a file is the first time, So we want to create a file with 8 empty spots plus the new name and score
        {
            Debug.Log("There was no File");

            Save save = new Save(new List<string>(), new List<int>());
            FileStream createdFile = File.Create(Application.persistentDataPath + "/SaveData.Highscores");

            save.scores.Add(newScore);
            save.names.Add(newName);

            for (int i = 0; i < 8; i++)
            {
                save.scores.Add(0);
                save.names.Add("");
            }
            
            bf.Serialize(createdFile, save);
            createdFile.Close();

            Debug.Log("Game is saved");
        }
    }

    public static Save LoadGame()
    {
        BinaryFormatter bf = new BinaryFormatter();

        if (File.Exists(Application.persistentDataPath + "/SaveData.Highscores"))
        {
            //BinaryFormatter bf = new BinaryFormatter();

            FileStream file = File.Open(Application.persistentDataPath + "/SaveData.Highscores", FileMode.Open);
            Save saved = (Save)bf.Deserialize(file);
            file.Close();

            Debug.Log("Game data loaded!");

            return saved;
        }
        else
        {
            Debug.Log("There was no File");

            Save save = new Save(new List<string>(), new List<int>());

            FileStream createdFile = File.Create(Application.persistentDataPath + "/SaveData.Highscores");
            for (int i = 0; i < 9; i++)
            {
                save.scores.Add(0);
                save.names.Add("");
            }

            bf.Serialize(createdFile, save);
            createdFile.Close();

            Debug.Log("Game is saved. We saved this many scores, names: " + save.scores.Count + ", " + save.names.Count);

            return save;
        }
    }
}

[System.Serializable]
public class Save
{
    public List<string> names;
    public List<int> scores;

    public Save(List<string> names, List<int> scores)
    {
        this.names = names;
        this.scores = scores;
    }
}
