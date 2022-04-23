using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform PlayerPos;

    public float Delay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIManager.instance.GameIsPaused)
        {
            transform.position = new Vector3(PlayerPos.transform.position.x + Delay, transform.position.y, transform.position.z);
        }
    }
}
