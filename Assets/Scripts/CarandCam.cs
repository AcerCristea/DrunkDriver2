using UnityEngine;

public class CarandCam : MonoBehaviour
{
    public Transform car;       // Reference to the car's transform
    public Transform camera;    // Reference to the camera's transform
    public float maxSwerveAngle = 5.0f; // Max swerve angle for the car
    public float maxRotationDifference = 5.0f; // Max allowed rotation difference for the camera

    private Vector3 cameraInitialOffset;  // The camera's initial offset from the car
    private float initialCarYRotation;    // Initial Y rotation of the car
    private float initialCameraYRotation; // Initial Y rotation of the camera
    private float currentSteeringAngle;   // Tracks the car's total steering angle

    private void Start()
    {
        if (car == null || camera == null)
        {
            Debug.LogError("Car or Camera is not assigned!");
            return;
        }

        // Calculate the initial offset and rotations
        cameraInitialOffset = camera.position - car.position;
        initialCarYRotation = car.rotation.eulerAngles.y;
        initialCameraYRotation = camera.rotation.eulerAngles.y;
        currentSteeringAngle = 0.0f;
    }

    private void Update()
    {
        if (car == null || camera == null) return;

        // 1. Handle car movement and rotation
        HandleCarMovement();

        // 2. Update camera position and rotation relative to the car
        UpdateCamera();
    }

    private void HandleCarMovement()
    {
        // Forward movement
        float carSpeed = 10f;
        if (Input.GetKey(KeyCode.W))
        {
            car.Translate(-Vector3.right * Time.deltaTime * carSpeed);
        }

        // Steering with uncontrollable swerve
        int beersDrunk = car.GetComponent<StateManager>()?.BeersDrunk ?? 0;
        float drunknessFactor = beersDrunk * 0.2f;
        float swerve = (Mathf.PerlinNoise(Time.time * drunknessFactor, 0.0f) - 0.5f) * drunknessFactor;
        float swerveAngle = Mathf.Clamp(swerve * Time.deltaTime * 50f, -maxSwerveAngle, maxSwerveAngle);
        car.Rotate(Vector3.up, swerveAngle);

        // Player steering input
        float baseSteeringSpeed = 0.5f;
        float steeringSpeed = baseSteeringSpeed + (drunknessFactor * 0.5f);

        if (Input.GetKey(KeyCode.A))
        {
            currentSteeringAngle -= steeringSpeed * Time.deltaTime * 50f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            currentSteeringAngle += steeringSpeed * Time.deltaTime * 50f;
        }

        // Clamp the total steering angle
        currentSteeringAngle = Mathf.Clamp(currentSteeringAngle, -45.0f, 45.0f);
        car.rotation = Quaternion.Euler(car.rotation.eulerAngles.x, initialCarYRotation + currentSteeringAngle, car.rotation.eulerAngles.z);
    }

    private void UpdateCamera()
    {
        // Calculate the rotation difference between the car and the camera
        float carYRotationChange = car.rotation.eulerAngles.y - initialCarYRotation;
        float targetCameraYRotation = initialCameraYRotation + carYRotationChange;

        // Clamp the camera rotation to stay within the max rotation difference
        float cameraCurrentYRotation = camera.rotation.eulerAngles.y;
        float rotationDifference = Mathf.DeltaAngle(cameraCurrentYRotation, targetCameraYRotation);
        if (Mathf.Abs(rotationDifference) > maxRotationDifference)
        {
            targetCameraYRotation = cameraCurrentYRotation + Mathf.Clamp(rotationDifference, -maxRotationDifference, maxRotationDifference);
        }

        // Update the camera's position relative to the car
        Vector3 targetCameraPosition = car.position + cameraInitialOffset;
        camera.position = new Vector3(targetCameraPosition.x, cameraInitialOffset.y + car.position.y, targetCameraPosition.z);

        // Update the camera's rotation
        camera.rotation = Quaternion.Euler(camera.rotation.eulerAngles.x, targetCameraYRotation, camera.rotation.eulerAngles.z);
    }
}
