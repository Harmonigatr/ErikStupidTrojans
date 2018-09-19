using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocko : MonoBehaviour {

    public static Queue<Rocko> PooledRockos = new Queue<Rocko>();
    [SerializeField]
    private float fadeTime = 2f;
    [SerializeField, Tooltip("The minimum vertical velocity the Rocko must maintain before being destroyed.")]
    private float minYVelocity;

    private SpriteRenderer spriteRenderer;
    private Color initialColor;
    private bool isFading = false;

    private Rigidbody2D rb2d;
    private AudioSource audioSource;

    private Coroutine cr_fading = null;

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        initialColor = spriteRenderer.color;

        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        ResetRocko();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isFading)
        {
            if (Mathf.Abs(rb2d.velocity.y) < minYVelocity)
            {
                StartFade();
            } else
            {
                audioSource.Play();
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 16)
        {
            StartFade();
        }
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Begin the process of fading the Rocko out.
    /// </summary>
    private void StartFade()
    {
        StopFade();
        cr_fading = StartCoroutine(FadeThenDisable());
    }

    /// <summary>
    /// Stop all fading Coroutines on the Rocko.
    /// </summary>
    private void StopFade()
    {
        if (cr_fading != null)
        {
            StopCoroutine(cr_fading);
            cr_fading = null;
        }
    }

    /// <summary>
    /// Coroutine that will fade the Rocko's alpha to 0, then disable it.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeThenDisable()
    {
        Color newCol = spriteRenderer.color;
        float t = fadeTime;
        gameObject.layer = 13;
        isFading = true;
        while (t > 0)
        {
            t -= Time.deltaTime;
            newCol.a = Mathf.Lerp(initialColor.a, 0, 1 - (t / fadeTime));
            spriteRenderer.color = newCol;
            yield return null;
        }
        isFading = false;
        PooledRockos.Enqueue(this);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Resets the Rocko to its initialized state.
    /// </summary>
    private void ResetRocko()
    {
        spriteRenderer.color = initialColor;
        gameObject.layer = 17;
    }
}
