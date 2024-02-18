using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //make it a singleton:
    public static GameManager instance;

    //set variable for text display:
    public TextMeshProUGUI display;
    
    //declare score:
    public int score;
    
    //create file path script variables that we can't modify:
    //the folder where the text file will be:
    const string FILE_DIR = "/DATA/";
    //the file for saving the highscores:
    private const string DATA_FILE = "highScores.txt";
    
    string FILE_FULL_PATH; 
    //this one is not a constant because it will depend on the operating system
    
    //property for Score:
    public int Score
    {
        //get pulls the value from lowercase score when this property is called
        get
        {
            return score;
        }
        set
        {
            score = value;
            
            //check if the current score is a high score, and decide where to put it in the high score list:
            if (isHighScore(score))
            {
                int highScoreSlot = -1;
                //-1 so it includes slot 0 in the for loop
                
                //iterate through each slot in the list and check if the score is higher
                //if it is higher, put it in that slot
                for (int i = 0; i < HighScores.Count; i++)
                {
                    if (score > highScores[i])
                    {
                        highScoreSlot = i;
                        break;
                    }
                }
                
                //put this value in the high scores list
                highScores.Insert(highScoreSlot, score);
                
                //only keep the top 5 scores:
                highScores = highScores.GetRange(0, 5);
                
                //set up the score text string, which starts empty so we can populate it and update it:
                string scoreBoardText = "";
                
                //put each high score in the score text with a separator:
                foreach (var highScore in highScores)
                {
                    scoreBoardText += highScore + "\n";
                }
                
                //set the string equal to the score text:
                highScoresString = scoreBoardText;
                
                
                //check to see if the directory exists:
                if (!Directory.Exists(Application.dataPath + FILE_DIR))
                {
                    Directory.CreateDirectory(Application.dataPath + FILE_DIR);
                }
                
                
                //write all the scores (in string form) to a text file to save:
                File.WriteAllText(FILE_FULL_PATH, highScoresString);
            }
        }
    }
    //SCENE MANAGER SETUP:
    //set up a timer:
    public float timeLeft = 10f;
    
    //check if the game scene is running so the game doesn't try to load the scenes over and over:
    bool isInGame = true;
    
    //declare the empty string where the high scores will go for the text file:
    private string highScoresString = "";
    
    //create the list of high scores:
    List<int> highScores;
    
    //property for the list:
    public List<int> HighScores
    {
        get
        {
            
            //if it's empty:
            if (highScores == null)
            {
                //make a new list:
                highScores = new List<int>();
                
                //had to initialize a list in order to get the text file to exist
                //so now I'm commenting these out
                //highScores.Add(0);
                //highScores.Insert(0,3);
                //highScores.Insert(1,2);
                //highScores.Insert(2,1);
                
                //check if the file exists and read its contents:
                if (File.Exists(FILE_FULL_PATH))
                {
                    //pull the string from the text file and assign it to highScoresString:
                    highScoresString = File.ReadAllText(FILE_FULL_PATH);
                    //trim the white space:
                    highScoresString = highScoresString.Trim();
                    //split it based on a separator (new lines, in this case):
                    string[] highScoreArray = highScoresString.Split("\n");

                    //iterate through the array, translate it back to ints, and add the current score to it:
                    for (int i = 0; i < highScoreArray.Length; i++)
                    {
                        int currentScore = Int32.Parse(highScoreArray[i]);
                        highScores.Add(currentScore);
                    }
                }

               
            }

            return highScores;
        }
        
        

    
}

    private void Awake()
    {
        //check if there are other singletons of this type:
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //define what the full file path will be based on the operating system:
        FILE_FULL_PATH = Application.dataPath + FILE_DIR + DATA_FILE;
    }

    // Update is called once per frame
    void Update()
    {
        //is the game scene running? if so, change display text
        if (isInGame) //assumes true
        {
            display.text = "Score: " + score + "\nTime: " + ((int)timeLeft);
        }
        else
        {
            display.text = "GAME OVER. The ants have won.\nFINAL SCORE: " + score +
                           "\nHigh Scores:\n" + highScoresString;
        }
        
        //update the timer:
        timeLeft -= Time.deltaTime;
        
        //if time is up:
        if (timeLeft <= 0 && isInGame)
        {
            isInGame = false;
            SceneManager.LoadScene("EndScene");
        }
        
    }

    //create a function that determines if the score is or is not a high score:
    bool isHighScore(int score)
    {
        for (int i = 0; i < HighScores.Count; i++)
        {
            //iterate through every slot in the HighScores list and check against the score value
            if (highScores[i] < score)
            {
                return true;
            }
        }

        //default is false:
        return false;
    }
}
