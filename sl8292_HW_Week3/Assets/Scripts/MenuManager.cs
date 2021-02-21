using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // SavedLevel set up
    const string DIR_LOGS = "/Logs";
    const string FILE_CURRENTLEVEL = DIR_LOGS + "/currentLevel.txt";
    string FILE_PATH_CURRENT_LEVEL;

    private void Start()
    {
        FILE_PATH_CURRENT_LEVEL = Application.dataPath + FILE_CURRENTLEVEL;
    }
    
    // clear the old data in file and start from level 1
    public void StartNewGame()
    {
        // if no file found, create a new one, set its value to one
        if (!File.Exists(FILE_PATH_CURRENT_LEVEL))
        {
            Directory.CreateDirectory(Application.dataPath + DIR_LOGS);
            GameManager.instance.CurrentLevel = 1;
        }
        else
        {
            // clear the data in file, set it back to 1
            File.WriteAllText(FILE_PATH_CURRENT_LEVEL, 1 + "");
        }
        GameManager.instance.targetScore = GameManager.instance.CurrentLevel + 2;
        SceneManager.LoadScene(GameManager.instance.CurrentLevel);
    }
    
    // read file and load to the saved level
    public void ContinueFromSaved()
    {
        if (!File.Exists(FILE_PATH_CURRENT_LEVEL))
        {
            Directory.CreateDirectory(Application.dataPath + DIR_LOGS);
            GameManager.instance.CurrentLevel = 1;
        }
        else
        {
            string fileContents = File.ReadAllText(FILE_PATH_CURRENT_LEVEL);
            GameManager.instance.CurrentLevel = Int32.Parse(fileContents);
        }
        GameManager.instance.targetScore = GameManager.instance.CurrentLevel + 2;
        SceneManager.LoadScene(GameManager.instance.CurrentLevel);
    }
}
