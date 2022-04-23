using System.Collections;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AnimatorController defaultPlayer;
    public AnimatorController[] players;
    public GameObject ShotPrefab;
    public GameObject JetpackSmoke;
    public Transform ShotSpawnPoint;

    public float FireRate;
    public float Speed;
    public float JumpForce;

    private Rigidbody2D rigidBody;
    private Animator animator;

    private bool CanJump = true;
    private bool CanShoot = true;
    private bool isAlive = true;

    private int JetpackFuel = 100;
    private int HitPoints = 10;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("CharSelected") == 6)
        {
            transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = defaultPlayer;
        }
        else
        {
            transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = players[PlayerPrefs.GetInt("CharSelected")];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = transform.GetChild(0).GetComponent<Animator>();

        UIManager.instance.SetMaxFuel(JetpackFuel);
        UIManager.instance.SetMaxLife(HitPoints);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D) && CanShoot && !UIManager.instance.GameIsPaused && isAlive)
        {
            Shoot();
        }
    }


    private void FixedUpdate()
    {
        if (isAlive)
        {
            Movement();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            JetpackSmoke.SetActive(false);
            animator.SetBool("isJumping", false);
        }
        else if (collision.gameObject.CompareTag("Spikes"))
        {
            LoseLife(HitPoints);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            RestoreFuel(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            LoseLife(1);
        }
    }

    private void Movement()
    {
        rigidBody.velocity = new Vector2(Speed, rigidBody.velocity.y);

        if (Input.GetKey(KeyCode.W) && CanJump)
        {
            //Zera a velocidade em Y para que não aja uma força puxando o player para baixo quando ele pular
            rigidBody.velocity = Vector2.right * rigidBody.velocity.x;
            rigidBody.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);

            JetpackSmoke.SetActive(true);
            animator.SetBool("isJumping", true);

            ConsumeFuel(1);
        }

        //Impede que o player saia da área do jogo
        rigidBody.position = new Vector2(rigidBody.position.x, Mathf.Clamp(rigidBody.position.y, -6f, 5f));
    }

    private void Shoot()
    {
        CanShoot = false;
        Instantiate(ShotPrefab, ShotSpawnPoint.position, Quaternion.identity);
        StartCoroutine("ShotCoolDown");
    }

    private void ConsumeFuel(int fuelConsumed)
    {
        JetpackFuel -= fuelConsumed;

        if (JetpackFuel <= 0)
        {
            JetpackFuel = 0;
            CanJump = false;
            JetpackSmoke.SetActive(false);
        }
       
        UIManager.instance.SetFuel(JetpackFuel);
    }

    private void RestoreFuel(int fuelRestored)
    {
        CanJump = true;

        if (JetpackFuel < 100)
        {
            JetpackFuel += fuelRestored;
        }
        else
        {
            JetpackFuel = 100;
        }
        
        UIManager.instance.SetFuel(JetpackFuel);
    }

    public void LoseLife(int hits)
    {
        HitPoints -= hits;

        if (HitPoints <= 0)
        {
            HitPoints = 0;
            isAlive = false;
            rigidBody.velocity = new Vector2(0f, rigidBody.velocity.y);

            JetpackSmoke.SetActive(false);
            UIManager.instance.StopCoroutine("UpdateScore");
            StopAllCoroutines();
            animator.SetTrigger("isDead");
            StartCoroutine("GameOver");
        }

        UIManager.instance.SetLife(HitPoints);
    }

    private IEnumerator ShotCoolDown()
    {
        yield return new WaitForSeconds(FireRate);
        CanShoot = true;
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.75f);
        UIManager.instance.GameOver();
    }
}
