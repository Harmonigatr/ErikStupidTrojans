using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHomeMenu : MonoBehaviour {


    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    void Start () {
        GameManager.Instance.PauseStateChanged += GameManager_PauseStateChanged;
	}


    /********************************************************************************************/
    /************************************* EVENT LISTENERS **************************************/
    /********************************************************************************************/

    private void GameManager_PauseStateChanged(object sender, System.EventArgs e)
    {
        if (!GameManager.Instance.Paused)
        {
            gameObject.SetActive(false);
        }
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Set the GameState of the game to START_SCREEN.
    /// </summary>
    public void ReturnToStart()
    {
        GameManager.Instance.ChangeGameState(GameManager.GameState.START_SCREEN);
        gameObject.SetActive(false);
    }
}
