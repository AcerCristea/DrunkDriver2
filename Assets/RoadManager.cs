using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public GameObject[] roadPrefabs;
    public GameObject car;
    public float roadLength = 12f; // Length of each road segment
    public int numberOfRoads = 5; // Number of road segments to keep in the scene

    private Queue<GameObject> roads = new Queue<GameObject>();
    private Vector3 nextRoadPosition = Vector3.zero;

    void Start()
    {
        // Initialize the road segments
        for (int i = 0; i < numberOfRoads; i++)
        {
            GameObject road = Instantiate(roadPrefabs[Random.Range(0, roadPrefabs.Length)], nextRoadPosition, Quaternion.identity);
            roads.Enqueue(road);
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
            roads.Enqueue(road);
            nextRoadPosition.x += roadLength;
        }
    }
}