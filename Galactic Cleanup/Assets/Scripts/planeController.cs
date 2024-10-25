using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlaneController : MonoBehaviour
{
    public float initialFlySpeed = 5f;
    public float speedIncreaseAmount = 5f;
    public float yawAmount = 120f;
    public Material[] skyboxes; // Assign skybox materials in the Inspector
    public float skyboxChangeInterval = 30f; // Interval in seconds for changing skybox and increasing speed
    public int playerLives = 3; // Number of lives
    public GameObject explosionPrefab; 
    public GameObject collectableExplosionPrefab; 
    public TMP_Text scoreText; 
    public TMP_Text livesText; 
    public AudioSource collectableAudioSource; 
    public AudioSource asteroidAudioSource; 

    private float yaw;
    private float elapsedTime = 0f;
    private int currentSkyboxIndex = 0;
    private int score = 0;
    private int skyboxChangeCount = 0; // Counter for the number of skybox changes

    void Start()
    {
        scoreText.text = "Score: " + score.ToString();
        livesText.text = "Health: " + playerLives.ToString();
    }

    void Update()
    {
        // Update elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the current speed based on elapsed time
        float flySpeed = initialFlySpeed + (speedIncreaseAmount * Mathf.Floor(elapsedTime / skyboxChangeInterval));
        transform.position += transform.forward * flySpeed * Time.deltaTime;

        // Get input for yaw
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Adjust yaw
        yaw += horizontalInput * yawAmount * Time.deltaTime;
        // Calculate pitch and roll based on input
        float pitch = Mathf.Lerp(0, 20, Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);
        float roll = Mathf.Lerp(0, 30, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);
        // Apply rotations
        transform.localRotation = Quaternion.Euler(new Vector3(pitch, yaw, roll));

        // Change skybox and increase speed based on elapsed time
        if (skyboxes.Length > 0 && skyboxChangeCount < 2 && (int)(elapsedTime / skyboxChangeInterval) > currentSkyboxIndex)
        {
            currentSkyboxIndex = (int)(elapsedTime / skyboxChangeInterval) % skyboxes.Length;
            RenderSettings.skybox = skyboxes[currentSkyboxIndex];
            skyboxChangeCount++;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerLives--;

            if (asteroidAudioSource != null) asteroidAudioSource.Play();
            Instantiate(explosionPrefab, collision.transform.position, collision.transform.rotation);
            Destroy(collision.gameObject);

            playerLives = Mathf.Max(playerLives, 0);
            livesText.text = "Health: " + playerLives.ToString();

            if (playerLives <= 0)
            {
                PlayerPrefs.SetInt("FinalScore", score); 
                Debug.Log("Game Over");
                SceneManager.LoadScene("You Lose");
            }
            else
            {
                Debug.Log("Lives left: " + playerLives);
            }
        }
        else if (collision.gameObject.CompareTag("Collectable"))
        {
            if (collectableAudioSource != null) collectableAudioSource.Play();
            Instantiate(collectableExplosionPrefab, collision.transform.position, collision.transform.rotation);
            Destroy(collision.gameObject);

            score += 1000;
            scoreText.text = "Score: " + score.ToString();
            Debug.Log("Collectable collected! Score: " + score);
        }
    }
}
