using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //static variable means the value is the same for all the objects of this class type and the class itself
    public static GameManager instance; //this static var will hold the Singleton

    private int score = 0;

    // chase the file in Logs
    const string FILE_CURRENTLEVEL = DIR_LOGS + "/currentLevel.txt";
    const string FILE_SCORES = DIR_LOGS + "/highScore.txt";
    const string FILE_ALL_SCORES = DIR_LOGS + "/allScores.csv";
    const string DIR_LOGS = "/Logs";

    // string to store the file path
    string FILE_PATH_CURRENT_LEVEL;
    string FILE_PATH_HIGH_SCORES;
    string FILE_PATH_ALL_SCORES;

    public int CurrentLevel
    {
        get { return currentLevel; }
        set {
                currentLevel = value;
                if (!File.Exists(FILE_PATH_CURRENT_LEVEL))
                {
                    Directory.CreateDirectory(Application.dataPath + DIR_LOGS);
                    //File.Create(FILE_PATH_HIGH_SCORES);
                }

                File.WriteAllText(FILE_PATH_CURRENT_LEVEL, currentLevel + "");
            }
    }

    public int Score
    {
        get { return score; }
        set
        {
            score = value;

            //Debug.Log("Someone set the Score!");
            if (score > HighScore)
            {
                HighScore = score;
            }

            string fileContents = "";
            if (File.Exists(FILE_PATH_ALL_SCORES))
            {
                fileContents = File.ReadAllText(FILE_PATH_ALL_SCORES);
            }

            fileContents += score + ",";
            File.WriteAllText(FILE_PATH_ALL_SCORES, fileContents);

        }
    }

    const string PREF_KEY_HIGH_SCORE = "HighScoreKey";
    int highScore = -1;

    public int HighScore
    {
        get
        {
            if (highScore < 0)
            {
                //highScore = PlayerPrefs.GetInt(PREF_KEY_HIGH_SCORE, 3);
                if (File.Exists(FILE_PATH_HIGH_SCORES))
                {
                    string fileContents = File.ReadAllText(FILE_PATH_HIGH_SCORES);
                    highScore = Int32.Parse(fileContents);
                }
                else
                {
                    highScore = 3;
                }
            }

            return highScore;
        }
        set
        {
            highScore = value;
            Debug.Log("New High Score!!!");
            Debug.Log("File Path: " + FILE_PATH_HIGH_SCORES);
            //PlayerPrefs.SetInt(PREF_KEY_HIGH_SCORE, highScore);

            if (!File.Exists(FILE_PATH_HIGH_SCORES))
            {
                Directory.CreateDirectory(Application.dataPath + DIR_LOGS);
                //File.Create(FILE_PATH_HIGH_SCORES);
            }

            File.WriteAllText(FILE_PATH_HIGH_SCORES, highScore + "");
        }
    }

    public int targetScore = 0;
    int currentLevel = 1;

    public Text text;  //TextMesh Component to tell you the time and the score
    
    void Awake()
    {
        if (instance == null) //instance hasn't been set yet
        {
            DontDestroyOnLoad(gameObject);  //Dont Destroy this object when you load a new scene
            instance = this;  //set instance to this object
        }
        else  //if the instance is already set to an object
        {
            Destroy(gameObject); //destroy this new object, so there is only ever one
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FILE_PATH_HIGH_SCORES = Application.dataPath + FILE_SCORES;
        FILE_PATH_ALL_SCORES  = Application.dataPath + FILE_ALL_SCORES;
        FILE_PATH_CURRENT_LEVEL = Application.dataPath + FILE_CURRENTLEVEL;
    }

    // Update is called once per frame
    void Update()
    {

        //update the text with the score and level
        text.text = "Level:" + CurrentLevel + 
                    "\nScore: " + score + " Target: " + targetScore +
                    "\nHigh Score: " + HighScore;
        
        if (score == targetScore)  //if the current score == the targetScore
        {
            CurrentLevel++; //increase the level number
            SceneManager.LoadScene(CurrentLevel); //go to the next level
            targetScore += 1; //update target score
        }
    }
}
