using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public List<GameObject> Enemies;

    public float MinSpawnY, MaxSpawnY;
    public float MinTimeToSpawn, MaxTimeToSpawn;
    
    private bool CanSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CanSpawn && !UIManager.instance.GameIsPaused)
        {
            SpawnEnemies();
        }
    }

    void SpawnEnemies()
    {
        CanSpawn = false;
        int randomEnemy = Random.Range(0, Enemies.Count);
        float randomY = Random.Range(MinSpawnY, MaxSpawnY);
        float randomTime = Random.Range(MinTimeToSpawn, MaxTimeToSpawn);

        Instantiate(Enemies[randomEnemy], new Vector3(transform.position.x, randomY, transform.position.z), Quaternion.identity);
        StartCoroutine("SpawnCoolDown", randomTime);
    }

    IEnumerator SpawnCoolDown(float time)
    {
        yield return new WaitForSeconds(time);
        CanSpawn = true;
    }
}
