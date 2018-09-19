using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShield : MonoBehaviour {

    [SerializeField]
    private float fadeTime;

    private Color defaultColor;

    private SpriteRenderer spriteRenderer;
    private Color fadedColor;

    private Coroutine cr_Fading = null;

    private AudioSource audioSource;

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        defaultColor = spriteRenderer.color;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        transform.rotation = new Quaternion();
        spriteRenderer.color = defaultColor;
        audioSource.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Projectile" && cr_Fading == null)
        {
            cr_Fading = StartCoroutine(FadeAndDisable());
        }
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Coroutine that will fade the shield's alpha to 0, then disable it.
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeAndDisable()
    {
        float t = 0;
        float factor;

        fadedColor = defaultColor;

        while(spriteRenderer.color.a > 0)
        {
            t += Time.deltaTime;
            factor = 1- (t / fadeTime);

            fadedColor.a = defaultColor.a * factor;
            spriteRenderer.color = fadedColor;

            yield return null;
        }

        cr_Fading = null;
        gameObject.SetActive(false);
    }
}
