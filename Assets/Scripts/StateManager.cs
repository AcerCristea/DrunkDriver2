using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI BeersDrunkText; //
    public int BeersDrunk = 0;

    public TextMeshProUGUI LivesText; //
    public int Lives = 3;

    void Start()
    {
        BeersDrunkText.text = "Beers Drunk: " + BeersDrunk.ToString();

        LivesText.text = "Lives: " + Lives.ToString();
        
    }

    // Update is called once per frame
    void Update()
    {

        BeersDrunkText.text = "Beers Drunk: " + BeersDrunk.ToString();
        LivesText.text = "Lives: " + Lives.ToString();


    }

    public void AddBeer()
    {
        BeersDrunk++;
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
