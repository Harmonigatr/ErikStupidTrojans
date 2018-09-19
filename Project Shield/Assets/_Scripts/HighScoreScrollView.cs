using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreScrollView : MonoBehaviour {

    [SerializeField]
    private GameObject highScoreDisplayObject;
    [SerializeField]
    private Transform content;

    private List<HighScoreDisplay> highScoreDisplays = new List<HighScoreDisplay>();

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    void Start () {
		for(int i = 0; i < HighScoreManager.MaxNumOfHighScores; i++)
        {
            GameObject go = Instantiate(highScoreDisplayObject, content);
            highScoreDisplays.Add(go.GetComponent<HighScoreDisplay>());
        }

        HighScoreManager.NewHighScore += HighScoreManager_NewHighScore;

        UpdateScores();
	}

    /********************************************************************************************/
    /************************************* EVENT LISTENERS **************************************/
    /********************************************************************************************/

    private void HighScoreManager_NewHighScore(object sender, HighScoreManager.NewHighScoreArgs e)
    {
        UpdateScores();
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Retrieve high scores from HighScoreManager and update to displayed list.
    /// </summary>
    private void UpdateScores()
    {
        for(int i = 0; i < highScoreDisplays.Count; i++)
        {
            highScoreDisplays[i].SetHighScore(HighScoreManager.GetHighScoreAt(i));
        }
    }

    /// <summary>
    /// Remove all high scores from the list.
    /// </summary>
    public void DeleteHighScores()
    {
        HighScoreManager.ClearHighScores();
        UpdateScores();
    }
}
