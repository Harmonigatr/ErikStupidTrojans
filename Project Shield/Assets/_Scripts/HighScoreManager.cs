using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public static class HighScoreManager {

    public static int NumOfHighScores { get { return highScores.Count; } }
    private static List<HighScore> highScores = new List<HighScore>();
    private static int _maxNumOfHighScores = 10;
    public static int MaxNumOfHighScores { get { return _maxNumOfHighScores; } }

    private static string saveKey = "highScoresJson";

    /********************************************************************************************/
    /*************************************** CONSTRUCTORS ***************************************/
    /********************************************************************************************/

    static HighScoreManager()
    {
        LoadHighScores();
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Adds a high score to the list. Will not add if it is not higher than the lowest high score.
    /// </summary>
    /// <param name="highScore">The score to add to the list.</param>
    public static void AddHighScore( HighScore highScore )
    {
        if(highScores.Count == 0) //Any score can be added if no high scores exist.
        {
            highScores.Insert(0, highScore);
            NewHighScoreArgs args = new NewHighScoreArgs() { newScore = highScore.score };
            OnNewHighScore(args);
        }
        else
        {
            for (int i = 0; i < highScores.Count; i++) //Find the appropriate ordered position for the new score.
            {
                if (highScore.score > highScores[i].score)
                {
                    highScores.Insert(i, highScore);
                    if (highScores.Count > _maxNumOfHighScores) //cull high scores that exist beyond the limit.
                    {
                        for (int c = highScores.Count - 1; c >= _maxNumOfHighScores; c--)
                        {
                            highScores.RemoveAt(c);
                        }
                    }
                    NewHighScoreArgs args = new NewHighScoreArgs() { newScore = highScore.score };
                    OnNewHighScore(args);
                    return;
                }
            }
            if(highScores.Count < _maxNumOfHighScores)//Any high score can be added if the list is not full.
            {
                highScores.Add(highScore);
                NewHighScoreArgs args = new NewHighScoreArgs() { newScore = highScore.score };
                OnNewHighScore(args);
            }
        }
    }

    /// <summary>
    /// Find the lowest score of the stores high scores.
    /// </summary>
    /// <returns>The HighScore with the lowest score.</returns>
    public static HighScore GetLowestHighScore()
    {
        if(highScores.Count == 0)
        {
            return new HighScore("", 0);
        }
        else
        {
            return highScores[highScores.Count - 1];
        }       
    }

    /// <summary>
    /// Find the HighScore at a given index.
    /// </summary>
    /// <param name="index">The index to return.</param>
    /// <returns>The HighScore at the specified index.</returns>
    public static HighScore GetHighScoreAt(int index)
    {
        if(index < 0 || index >= highScores.Count)
        {
            return null;
        }

        return highScores[index];
    }

    /// <summary>
    /// Save the high score to PlayerPrefs.
    /// </summary>
    public static void SaveHighScores()
    {
        HighScoreWrapper wrapper = new HighScoreWrapper(); //Need to wrap the high sores because Lists cannot be serialized independently.
        wrapper.highScores = highScores.ToArray();
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(saveKey, json);
    }

    /// <summary>
    /// Load the high scores from PlayerPrefs.
    /// </summary>
    public static void LoadHighScores()
    {
        try
        {
            string json = "";

            if (PlayerPrefs.HasKey(saveKey))
            {
                json = PlayerPrefs.GetString(saveKey);
            }

            if (json != "{}")
            {
                HighScoreWrapper wrapper = JsonUtility.FromJson<HighScoreWrapper>(json);
                highScores = new List<HighScore>(wrapper.highScores);
            }
            else
            {
                highScores = new List<HighScore>();
            }
        } catch(Exception e)
        {
            BugLog.Instance.ShowException(e);
        }

    }

    /// <summary>
    /// Clear all records of HighScores.
    /// </summary>
    public static void ClearHighScores()
    {
        PlayerPrefs.DeleteKey(saveKey);
        highScores = new List<HighScore>();
    }



    /********************************************************************************************/
    /****************************************** EVENTS ******************************************/
    /********************************************************************************************/

    /// <summary>
    /// Called when a new high score is added.
    /// </summary>
    #region NewHighScore Event
    public static event EventHandler<NewHighScoreArgs> NewHighScore;

    public class NewHighScoreArgs : EventArgs
    {
        public int newScore;
    }

    private static void OnNewHighScore(NewHighScoreArgs args)
    {
        SaveHighScores();

        EventHandler<NewHighScoreArgs> handler = NewHighScore;

        if (handler != null)
        {
            handler(typeof(HighScoreManager), args);
        }
    }
    #endregion
}

/// <summary>
/// Container for HighScore data.
/// </summary>
[Serializable]
public class HighScore
{
    public int score;
    public string name;


    public HighScore(string name, int score)
    {
        this.score = score;
        this.name = name;
    }
}

/// <summary>
/// Wrapper so that the HighScore list can be serialized to JSON.
/// </summary>
[Serializable]
public class HighScoreWrapper
{
    public HighScore[] highScores;
}
