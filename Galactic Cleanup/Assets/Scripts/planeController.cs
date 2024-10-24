using UnityEngine;
using UnityEngine.SceneManagement;

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

    private float yaw;
    private float elapsedTime = 0f;
    private int currentSkyboxIndex = 0;

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

            // Instantiate explosion effect
            Instantiate(explosionPrefab, collision.transform.position, collision.transform.rotation);
            // Destroy the asteroid
            Destroy(collision.gameObject);

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
            // Instantiate explosion effect for collectable
            Instantiate(collectableExplosionPrefab, collision.transform.position, collision.transform.rotation);

            // Destroy the collectable
            Destroy(collision.gameObject);

            Debug.Log("Collectable collected!");
        }
    }
}
