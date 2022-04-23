using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    private Transform EndPoint;

    private void Start()
    {
        EndPoint = GameObject.Find("EndPoint").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EndPoint.position.x >= transform.position.x && !UIManager.instance.GameIsPaused)
        {
            Destroy(this.gameObject);
        }
    }
}
