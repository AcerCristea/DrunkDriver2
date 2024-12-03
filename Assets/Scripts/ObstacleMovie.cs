using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    public float speed = 5f;               // Speed of obstacle movement
    public float resetDistanceAhead = 50f; // Distance ahead of the player where obstacles reset
    private float yPosition;               // Initial y-axis position

    private Transform playerTransform;     // Reference to the player's Transform

    void Start()
    {
        // Store the initial y position to maintain it during movement
        yPosition = transform.position.y;

        // Find the player by tag and get its Transform
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found. Please tag your car GameObject with 'Player'.");
        }
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        // Move the obstacle along the negative x-axis (towards the player)
        Vector3 movement = Vector3.left * speed * Time.deltaTime;
        transform.position += movement;

        // Keep the obstacle at the same y position
        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);

        // Check if the obstacle has passed behind the player
        if (transform.position.x < playerTransform.position.x - 5f)
        {
            // Reset obstacle ahead of the player
            float resetX = playerTransform.position.x + resetDistanceAhead;

            // Randomize z-position within road boundaries
            float minZ = -5f; // Adjust according to your road width
            float maxZ = 5f;
            float randomZ = Random.Range(minZ, maxZ);

            transform.position = new Vector3(resetX, yPosition, randomZ);
        }
    }
}
