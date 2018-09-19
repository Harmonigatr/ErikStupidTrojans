using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class BugLog : MonoBehaviour {

    public static BugLog Instance { get; private set; }

    [SerializeField]
    private GameObject textField;
    [SerializeField]
    private Transform content;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowException(Exception e)
    {
        GameObject go = Instantiate(textField, content);
        Text t = go.GetComponent<Text>();
        t.text = e.Message;
    }

   
}
