using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUmbrella : MonoBehaviour {

    [SerializeField]
    private float duration;
    [SerializeField]
    private float flashFrequency;
    [SerializeField]
    private Color flashColor;
    [SerializeField]
    private Vector2 speedVector = new Vector2();

    private Color defaultColor;
    private bool isDefaultColor = true;
    private Rigidbody2D rb2d;
    private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        defaultColor = spriteRenderer.color;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        spriteRenderer.color = defaultColor;
        isDefaultColor = true;
        StartCoroutine(KillTimer());
        audioSource.Play();
    }

    void Update()
    {
        rb2d.velocity = speedVector;
    }

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Coroutine that waits for a set time before it flashes the image of the shield platform.
    /// </summary>
    /// <returns></returns>
    private IEnumerator KillTimer()
    {
        yield return new WaitForSeconds(duration * 0.75f);

        StartCoroutine(Flash());
    }

    /// <summary>
    /// Coroutine that flashes the alpha of the shield platform before disabling.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Flash()
    {
        float time = duration * 0.25f;

        while(time > 0)
        {
            yield return new WaitForSeconds(flashFrequency);
            if (isDefaultColor)
            {
                spriteRenderer.color = flashColor;
            } else
            {
                spriteRenderer.color = defaultColor;
            }

            isDefaultColor = !isDefaultColor;

            time -= flashFrequency;
        }

        gameObject.SetActive(false);
    }

}
