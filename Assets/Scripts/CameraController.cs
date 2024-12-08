using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform car; // Reference to the car transform

    private Vector3 offset; // Offset to maintain the initial height and angle
    private float initialCarYRotation; // Initial car Y rotation
    private float initialCameraYRotation; // Initial camera Y rotation

    private void Start()
    {
        if (car == null) return;

        // Calculate the initial offset (relative to the car)
        offset = transform.position - car.position;

        // Store the initial Y rotation values
        initialCarYRotation = car.rotation.eulerAngles.y;
        initialCameraYRotation = transform.rotation.eulerAngles.y;
    }

    private void LateUpdate()
    {
        if (car == null) return;

        // Keep the camera at the same offset but align it to the car's X and Z position
        Vector3 targetPosition = new Vector3(car.position.x, transform.position.y, car.position.z) + new Vector3(offset.x, 0, offset.z);
        transform.position = targetPosition;

        // Calculate the change in the car's Y rotation
        float carYRotationChange = car.rotation.eulerAngles.y - initialCarYRotation;

        // Apply the same change to the camera's Y rotation, preserving its initial rotation difference
        float newCameraYRotation = initialCameraYRotation + carYRotationChange;
        
        //if cameraRotation is >= 5 then set it to the initialCameraYRotation
        if (newCameraYRotation >= 5)
        {
            newCameraYRotation = initialCameraYRotation;
        }
        // Update the camera's rotation with the new calculated Y rotation while keeping the original X and Z rotations
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, newCameraYRotation, transform.rotation.eulerAngles.z);
    }
}
