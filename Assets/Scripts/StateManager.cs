using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StateManager : MonoBehaviour
{
    // UI Elements
    public TextMeshProUGUI BeersDrunkText; 
    public TextMeshProUGUI LivesText;

    // Player Stats
    public int BeersDrunk = 0;
    public int Lives = 3;

    // Fog Variables
    [Header("Fog Settings")]
    public float initialFogDensity = 0.01f; // Starting fog density
    public float maxFogDensity = 0.3f;      // Maximum fog density
    public float fogIncreaseRate = 0.01f;   // Increase rate per beer
    public Color fogColor = new Color(1f, 0.65f, 0.2f); // Yellowish-orange fog color

    void Start()
    {
        // Initialize UI
        BeersDrunkText.text = "Beers Drunk: " + BeersDrunk.ToString();
        LivesText.text = "Lives: " + Lives.ToString();

        // Initialize Fog
        RenderSettings.fog = true;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = initialFogDensity;
    }

    void Update()
    {
        // Update UI
        BeersDrunkText.text = "Beers Drunk: " + BeersDrunk.ToString();
        LivesText.text = "Lives: " + Lives.ToString();
    }

    public void AddBeer()
    {
        BeersDrunk++;

        // Adjust fog density dynamically
        float targetDensity = initialFogDensity + (BeersDrunk * fogIncreaseRate);
        RenderSettings.fogDensity = Mathf.Clamp(targetDensity, initialFogDensity, maxFogDensity);
    }

    public void LoseLife()
    {
        if (Lives > 0)
        {
            Lives--;
            LivesText.text = "Lives: " + Lives.ToString();

            // Optional: Check for Game Over
            if (Lives <= 0)
            {
                Debug.Log("Game Over!");
                // Handle game over logic here
            }
        }
    }
}
