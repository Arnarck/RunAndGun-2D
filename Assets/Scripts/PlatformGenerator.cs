using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public Transform SpawnPoint;
    public List<GameObject> Platforms;
    public GameObject SpikesPrefab;

    public int MaxSpikesNumber;

    // Update is called once per frame
    void Update()
    {
        if (SpawnPoint.position.x >= transform.position.x && !UIManager.instance.GameIsPaused)
        {
            GeneratePlatform();
        }
    }

    private void GeneratePlatform()
    {
        int platform = Random.Range(0, Platforms.Count);
        int numSpikes = Random.Range(0, MaxSpikesNumber);

        for (int i = 0; i <= numSpikes; i++)
        {
            float spikesColliderSizeX = SpikesPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

            transform.Translate(spikesColliderSizeX / 2f, 0f, 0f);

            Instantiate(SpikesPrefab, new Vector3(transform.position.x, SpikesPrefab.transform.position.y, transform.position.z), Quaternion.identity);

            transform.Translate(spikesColliderSizeX / 2f, 0f, 0f);
        }

        if (Platforms[platform].GetComponent<BoxCollider2D>())
        {
            float platformBColliderSizeX = Platforms[platform].GetComponent<BoxCollider2D>().size.x;

            transform.Translate(platformBColliderSizeX / 2f, 0f, 0f);

            Instantiate(Platforms[platform], new Vector3(transform.position.x, Platforms[platform].transform.position.y, transform.position.z), Quaternion.identity);

            transform.Translate(platformBColliderSizeX / 2f, 0f, 0f);
        }
        else if (Platforms[platform].GetComponent<PolygonCollider2D>())
        {
            GameObject PolygonPlatform = Instantiate(Platforms[platform], new Vector3(transform.position.x, Platforms[platform].transform.position.y, transform.position.z), Quaternion.identity);

            float platformPColliderSizeX = PolygonPlatform.GetComponent<PolygonCollider2D>().bounds.size.x;

            transform.Translate(platformPColliderSizeX / 2f, 0f, 0f);

            PolygonPlatform.transform.position = new Vector3(transform.position.x, PolygonPlatform.transform.position.y, PolygonPlatform.transform.position.z);

            transform.Translate(platformPColliderSizeX / 2f, 0f, 0f);
        }

    }
}
