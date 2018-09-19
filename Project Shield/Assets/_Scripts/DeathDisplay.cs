using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathDisplay : MonoBehaviour {

    [SerializeField]
    private Text deathsLabel;

	// Use this for initialization
	void Start () {
        GameManager.Instance.DeathsChanged += GameManager_DeathsChanged;
	}

    private void GameManager_DeathsChanged(object sender, GameManager.DeathsChangedArgs e)
    {
        deathsLabel.text = e.newDeaths.ToString() + "/" + GameManager.Instance.DeathsAllowed.ToString();
    }
}
