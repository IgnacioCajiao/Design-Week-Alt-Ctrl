using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    public float initialFlySpeed = 5f;
    public float speedIncreaseAmount = 5f;
    public float YawAmount = 120f;
    public Material[] skyboxes; // Assign skybox materials in the Inspector
    public float skyboxChangeInterval = 30f; // Interval in seconds for changing skybox and increasing speed

    private float Yaw;
    private float elapsedTime = 0f;
    private int currentSkyboxIndex = 0;

    void Update()
    {
        // Update elapsed time
        elapsedTime += Time.deltaTime;

        // Calculate the current speed based on elapsed time
        float FlySpeed = initialFlySpeed + (speedIncreaseAmount * Mathf.Floor(elapsedTime / skyboxChangeInterval));
        transform.position += transform.forward * FlySpeed * Time.deltaTime;

        // Get input for yaw
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Adjust yaw
        Yaw += horizontalInput * YawAmount * Time.deltaTime;

        // Calculate pitch and roll based on input
        float pitch = Mathf.Lerp(0, 20, Mathf.Abs(verticalInput)) * Mathf.Sign(verticalInput);
        float roll = Mathf.Lerp(0, 30, Mathf.Abs(horizontalInput)) * -Mathf.Sign(horizontalInput);

        // Apply rotations
        transform.localRotation = Quaternion.Euler(Vector3.up * Yaw + Vector3.right * pitch + Vector3.forward * roll);

        // Change skybox and increase speed based on elapsed time
        if (skyboxes.Length > 0 && (int)(elapsedTime / skyboxChangeInterval) > currentSkyboxIndex)
        {
            currentSkyboxIndex = (int)(elapsedTime / skyboxChangeInterval) % skyboxes.Length;
            RenderSettings.skybox = skyboxes[currentSkyboxIndex];
        }
    }
}