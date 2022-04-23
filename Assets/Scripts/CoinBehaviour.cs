using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    public GameObject CollectedPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.instance.UpdateCoins();
            Instantiate(CollectedPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
