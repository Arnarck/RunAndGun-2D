using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject ExplosionPrefab;

    public float Speed;
    public float timeToDestroy;

    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player_Shot"))
        {
            UIManager.instance.IncrementScore(10);
        }
        if (collision.CompareTag("Player_Shot") || collision.gameObject.layer == 8 || collision.CompareTag("Player"))
        {
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void Movement()
    {
        rigidBody.velocity = new Vector2(-Speed, 0f);
    }
}
