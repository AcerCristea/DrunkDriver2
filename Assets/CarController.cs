using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    // Start is called before the first frame update
    private int carSpeed = 10;
    private float baseSteeringSpeed = 0.5f;
    public StateManager stateManager;

    void Start()
    {
        if (stateManager == null)
        {
            stateManager = FindObjectOfType<StateManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        int beersDrunk = stateManager.BeersDrunk;

        float drunknessFactor = beersDrunk * 0.2f;

        float swerve = Mathf.PerlinNoise(Time.time * drunknessFactor, 0.0f) - 0.5f;
        swerve *= drunknessFactor;

        float steeringSpeed = baseSteeringSpeed + (drunknessFactor * 0.5f);

        // Forward movement
        transform.position += transform.forward * Time.deltaTime * carSpeed;


        // Steering with uncontrollable swerve
        float swerveAmount = swerve * Time.deltaTime * 50f;
        transform.Rotate(Vector3.up, swerveAmount);

        // Steering input
        float steeringInput = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            steeringInput = -steeringSpeed * Time.deltaTime * 50f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            steeringInput = steeringSpeed * Time.deltaTime * 50f;
        }
        transform.Rotate(Vector3.up, steeringInput);

        // Optional: Add delayed response
        // Implement input lag based on drunkness
        // You can queue inputs and process them after a delay proportional to drunkness
    }
}
