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

    public bool isGame = false;
    public bool isOver = false;
    public float gameTime = 20;
    public float timer = 0;
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

    private List<int> highScores;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;

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

    public int targetScore = 0;
    int currentLevel = 0;

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
        
        if (highScores == null)
        {
            highScores = new List<int>();
            
            string fileContents = File.ReadAllText(FILE_PATH_HIGH_SCORES);

            string[] fileScores = fileContents.Split(',');

            for (var i = 0; i < fileScores.Length - 1; i++)
            {
                highScores.Add(Int32.Parse(fileScores[i]));
            }
        }
    }

    // Update is called once per frame
    void Update()
    { 
        // press start or continue, enter the game
        if (isGame && !isOver)
        {
            timer += Time.deltaTime;
            //update the text with the score and level
            text.text = "Time:" + (int) (gameTime - timer) +
                        "Level:" + CurrentLevel +
                        "\nScore: " + score + " Target: " + targetScore +
                        "\nHigh Score: " + highScores[0];
        
            if (score == targetScore)  //if the current score == the targetScore
            {
                CurrentLevel++; //increase the level number
                SceneManager.LoadScene(CurrentLevel); //go to the next level
                targetScore += 2; //update target score
                if (CurrentLevel >= 7)
                {
                    isOver = true;
                    isGame = false;
                }
            }
        }
        
        // enter over scene
        if(!isGame && isOver)
        {
            Debug.Log("show highscores");
            string highScoreString = "High Scores \n";

            for (var i = 0; i < highScores.Count; i++)
            {
                //show in UI text
                highScoreString += highScores[i] + "\n";
            }

            text.text = highScoreString;
        }
        
        // time is up
        if (gameTime < timer && isGame)
        {
            Debug.Log("update!");
            isOver = true;
            SceneManager.LoadScene(7);
            isGame = false;
            UpdateHighScore();
        }
    }
    
    void UpdateHighScore()
    {
        if (highScores == null)
        {
            highScores = new List<int>();

            string fileContents = File.ReadAllText(FILE_PATH_HIGH_SCORES);

            string[] fileScores = fileContents.Split(',');

            for (var i = 0; i < fileScores.Length - 1; i++)
            {
                highScores.Add(Int32.Parse(fileScores[i]));
            }
        }

        for (var i = 0; i < highScores.Count; i++)
        {
            if (Score > highScores[i])
            {
                highScores.Insert(i,Score);
                break;
            }
        }

        string saveHighScoreString = "";
        for (var i = 0; i < highScores.Count; i++)
        {
            saveHighScoreString += highScores[i] + ",";
        }
        if (!File.Exists(FILE_PATH_HIGH_SCORES))
        {
            Directory.CreateDirectory(Application.dataPath + DIR_LOGS);
        }
        File.WriteAllText(FILE_PATH_HIGH_SCORES, saveHighScoreString);
    }
}
