using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Troop : MonoBehaviour {

    private Rigidbody2D rb2d;
    private bool isAlive = true;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] hitSounds;

    private Animator animator;

    public static List<Troop> Troops = new List<Troop>();

    private Coroutine fading = null;

    [SerializeField]
    private float speed = 3;

    private Vector2 speedVector = new Vector2();

    private string deathAnim = "death_0";

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        animator.Play("run");
    }

    void Start () {
        speedVector.x = speed;
        GameManager.Instance.GameStateChanged += GameManager_GameStateChanged;
	}

    void Update()
    {
        if (isAlive)
        {
            rb2d.velocity = speedVector;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Projectile" && fading == null)
        {
            if (collision.gameObject.layer == 17) //If hit by Rocko
            {
                deathAnim = "death_splat";
            }
            else
            {
                deathAnim = "death_0";
            }

            Kill();

        }
        else if (collision.gameObject.tag == "Gate")
        {
            Succeed();
        }
    }

    /********************************************************************************************/
    /************************************* EVENT LISTENERS **************************************/
    /********************************************************************************************/

    private void GameManager_GameStateChanged(object sender, GameManager.GameStateChangedArgs e)
    {
        if (e.newState == GameManager.GameState.GAMEOVER)
        {
            deathAnim = "death_0";
            Kill();
        }
        else if (e.newState == GameManager.GameState.START_SCREEN)
        {
            Troops.Add(this);
            if (fading != null)
            {
                StopCoroutine(fading);
                fading = null;
            }
            gameObject.layer = 13;
            gameObject.SetActive(false);
        }
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Kill the troop.
    /// </summary>
    private void Kill()
    {
        if (!isAlive) { return; }
        isAlive = false;
        animator.Play(deathAnim);
        audioSource.clip = hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)];
        audioSource.Play();
        if(deathAnim == "death_splat") // ugh. String literals. If only I had more time.
        {
            rb2d.velocity = new Vector2();
        }
        else
        {
            rb2d.AddForce(new Vector2(-10, 30f));
        }
        

        if (gameObject.activeSelf) { fading = StartCoroutine(FadeThenDestroy(5)); }
        

        gameObject.layer = 13;

        GameManager.Instance.AddDeath();
    }

    /// <summary>
    /// Handle the troop reaching the objective.
    /// </summary>
    private void Succeed()
    {
        PlayerControl.Instance.IncrementScore(1);
        Troops.Add(this);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Coroutine that fades the Troop's alpha to 0 then disables it.
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator FadeThenDestroy(float time)
    {
        float t = time;
        Color color = spriteRenderer.color;
        while (t > 0)
        {
            t -= Time.deltaTime;
            color.a = t / time;
            spriteRenderer.color = color;
            yield return null;
        }
        Troops.Add(this);
        fading = null;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Returns the Troop to its initialized state.
    /// </summary>
    public void Resurrect()
    {
        isAlive = true;
        gameObject.layer = 11;
        Color col = spriteRenderer.color;
        col.a = 1;
        spriteRenderer.color = col;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
