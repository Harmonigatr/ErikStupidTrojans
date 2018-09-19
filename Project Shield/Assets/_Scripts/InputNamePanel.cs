using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputNamePanel : MonoBehaviour {

    [SerializeField]
    private InputField nameField;

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void OnEnable()
    {
        nameField.text = "";
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Submit the HighScore to the HighScoreManager.
    /// </summary>
    public void EnterHighScore()
    {
        string name = nameField.text;
        if(name == "")
        {
            name = "Noob";
        }
        
        HighScoreManager.AddHighScore(new HighScore(name, PlayerControl.Instance.Score));

        gameObject.SetActive(false);
    }
}
