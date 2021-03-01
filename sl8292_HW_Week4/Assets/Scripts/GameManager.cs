using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Net;

public class GameManager : MonoBehaviour
{
    const string FILE_HIGH_SCORES = DIR_LOGS + "/highScores.txt";
    const string DIR_LOGS = "/Logs";
    string FILE_PATH_HIGH_SCORES;
    
    public static GameManager instance;
    
    public Text timerText;
    
    public float gameTime = 10;
    
    private float timer;

    public int score;

    private bool isGame = true;
    private bool isOver = false;
    
    string fileContents = "";

    public int Score
    {
        get { return score;}
        set
        {
            score = value;
        }
    }
    
    private List<int> highScores;
    
    void Awake()
    {
        if (instance == null) 
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        FILE_PATH_HIGH_SCORES = Application.dataPath + FILE_HIGH_SCORES;
    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;
        
        if (!isGame && isOver) // display highscores when not in game
        {
            string highScoreString = "High Scores \n";

            for (var i = 0; i < highScores.Count; i++)
            {
                //show in UI text
                highScoreString += highScores[i] + "\n";
            }

            timerText.text = highScoreString;
        }
        else // display time when in game
        {
            timerText.text = "Time:" + (int)(gameTime - timer);
        }

        if (gameTime < timer && isGame)
        {
            SceneManager.LoadScene(7);
            isOver = true;
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
            if (score > highScores[i])
            {
                highScores.Insert(i,score);
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
