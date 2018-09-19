using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archers : MonoBehaviour {

    public static Archers Instance { get; set; }

    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private Vector2 maxLaunchForce = new Vector2(-1f, 1f);
    [SerializeField]
    private Vector2 minLaunchForce = new Vector2(-1f, 1f);
    [SerializeField]
    private int baseNumberOfArrows = 1;
    [SerializeField, Tooltip("Number of seconds to wait before adding another arrow to the volley.")]
    private float difficultyIncrement = 25;
    [SerializeField]
    private float timeBetweenLaunches = 3f;

    private AudioSource audioSource;
    private Coroutine cr_LaunchSequence = null;
    private Animator animator;
    private WaitForSeconds shotAnimationDelay = new WaitForSeconds(0.4f);


    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    void Start () {
        Instance = this; //Singleton pattern...

        GameManager.Instance.GameStateChanged += GameManager_GameStateChanged;
	}

    /********************************************************************************************/
    /************************************* EVENT LISTENERS **************************************/
    /********************************************************************************************/

    private void GameManager_GameStateChanged(object sender, GameManager.GameStateChangedArgs e)
    {
        if(GameManager.Instance.state != GameManager.GameState.PLAYING)
        {
            StopLaunching();
        } else 
        {
            StartLaunching();
        }
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Start the repeating launch pattern.
    /// </summary>
    public void StartLaunching()
    {
        StopLaunching();
        cr_LaunchSequence = StartCoroutine(LaunchSequence());
    }

    /// <summary>
    /// Stop the repeating lauch pattern.
    /// </summary>
    public void StopLaunching()
    {
        if(cr_LaunchSequence != null)
        {
            StopCoroutine(cr_LaunchSequence);
            cr_LaunchSequence = null;
        }
    }

    /// <summary>
    /// Launch projectiles.
    /// </summary>
    private void LaunchProjectile()
    {
        int numOfArrows = (int)((Time.time - GameManager.Instance.LevelStartTime) / difficultyIncrement) + baseNumberOfArrows;
        int c = 0;
        while (c < numOfArrows)
        {
            Rigidbody2D rb2d = GetProjectile().GetComponent<Rigidbody2D>();
            //Add random force to projectile.
            Vector2 launchForce = new Vector2(UnityEngine.Random.Range(minLaunchForce.x, maxLaunchForce.x), UnityEngine.Random.Range(minLaunchForce.y, maxLaunchForce.y));
            rb2d.AddForce(launchForce);
            c++;
        }
        audioSource.Play();
    }

    /// <summary>
    /// Retrieve a projectile GameObject.
    /// </summary>
    /// <returns>A projectile GameObject.</returns>
    private GameObject GetProjectile()
    {
        GameObject go;
        if(Arrow.Arrows.Count > 0) //If there are Arrows in the pool, take from there...
        {
            go = Arrow.Arrows[0].gameObject;
            Arrow.Arrows.RemoveAt(0);
        } else //... otherwise instantiate a new one.
        {
            go = Instantiate(projectile);
        }

        go.transform.parent = transform;
        go.transform.position = this.transform.position;
        go.SetActive(true);
        Arrow a = go.GetComponent<Arrow>();
        a.Revert();
        a.EnableTrail();

        return go;
    }

    /// <summary>
    /// Recurseive coroutine that will continually launch projectiles until explicitly stopped.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LaunchSequence()
    {
        float t = timeBetweenLaunches;

        while(t > 0)
        {
            t -= Time.deltaTime;
            yield return null;
        }
        animator.Play("shoot");
        yield return shotAnimationDelay;
        LaunchProjectile();

        cr_LaunchSequence = StartCoroutine(LaunchSequence());
    }
}
