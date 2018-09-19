using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour {

    [SerializeField]
    private Image image;
    [SerializeField]
    private GameObject pauseButton;

    [SerializeField]
    private Sprite playGraphic;
    [SerializeField]
    private Sprite pauseGraphic;

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    void Start () {
        GameManager.Instance.GameStateChanged += GameManager_GameStateChanged;
        GameManager.Instance.PauseStateChanged += GameManager_PauseStateChanged;
	}

    /********************************************************************************************/
    /************************************* EVENT LISTENERS **************************************/
    /********************************************************************************************/

    private void GameManager_PauseStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.Paused)
        {
            image.sprite = playGraphic;
        } else
        {
            image.sprite = pauseGraphic;
        }
    }

    private void GameManager_GameStateChanged(object sender, GameManager.GameStateChangedArgs e)
    {
        if(e.newState == GameManager.GameState.START_SCREEN || e.newState == GameManager.GameState.GAMEOVER)
        {
            SetButtonActive(false);
        } else if(e.newState == GameManager.GameState.PLAYING)
        {
            SetButtonActive(true);
        }
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Toggle the pause state of the game.
    /// </summary>
    public void TogglePauseGame()
    {
        if (GameManager.Instance.Paused)
        {
            GameManager.Instance.UnpauseGame();            
        } else
        {
            GameManager.Instance.PauseGame();
        }
    }

    /// <summary>
    /// Sets the active state of the button.
    /// </summary>
    /// <param name="active">Whether or not the button should be active.</param>
    public void SetButtonActive(bool active)
    {
        pauseButton.SetActive(active);
    }
}
