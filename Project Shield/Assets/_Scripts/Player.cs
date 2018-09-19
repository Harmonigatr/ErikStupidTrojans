using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private float speed = 10;
    [SerializeField]
    private float accelerationSpeed = 0.1f;
    private Rigidbody2D rb2d;

    private Vector2 translationVector = new Vector2();
    [SerializeField]
    private GameObject shieldUmbrella;
    [SerializeField]
    private GameObject shieldProjectile;
    [SerializeField]
    private Vector2 shieldThrowForce;

    private Animator animator;
    private bool isRunning = false;

    /********************************************************************************************/
    /************************************* UNITY BEHAVIOURS *************************************/
    /********************************************************************************************/

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update() {

        if (GameManager.Instance.state != GameManager.GameState.PLAYING || GameManager.Instance.Paused) {
            animator.Play("idle");
            return;
        }

        if (Input.GetKey(KeyCode.A))//Handle move left
        {
            if(!(translationVector.x <= -speed))
            {
                translationVector.x-= accelerationSpeed;
            }
            
            rb2d.velocity = translationVector;
            if (!isRunning)
            {
                animator.Play("run");
            }
            isRunning = true;

        } else if (Input.GetKey(KeyCode.D))//Handle move right
        {
            if(!(translationVector.x >= speed))
            {
                translationVector.x += accelerationSpeed;
            }

            rb2d.velocity = translationVector;
            animator.Play("run");
            isRunning = true;
        }

        if(Input.GetKeyDown(KeyCode.S) && !shieldUmbrella.activeSelf)//Handle shield platform deployment
        {
            DeployShieldUmbrella();
        }

        if(Input.GetKeyDown(KeyCode.W) && !shieldProjectile.activeSelf)//Handle shield chuck
        {
            DeployShieldProjectile();
        }

        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) //Handle movement halted
        {
            translationVector = new Vector2();
            if (isRunning)
            {
                animator.Play("idle");
            }
            isRunning = false;
        }
	}

    /********************************************************************************************/
    /**************************************** BEHAVIOURS ****************************************/
    /********************************************************************************************/

    /// <summary>
    /// Deploys a rolling shield platform at the player's position.
    /// </summary>
    private void DeployShieldUmbrella()
    {
        Vector3 pos = new Vector3(transform.position.x, shieldUmbrella.transform.position.y, shieldUmbrella.transform.position.z);
        shieldUmbrella.transform.position = pos;
        shieldUmbrella.SetActive(true);
    }

    /// <summary>
    /// Throws a shield vertically from the player's position.
    /// </summary>
    private void DeployShieldProjectile()
    {
        shieldProjectile.SetActive(true);
        shieldProjectile.transform.position = transform.position;
        Rigidbody2D shieldRb2d = shieldProjectile.GetComponent<Rigidbody2D>();
        shieldRb2d.AddForce(shieldThrowForce);
        shieldRb2d.AddTorque(UnityEngine.Random.Range(-30f, 30f));
    }
}
