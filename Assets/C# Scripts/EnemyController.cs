using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Rigidbody enemyRB;
    private GameObject player;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        enemyRB = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 TrackerDirection = (player.transform.position - transform.position).normalized;
        enemyRB.AddForce(TrackerDirection * speed);

        if (transform.position.y < -70) {
            Destroy(gameObject);
        }
    }
}
