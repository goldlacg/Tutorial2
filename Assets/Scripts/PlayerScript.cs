using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public float jumpForce;
    public TextMeshProUGUI score;
    private int scoreValue = 0;
    public GameObject winTextObject;
    public TextMeshProUGUI lives;
    private int livesValue = 3;
    public GameObject loseTextObject;
    public GameObject player;
    public AudioSource musicSource;
    public AudioClip backgroundMusic;
    public AudioClip winMusic;
    Animator anim;
    private bool facingRight = true;
    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;



    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();

        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);

        musicSource.clip = backgroundMusic;
        musicSource.Play();
        musicSource.loop = true;

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 2);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 2);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);

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
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            if(scoreValue == 4)
            {
                transform.position = new Vector2(84f, 0f);
                livesValue = 3;
                lives.text = "Lives: " + livesValue.ToString();
            }
            
            if(scoreValue == 8)
            {
                winTextObject.SetActive(true);
                musicSource.Stop();
                musicSource.clip = winMusic;
                musicSource.Play();
                musicSource.loop = false;
            }
        }

        if(collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if(livesValue == 0)
        {
            loseTextObject.SetActive(true);
            Destroy(player);
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            anim.SetBool("isOnGround", true);

            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                anim.SetBool("isOnGround", false);
            }
        }
    }
}
