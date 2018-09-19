using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockoLauncher : MonoBehaviour {

    [SerializeField]
    private Vector2 launchVelocity;
    [SerializeField]
    private float variableTorque = 180f;
    [SerializeField]
    private GameObject rockoGO;
    [SerializeField, Tooltip("Time to wait between launches.")]
    private float launchWaitTime;
    private WaitForSeconds wfs_launchWaitTime;

    private Coroutine cr_LaunchSequence = null;

    private AudioSource audioSource;

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void Awake()
    {
        wfs_launchWaitTime = new WaitForSeconds(launchWaitTime);
        audioSource = GetComponent<AudioSource>();
    }

    void Start () {
        GameManager.Instance.GameStateChanged += GameManager_GameStateChanged;
	}

    /********************************************************************************************/
    /************************************* EVENT LISTENERS **************************************/
    /********************************************************************************************/

    private void GameManager_GameStateChanged(object sender, GameManager.GameStateChangedArgs e)
    {
        if (GameManager.Instance.state != GameManager.GameState.PLAYING)
        {
            StopLaunching();
        }
        else
        {
            StartLaunching();
        }
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Start the repeating launch sequence.
    /// </summary>
    public void StartLaunching()
    {
        StopLaunching();
        cr_LaunchSequence = StartCoroutine(LaunchSequence());
    }

    /// <summary>
    /// Halt the repeating launch sequence.
    /// </summary>
    public void StopLaunching()
    {
        if (cr_LaunchSequence != null)
        {
            StopCoroutine(cr_LaunchSequence);
            cr_LaunchSequence = null;
        }
    }

    /// <summary>
    /// Coroutine that launches Rockos at a set interval.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LaunchSequence()
    {
        yield return wfs_launchWaitTime;
        LaunchRock();
        cr_LaunchSequence = StartCoroutine(LaunchSequence());
    }

    /// <summary>
    /// Launches a Rocko.
    /// </summary>
    private void LaunchRock()
    {
        GameObject projectile = GetRocko();
        Rigidbody2D rb2d = projectile.GetComponent<Rigidbody2D>();
        audioSource.Play();
        rb2d.AddForce(launchVelocity);
        rb2d.angularVelocity = UnityEngine.Random.Range(-variableTorque, variableTorque);
    }

    /// <summary>
    /// Retrieves a Rocko GameObject.
    /// </summary>
    /// <returns>A Rocko GameObject.</returns>
    private GameObject GetRocko()
    {
        GameObject rocko;
        if(Rocko.PooledRockos.Count > 0) //If there are pooled Rockos, grab one...
        {
            rocko = Rocko.PooledRockos.Dequeue().gameObject;
        } else //... else instantiate one.
        {
            rocko = Instantiate(rockoGO);
        }

        rocko.transform.position = transform.position;
        rocko.SetActive(true);

        return rocko;
    }
}
