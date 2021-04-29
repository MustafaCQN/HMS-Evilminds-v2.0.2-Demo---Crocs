using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using HmsPlugin;

public class Player : MonoBehaviour
{

    public Text healthDisplay;
    public GameObject losePanel;

    Rigidbody2D rb;
    public float speed;
    Animator anim;
    private float input;
    public int health;
    Touch touch;

    public float startDashTime;
    private float dashTime;
    public float extraSpeed;
    private bool isDashing;
    public Text GameTime;
    public float DeltaTime;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        healthDisplay.text = health.ToString();
        GameTime.text = "0";
    }

    private void Update()
    {

        if (Application.platform == RuntimePlatform.Android 
            || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            AnimateForMobile();
        }else
        {
            if (input != 0)
            {
                anim.SetBool("isRunning", true);
            }
            else
            {
                anim.SetBool("isRunning", false);
            }

            // rotating the character based on running direction
            if (input > 0)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (input < 0)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }        
    }

    // physics calculate stuff
    void FixedUpdate()
    {

        DeltaTime += Time.deltaTime;
        GameTime.text = DeltaTime.ToString("0.00");

        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.position.x > Screen.width / 2)
                {
                    // right
                    rb.velocity = new Vector2(speed, rb.velocity.y);

                }
                else if (touch.position.x <= Screen.width / 2)
                {
                    // left
                    rb.velocity = new Vector2(-speed, rb.velocity.y);

                }
            }else
            {
                // because of velocity is not going to stop, when we finish clicking, we need to change the velocity to 0
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        else
        {
            input = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(input * speed, rb.velocity.y);

            if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
            {
                speed += extraSpeed;
                isDashing = true;

            }

            if (dashTime <= 0 && isDashing)
            {
                isDashing = false;
                speed -= extraSpeed;
            }
            else
            {
                dashTime -= Time.deltaTime;
            }

                
        }

    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        healthDisplay.text = health.ToString();

        if (health <= 0)
        {
            healthDisplay.text = "0";
            losePanel.SetActive(true);
            // destroy player
            Destroy(gameObject);

            // Send Leaderboard
            HMSLeaderboardManager.Instance.SubmitScore(HMSLeaderboardConstants.TimeLeaderboard, (long)DeltaTime);
        }
    }

    public void AnimateForMobile()
    {
        Debug.Log(touch.phase);
        if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved|| touch.phase == TouchPhase.Stationary)
        {
            anim.SetBool("isRunning", true);
        }else if(touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
        {
            anim.SetBool("isRunning", false);
        }

        // rotating the character based on running direction
        if (touch.position.x > Screen.width / 2)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (touch.position.x < Screen.width / 2)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

    }
}
