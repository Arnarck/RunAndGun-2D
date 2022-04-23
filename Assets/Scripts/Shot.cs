using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Shot : MonoBehaviour
{
    public GameObject DestroyEffect;

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
        rigidBody.velocity = Vector2.right * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Coin"))
        {
            if (collision.gameObject.layer == 8)
            {
                Instantiate(DestroyEffect, transform.position, Quaternion.identity);
            }

            Destroy(this.gameObject);
        }
    }
}
