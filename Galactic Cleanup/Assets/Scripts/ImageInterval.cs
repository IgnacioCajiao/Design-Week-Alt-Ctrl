using UnityEngine;
using UnityEngine.UI;

public class ImageInterval : MonoBehaviour
{
    public float interval = 25f; // Interval in seconds between appearances
    public float displayTime = 5f; // Time in seconds the image stays visible
    private float timer; 
    private Image image; // Reference 

    void Start()
    {
        image = GetComponent<Image>();
        image.enabled = false; // Initially hide the image
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (image.enabled && timer >= displayTime)
        {
            // Hide the image after the display time has elapsed
            image.enabled = false;
            timer = 0f; // Reset the timer for the interval countdown
        }
        else if (!image.enabled && timer >= interval)
        {
            // Show the image after the interval has elapsed
            image.enabled = true;
            timer = 0f; // Reset the timer for the display time countdown
        }
    }
}

