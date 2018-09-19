using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public static PlayerControl Instance { get { return _instance; } }
    private static PlayerControl _instance;

    private int _score;
    public int Score { get { return _score; } }

    [SerializeField]
    private GameObject newHighScorePanel;


    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(_instance.gameObject);
        }

        _instance = this;

        _score = 0;
    }

    void Start () {
        GameManager.Instance.GameStateChanged += GameManager_GameStateChanged;
    }

    /********************************************************************************************/
    /************************************* EVENT LISTENERS **************************************/
    /********************************************************************************************/

    /// <summary>
    /// Responds to GameState changes.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GameManager_GameStateChanged(object sender, GameManager.GameStateChangedArgs e)
    {
        if (e.newState == GameManager.GameState.PLAYING)
        {
            SetScore(0);
        }
        else if (e.newState == GameManager.GameState.GAMEOVER)
        {
            if (Score > HighScoreManager.GetLowestHighScore().score || HighScoreManager.NumOfHighScores < HighScoreManager.MaxNumOfHighScores)
            {
                newHighScorePanel.SetActive(true);
            }
        }
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Adds byAmount to score. Can be negative.
    /// </summary>
    /// <param name="byAmount">Amount to increment score by.</param>
    public void IncrementScore(int byAmount)
    {
        ScoreChangedArgs args = new ScoreChangedArgs();
        args.oldScore = _score;
        _score += byAmount;
        args.newScore = _score;

        OnScoreChanged(args);

    }

    /// <summary>
    /// Sets the score to a specified value.
    /// </summary>
    /// <param name="score">Number to set the score to.</param>
    public void SetScore(int score)
    {
        ScoreChangedArgs args = new ScoreChangedArgs();
        args.oldScore = _score;
        _score = score;
        args.newScore = _score;

        OnScoreChanged(args);
    }

    /********************************************************************************************/
    /****************************************** EVENTS ******************************************/
    /********************************************************************************************/

    /// <summary>
    /// Called when the score changes.
    /// </summary>
    #region ScoreChanged Event
    public event EventHandler<ScoreChangedArgs> ScoreChanged;

    public class ScoreChangedArgs : EventArgs
    {
        public int newScore;
        public int oldScore;
    }

    private void OnScoreChanged(ScoreChangedArgs args)
    {
        EventHandler<ScoreChangedArgs> handler = ScoreChanged;

        if(handler != null)
        {
            handler(this, args);
        }
    }
    #endregion

    /// <summary>
    /// Called when a new HighScore is achieved.
    /// </summary>
    #region NewHighScore Event
    public event EventHandler<NewHighScoreArgs> NewHighScore;

    public class NewHighScoreArgs : EventArgs
    {
        public int newScore;
    }

    private void OnNewHighScore(NewHighScoreArgs args)
    {
        EventHandler<NewHighScoreArgs> handler = NewHighScore;

        if(handler != null)
        {
            handler(this, args);
        }
    }
    #endregion
}
