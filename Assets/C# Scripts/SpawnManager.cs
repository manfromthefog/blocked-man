using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    public PlayerController playerControllerScript;
    public Button RespawnButton;
    public GameObject[] PowerUps;
    private Vector3[] powerUpPositions;

    void OnRespawn() {
        for (int i = 0; i < PowerUps.Length; i++) {
            Rigidbody PowerUpRB;
            PowerUps[i].transform.position = powerUpPositions[i];
            PowerUpRB = PowerUps[i].GetComponent<Rigidbody>();
            PowerUps[i].SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        PowerUps = GameObject.FindGameObjectsWithTag("PowerUp");

        Button b = RespawnButton.GetComponent<Button>();
        b.onClick.AddListener(OnRespawn);

        powerUpPositions = new Vector3[PowerUps.Length];
        for (int i = 0; i < PowerUps.Length; i++) {
            powerUpPositions[i] = PowerUps[i].transform.position;  
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
