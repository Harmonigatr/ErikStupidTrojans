using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour {

    [SerializeField]
    private Text bigText;

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void OnEnable()
    {
        if(GameManager.Instance.state == GameManager.GameState.GAMEOVER)
        {
            bigText.text = "GAME OVER";
        } else if(GameManager.Instance.state == GameManager.GameState.START_SCREEN)
        {
            bigText.text = "Stupid Trojans";
        }
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Change the game's GameState to PLAYING.
    /// </summary>
    public void StartGame()
    {
        GameManager.Instance.ChangeGameState(GameManager.GameState.PLAYING);
        gameObject.SetActive(false);
    }
}
