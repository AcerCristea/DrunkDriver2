using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public GameObject[] roadPrefabs; // Array of road prefabs
    public GameObject[] obstaclePrefabs; // Array of obstacle prefabs
    public GameObject car; // Reference to the car
    public float roadLength = 12f; // Length of each road segment
    public int numberOfRoads = 5; // Number of road segments to keep in the scene
    public int obstaclesPerRoad = 1; // Number of obstacles per road segment
    public float obstacleSpawnRangeY = 1f; // Y range to spawn obstacles within the road

    private Queue<GameObject> roads = new Queue<GameObject>();
    private Vector3 nextRoadPosition = Vector3.zero;

    void Start()
    {
        // Initialize the road segments
        for (int i = 0; i < numberOfRoads; i++)
        {
            GameObject road = Instantiate(roadPrefabs[Random.Range(0, roadPrefabs.Length)], nextRoadPosition, Quaternion.identity);
            roads.Enqueue(road);
            SpawnObstacles(road); // Spawn obstacles on the road segment
            nextRoadPosition.x += roadLength;
        }
    }

    void Update()
    {
        // Check if the car has moved off the first road segment
        if (car.transform.position.x > roads.Peek().transform.position.x + roadLength)
        {
            // Move the road segment to the front
            GameObject road = roads.Dequeue();
            road.transform.position = nextRoadPosition;

            // Clear existing obstacles and spawn new ones
            ClearObstacles(road);
            SpawnObstacles(road);

            roads.Enqueue(road);
            nextRoadPosition.x += roadLength;
        }
    }

    // Spawns obstacles on a road segment
private void SpawnObstacles(GameObject road)
{
    int obstaclesToSpawn = Mathf.Min(2, obstaclesPerRoad); // Limit to 2 obstacles per road
    bool pathCleared = false; // Ensure at least one clear path exists

    for (int i = 0; i < obstaclesToSpawn; i++)
    {
        // Ensure at least one valid path by skipping one spawn if a path hasn't been cleared
        if (!pathCleared && i == obstaclesToSpawn - 1)
        {
            pathCleared = true;
            continue; // Skip this spawn to leave a clear path
        }

        // Randomize the position of the obstacle
        float spawnPosX = road.transform.position.x + Random.Range(-roadLength / 2f, roadLength / 2f);
        float spawnPosZ = road.transform.position.z + Random.Range(-obstacleSpawnRangeY, obstacleSpawnRangeY);

        // Position the obstacle
        Vector3 obstaclePosition = new Vector3(spawnPosX, 7.5f, spawnPosZ);

        // Instantiate the obstacle as a child of the road
        GameObject obstacle = Instantiate(
            obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)],
            obstaclePosition,
            Quaternion.identity,
            road.transform // Make this obstacle a child of the road
        );

        //set obstacle Tag
        obstacle.tag = "Obstacle";

        // Scale the obstacle to match the player's car size
        obstacle.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        SetRandomColorRecursive(obstacle.transform); // Randomize the color of the obstacle

        // Randomize the color of the obstacle
        Renderer renderer = obstacle.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = GetRandomColor();
        }

        // Destroy the obstacle after 5 seconds
        Destroy(obstacle, 3.5f);
    }
}
private void ClearObstacles(GameObject road)
{
    // Destroy all child objects of the road that are obstacles
    foreach (Transform child in road.transform)
    {
        if (child.CompareTag("Obstacle")) // Ensure your obstacle prefabs are tagged as "Obstacle"
        {
            Destroy(child.gameObject); // Destroy the obstacle game object
        }
    }
}

// Generates a random color
private Color GetRandomColor()
{
    return new Color(Random.value, Random.value, Random.value); // RGB values between 0 and 1
}

private void SetRandomColorRecursive(Transform obj)
{
    Renderer renderer = obj.GetComponent<Renderer>();
    if (renderer != null)
    {
        renderer.material.color = GetRandomColor();
    }

    foreach (Transform child in obj)
    {
        SetRandomColorRecursive(child);
    }
}



    // Clears all obstacles from a road segment
    // private void ClearObstacles(GameObject road)
    // {
    //     // Destroy all child objects of the road that are obstacles
    //     foreach (Transform child in road.transform)
    //     {
    //         if (child.CompareTag("Obstacle")) // Ensure your obstacle prefabs are tagged as "Obstacle"
    //         {
    //             Destroy(child.gameObject);
    //         }
    //     }
    // }
}
