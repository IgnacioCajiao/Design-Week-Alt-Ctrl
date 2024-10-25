using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // Add this directive for TextMesh Pro

public class PlaneController : MonoBehaviour
{
    public float initialFlySpeed = 5f;
    public float speedIncreaseAmount = 5f;
    public float yawAmount = 120f;
    public Material[] skyboxes; // Assign skybox materials in the Inspector
    public float skyboxChangeInterval = 30f; // Interval in seconds for changing skybox and increasing speed
    public int playerLives = 3; // Number of lives
    public GameObject explosionPrefab; // Reference to the explosion prefab
    public GameObject collectableExplosionPrefab; // Reference to the collectable explosion prefab
    public TMP_Text scoreText; // Reference to the ScoreText TMP element
    public TMP_Text livesText; // Reference to the LivesText TMP element
    public AudioSource collectableAudioSource; // Reference to the collectable AudioSource
    public AudioSource asteroidAudioSource; // Reference to the asteroid AudioSource

    private float yaw;
    private float elapsedTime = 0f;
    private int currentSkyboxIndex = 0;
    private int score = 0;

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
        if (skyboxes.Length > 0 && (int)(elapsedTime / skyboxChangeInterval) > currentSkyboxIndex)
        {
            currentSkyboxIndex = (int)(elapsedTime / skyboxChangeInterval) % skyboxes.Length;
            RenderSettings.skybox = skyboxes[currentSkyboxIndex];
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            playerLives--;

            // Play asteroid collision sound
            if (asteroidAudioSource != null) asteroidAudioSource.Play();

            // Instantiate explosion effect
            Instantiate(explosionPrefab, collision.transform.position, collision.transform.rotation);
            // Destroy the asteroid
            Destroy(collision.gameObject);

            // Update lives display
            livesText.text = "Health: " + playerLives.ToString();

            if (playerLives <= 0)
            {
                Debug.Log("Game Over");
                SceneManager.LoadScene("You Lose"); // Load the "You Lose" scene
            }
            else
            {
                Debug.Log("Lives left: " + playerLives);
            }
        }
        else if (collision.gameObject.CompareTag("Collectable"))
        {
            // Play collectable collision sound
            if (collectableAudioSource != null) collectableAudioSource.Play();

            // Instantiate explosion effect for collectable
            Instantiate(collectableExplosionPrefab, collision.transform.position, collision.transform.rotation);

            // Destroy the collectable
            Destroy(collision.gameObject);

            // Update score
            score += 1000;
            scoreText.text = "Score: " + score.ToString();

            Debug.Log("Collectable collected! Score: " + score);
        }
    }
}
