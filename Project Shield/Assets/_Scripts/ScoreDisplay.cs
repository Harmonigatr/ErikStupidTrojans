using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour {

    [SerializeField]
    private Text scoreLabel;

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    void Start () {
        PlayerControl.Instance.ScoreChanged += PlayerControl_ScoreChanged;
        UpdateScoreLabel(0);
	}

    /********************************************************************************************/
    /************************************* EVENT LISTENERS **************************************/
    /********************************************************************************************/

    private void PlayerControl_ScoreChanged(object sender, PlayerControl.ScoreChangedArgs e)
    {
        UpdateScoreLabel(e.newScore);
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Updates the score label with the specified score.
    /// </summary>
    /// <param name="score">The score to set the label to display.</param>
    private void UpdateScoreLabel(int score)
    {
        scoreLabel.text = "X " + score.ToString();
    }


}
