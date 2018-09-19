using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gate : MonoBehaviour {

    [SerializeField]
    private GameObject plusOneTextObject;
    [SerializeField]
    private Color textColor;
    [SerializeField]
    private float delayBeforeFade = 2;
    [SerializeField]
    private float fadeTime = 2;
    [SerializeField]
    private Vector2 maxScoreForce;
    [SerializeField]
    private Vector2 minScoreForce;
    [SerializeField]
    private float variableScoreTorque;

    private AudioSource audioSource;

    private Queue<GameObject> plusOneTextPool = new Queue<GameObject>();

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start () {
        PlayerControl.Instance.ScoreChanged += PlayerControl_ScoreChanged;
	}

    /********************************************************************************************/
    /************************************* EVENT LISTENERS **************************************/
    /********************************************************************************************/

    private void PlayerControl_ScoreChanged(object sender, PlayerControl.ScoreChangedArgs e)
    {
        if(e.newScore > e.oldScore)
        {
            CreatePlusOneText();
        }
        
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Place the PlusOneText in the game.
    /// </summary>
    private void CreatePlusOneText()
    {
        GameObject floatingText = GetPlusOneText();

        audioSource.Play();

        StartCoroutine(LaunchScoreThenFade(floatingText, fadeTime));
        
    }

    /// <summary>
    /// Get a PlusOneText GameObject.
    /// </summary>
    /// <returns>A PlusOneText GameObject.</returns>
    private GameObject GetPlusOneText()
    {
        GameObject go;
        if(plusOneTextPool.Count > 0) //If there are texts in the pool, take from there...
        {
            go = plusOneTextPool.Dequeue();

        } else //...else instantiate one.
        {
            go = Instantiate(plusOneTextObject);
        }

        ResetPlusOneText(go);

        return go;
    }

    /// <summary>
    /// Reset the PlusOneText to its initialized state.
    /// </summary>
    /// <param name="go">The PlusOneText GameObject.</param>
    private void ResetPlusOneText(GameObject go)
    {
        go.GetComponentInChildren<Text>().color = textColor;
        go.transform.position = transform.position;
        go.transform.rotation = new Quaternion();
        go.SetActive(true);
    }

    /// <summary>
    /// Coroutine to launch the text downwards, then fade it to 0 alpha after a delay.
    /// </summary>
    /// <param name="go">The PlusOneText GameObject.</param>
    /// <param name="time">The duration of the fade.</param>
    /// <returns></returns>
    private IEnumerator LaunchScoreThenFade(GameObject go, float time)
    {

        float t = time;
        float factor;

        Text text = go.GetComponentInChildren<Text>();
        Color col = text.color;
        Rigidbody2D rb2d = go.GetComponent<Rigidbody2D>();

        //Launch the text.
        Vector2 launchForce = new Vector2(UnityEngine.Random.Range(minScoreForce.x, maxScoreForce.x), UnityEngine.Random.Range(minScoreForce.y, maxScoreForce.y));

        rb2d.AddForce(launchForce);
        rb2d.AddTorque(UnityEngine.Random.Range(-variableScoreTorque, variableScoreTorque));

        //Wait for a moment before fading.
        yield return new WaitForSeconds(delayBeforeFade);

        while (t > 0)
        {
            t -= Time.deltaTime;
            factor = t / time;
            col.a = factor;
            text.color = col;
            yield return null;
        }

        go.SetActive(false);
        plusOneTextPool.Enqueue(go); //Pool the text.

    }
}
