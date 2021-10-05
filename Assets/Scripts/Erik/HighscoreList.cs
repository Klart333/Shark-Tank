using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System;

public class HighscoreList : MonoBehaviour
{
    public TextMeshProUGUI HighName;
    public TextMeshProUGUI HighScore;

    [HideInInspector]
    public List<string> names = new List<string>();
    [HideInInspector]
    public List<int> scores = new List<int>();

    private void Start()
    {
        Reset();
        Load();
        ShowScores();
    }

    private void Load()
    {
        Save save = SaveScore.LoadGame();

        for (int i = 0; i < save.names.Count; i++)
        {
            names.Add(save.names[i]);
        }

        for (int i = 0; i < save.scores.Count; i++)
        {
            scores.Add(save.scores[i]);
        }
    }

    public void Addperson(string name, int score)
    {
        names.Add(name);
        scores.Add(score);
    }
    
    public void Reset() // Removes all scores and updates the board to show nothing
    {
        names.RemoveRange(0, names.Count);
        scores.RemoveRange(0, scores.Count);
        HighName.text = "";
        HighScore.text = "";
    }

    public void ShowScores()
    {
        SortLists(); // Sorts the lists before dislaying them

        for (int i = 1; i <= names.Count; i++) // Needs to start at one and end equal to the count because of zero indexing
        {
            if (string.IsNullOrEmpty(names[names.Count - i]))
            {
                HighName.text += "N/A\n";
                continue;
            }

            HighName.text += names[names.Count - i] + "\n"; // Goes through the list backwards because of how the list is sorted
        }

        for (int i = 1; i <= scores.Count; i++) // Same here
        {
            HighScore.text += scores[scores.Count - i] + "\n"; // Goes through the list backwards because of how the list is sorted
        }
    }

    public void SortLists()
    {
        List<int> unSortedScores = new List<int>();
        foreach (int score in scores)
        {
            unSortedScores.Add(score); // Can't assign the unSortedScores to scores becuase then when we sort scores the unSortedScores list is also sorted, for some reason
        }

        string[] sortedNames = new string[names.Count]; // An Array that can hold as many values as the names list has 
        scores.Sort();

        for (int i = 0; i < names.Count; i++)
        {
            for (int g = 0; g < unSortedScores.Count; g++)
            {
                if (names.IndexOf(names[i]) == unSortedScores.IndexOf(unSortedScores[g])) // If they previously were paired
                {
                    int listIndex = scores.IndexOf(unSortedScores[g]);
                    if (listIndex < 0)
                    {
                        continue;
                    }

                    sortedNames[listIndex] = names[i]; // Then we find the new index of the old score and assign it to the sorted string list
                    names[i] = ""; // Remove the name so that the IndexOf function doesn't return it
                    unSortedScores[g] = -1; // We remove the score so that the same score cannot get multiple names, we cannot delete it becuase that would fuck up the index
                    break; // We don't have to check the rest, also gotta skeedadle from errors
                }
            }
        }

        names = sortedNames.ToList();
    }
}