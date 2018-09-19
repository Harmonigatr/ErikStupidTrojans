using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreDisplay : MonoBehaviour {

    [SerializeField]
    private Text highScoreName;
    [SerializeField]
    private Text highScoreNumber;

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Set's the text of the display to reflect a score.
    /// </summary>
    /// <param name="highScore">The score that should be shown.</param>
    public void SetHighScore(HighScore highScore)
    {
        if(highScore == null)
        {
            highScoreName.text = "--";
            highScoreNumber.text = "--";
        }
        else
        {
            highScoreName.text = highScore.name;
            highScoreNumber.text = highScore.score.ToString();
        }
    }
}
