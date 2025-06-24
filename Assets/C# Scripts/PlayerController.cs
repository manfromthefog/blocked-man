using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private AudioSource playerAudio;
    public EnemyController EnemyControllerScript;
    public ParticleSystem implosionParticle;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI lifeText;
    public Button restartButton;

    public GameObject powerUpIndicator; 
    public GameObject spawnPoint; 
    public AudioClip explosionSound;
    public GameObject[] Obstacles;
    public Vector3[] obstaclePosition;

    [SerializeField] private int deathCounter = 0;
    [SerializeField] private int life = 3;
    private int impactPower = 5800;
    public float speed = 5.0f;
    public float rotationSpeed = 25.0f;

    private bool isAdmin = false;
    public bool invincibilityAndSpeed = false;
    public bool gameOver = false;

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }
    public void RestartGame()
    {
        // Allow game activity to occur
        gameOver = false;
        powerUpIndicator.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        life = 3;
        lifeText.text = "Lives: " + life;

        // Resetting player velocity, position, and momentum
        speed = 50;
        rotationSpeed = 75;
        playerRb.transform.position = spawnPoint.transform.position;
        playerRb.velocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        // Resetting obstacle rotation
        foreach (GameObject Obstacle in Obstacles) {
            Obstacle.transform.eulerAngles = new Vector3(0, 270, 0);
        }
        // Resetting obstacle position
        for (int i = 0; i < Obstacles.Length; i++) {
            Rigidbody ObstacleRb;
            Obstacles[i].transform.position = obstaclePosition[i];
            ObstacleRb = Obstacles[i].GetComponent<Rigidbody>();

            ObstacleRb.velocity = Vector3.zero;
            ObstacleRb.angularVelocity = Vector3.zero;
        }
    }

    private IEnumerator PowerupCountdownRoutine() {
        yield return new WaitForSeconds(6);
        invincibilityAndSpeed = false;
        powerUpIndicator.gameObject.SetActive(false);
    }
    // Creates the "Withered Impact" Ability
    public void WitheredImpact() {
        implosionParticle.Play();
        playerAudio.PlayOneShot(explosionSound, 0.05f);
        
        // Get that object's rigidbody
        for (int i = 0; i < Obstacles.Length; i++) {
            Rigidbody ObstacleRb;
            ObstacleRb = Obstacles[i].GetComponent<Rigidbody>();
            
            // For every object in a 5 unit radius affected by explosion
            ObstacleRb.AddExplosionForce(impactPower, playerRb.transform.position, 15, 10.0F);
        }

        invincibilityAndSpeed = true;
        StartCoroutine(PowerupCountdownRoutine());
        if (invincibilityAndSpeed) {
            speed = 100;
            rotationSpeed = 125;
        }
    }

    void lifeScore(int decreaseLife) {
        life -= decreaseLife;
        lifeText.text = "Lives: " + life;
    }

    // Detects collision decisions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && !invincibilityAndSpeed)
        {
            if (gameOver == false)
            {
                lifeScore(1);
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PowerUp") && !gameOver) {
            WitheredImpact();
            powerUpIndicator.gameObject.SetActive(true);
            other.gameObject.SetActive(false);
            // Debug.Log("An ancient power is evoked as the ground trembles under the force of Wither Impact!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        Button b = restartButton.GetComponent<Button>();
        Obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        b.onClick.AddListener(RestartGame);
        obstaclePosition = new Vector3[Obstacles.Length];
        for (int i = 0; i < Obstacles.Length; i++) {
            obstaclePosition[i] = Obstacles[i].transform.position;  
        }
        /**
         *  Withered Impact: Create a special distortion around you; 
         *  Then implode, knocking away all obstacles then apply the 
         *  Wither Shield ability granting invincibility and additional 
         *  speed for 5 seconds.
        **/
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameOver == false) 
        {
            if (!invincibilityAndSpeed) {
                speed = 50;
                rotationSpeed = 65;
            }
            if (transform.position.y < -20) {
                deathCounter++;
                life = 0;
                lifeText.text = "Lives: " + life;
                gameOver = true;
                GameOver();
            }
            float forwardInput = Input.GetAxis("Vertical");
            float rotationInput = Input.GetAxis("Horizontal");

            playerRb.AddForce(Vector3.forward * speed * forwardInput);
            playerRb.AddForce(Vector3.right * rotationSpeed * rotationInput);

            powerUpIndicator.transform.position = transform.position;

            if (isAdmin) {
                if (Input.GetKey(KeyCode.F)) {
                    WitheredImpact();
                }
            }
            if (life <= 0) {
                deathCounter++;
                gameOver = true;
                GameOver();
            }
        }
    }
}
