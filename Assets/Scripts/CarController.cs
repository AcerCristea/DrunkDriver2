using UnityEngine;

public class CarController : MonoBehaviour
{
    private float carSpeed = 10f;
    private float baseSteeringSpeed = 0.5f;
    public StateManager stateManager;

    public float minRotation = 181.957f; // Minimum allowed Y rotation (A key limit)
    public float maxRotation = 188.616f; // Maximum allowed Y rotation (D key limit)

    private float currentSwerveAngle = 0.0f; // Tracks current swerve angle

    void Start()
    {
        if (stateManager == null)
        {
            stateManager = FindObjectOfType<StateManager>();
        }
    }

    void Update()
    {
        if (stateManager == null) return;

        int beersDrunk = stateManager.BeersDrunk;

        // Drunkness increases random swerve
        float drunknessFactor = beersDrunk * 0.2f;

        // Perlin noise for smooth random motion (swerve)
        float swerve = (Mathf.PerlinNoise(Time.time * drunknessFactor, 0.0f) - 0.5f) * drunknessFactor;

        // Clamp the swerve angle
        currentSwerveAngle = Mathf.Clamp(swerve * Time.deltaTime * 50f, -drunknessFactor, drunknessFactor); // Keep swerve subtle

        // Calculate steering input
        float steeringSpeed = baseSteeringSpeed + (drunknessFactor * 0.5f);
        float steeringInput = 0.0f;

        if (Input.GetKey(KeyCode.A))
        {
            steeringInput = -steeringSpeed * Time.deltaTime * 90f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            steeringInput = steeringSpeed * Time.deltaTime * 90f;
        }

        // Combine swerve and steering input
        float targetRotation = transform.rotation.eulerAngles.y + steeringInput + currentSwerveAngle;

        // Normalize angles to avoid wrap-around issues
        targetRotation = NormalizeAngle(targetRotation);
        float clampedRotation = Mathf.Clamp(targetRotation, minRotation, maxRotation);

        // Apply the clamped rotation
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, clampedRotation, transform.rotation.eulerAngles.z);

        // Forward movement
        transform.Translate(-Vector3.right * Time.deltaTime * carSpeed);

    }

    // Helper function to normalize angles to the range [0, 360)



    private float NormalizeAngle(float angle)
    {
        while (angle < 0) angle += 360;
        while (angle >= 360) angle -= 360;
        return angle;
        
    }

    //if the car collides with the obstacle, the player loses a life
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collision with obstacle!");  
            stateManager.LoseLife();
        }
    }
}
