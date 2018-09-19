using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    private Rigidbody2D rb2d;
    private Collider2D col2d;
    private bool isFlying = true;
    private SpriteRenderer spriteRenderer;
    private TrailRenderer trailRenderer;
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip blockedSound;


    public static List<Arrow> Arrows = new List<Arrow>();

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
    }
	
	void Update () {
        if (isFlying)
        {
            transform.up = rb2d.velocity; //Point arrow in direction of movement.
        }
	}

    private void OnDisable()
    {
        Color color = spriteRenderer.color;
        color.a = 0;
        spriteRenderer.color = color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 10)
        {
            audioSource.clip = blockedSound;
            audioSource.Play();
        }
        
        //Remove Arrow from physics sim and add as child of collided object.
        rb2d.isKinematic = true;
        col2d.enabled = false;
        isFlying = false;
        rb2d.velocity = new Vector2();
        rb2d.angularVelocity = 0;
        StartCoroutine(Fade(2f));
        transform.parent = collision.gameObject.transform;

        DisableTrail();
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Return the Arrow to its initialized state.
    /// </summary>
    public void Revert()
    {
        rb2d.isKinematic = false;
        col2d.enabled = true;
        isFlying = true;
        Color color = spriteRenderer.color;
        color.a = 1;
        spriteRenderer.color = color;
    }

    /// <summary>
    /// Enable the Arrow's trail renderer component.
    /// </summary>
    public void EnableTrail()
    {
        trailRenderer.enabled = true;
        trailRenderer.Clear();

    }

    /// <summary>
    /// Disable the Arrow's trail renderer component.
    /// </summary>
    public void DisableTrail()
    {
        trailRenderer.enabled = false;
    }

    /// <summary>
    /// Fades the Arrow's alpha to 0 before disabling it.
    /// </summary>
    /// <param name="time">The length of time the fade should take.</param>
    /// <returns></returns>
    private IEnumerator Fade(float time)
    {
        float t = time;
        Color color = spriteRenderer.color;
        while(t > 0)
        {
            t -= Time.deltaTime;
            color.a = t / time;
            spriteRenderer.color = color;
            yield return null;
        }
        DisableTrail();
        Arrows.Add(this);
        gameObject.SetActive(false);
    }
}
