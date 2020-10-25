using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public Text score;
    public Text lives;
    public Text winText;
    public Text loseText;
    public Text controlText;

    private int scoreValue = 0;
    private int livesValue = 3;

    private bool facingRight = true;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    public float speed;

    public AudioSource musicSource;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();

        score.text = scoreValue.ToString();
        lives.text = livesValue.ToString();
        controlText.text = "Controls: WASD";
        SetScore();
        SetLives();

        winText.text =  "";
        loseText.text = "";

        musicSource.clip = musicClipOne;
        musicSource.Play();

        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            SetScore();
            Destroy(collision.collider.gameObject);

            if (scoreValue == 5)
            {
                transform.position = new Vector2(24.0f, -1.0f);
                livesValue = 3;
                SetLives();
            }
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            SetLives();
            Destroy(collision.collider.gameObject);
        }
  

        if (scoreValue == 9)
        {
            winText.text = "You win! - Callie Figueroa";

            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.Play();
        }
        if (livesValue == 0)
        {
            loseText.text = "You lose!";
            Destroy(this);
            musicSource.Stop();
        }

    }

    void SetScore()
    {
        score.text = "Score: " + scoreValue.ToString();
        if (scoreValue >= 9 && winText != null)
        {
            winText.text = "You win! - Callie Figueroa";
            Destroy(this);
            musicSource.Stop();
        }
    }
    void SetLives()
    {
        lives.text = "Lives: " + livesValue.ToString();
        if (livesValue <= 0)
        {
            loseText.text = "You lose!";
            Destroy(this);
            musicSource.Stop();
        }
    } 

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                anim.SetTrigger("takeOff");
                
            }
        }
    }
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.D))

        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyDown(KeyCode.A))

        {
            anim.SetInteger("State", 1);
        }
        if (Input.GetKeyUp(KeyCode.D))

        {
            anim.SetInteger("State", 0);
        }
        if (Input.GetKeyUp(KeyCode.A))

        {
            anim.SetInteger("State", 0);
        }

    }
}