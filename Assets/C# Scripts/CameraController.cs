using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 8, -10);
    public GameObject player;

    // start is called before the first frame update
    void Start()
    {
        
    }

    // update is called once per frame
    void Update()
    {
        transform.position = player.transform.position + offset;
    }
}