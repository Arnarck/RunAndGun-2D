using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform ResetPoint;

    public float Speed;

    private Transform camPos;

    private float previousPos;
    private float currentPos;

    // Start is called before the first frame update
    void Start()
    {
        camPos = Camera.main.transform;
        previousPos = camPos.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (!UIManager.instance.GameIsPaused)
        {
            Movement();
        }
    }

    private void Movement()
    {
        currentPos = camPos.position.x;

        if (camPos.position.x > ResetPoint.position.x)
        {
            transform.position = new Vector3(camPos.position.x, transform.position.y, transform.position.z);
        }
        
        if (currentPos > previousPos)
        {
            transform.Translate(Vector3.left * Time.deltaTime * Speed);
        }

        previousPos = camPos.position.x;

    }
}
