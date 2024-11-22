using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StateManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI BeersDrunkText; //
    public int BeersDrunk = 0; 

    void Start()
    {
        BeersDrunkText.text = "Beers Drunk: " + BeersDrunk.ToString();

        
    }

    // Update is called once per frame
    void Update()
    {

        BeersDrunkText.text = "Beers Drunk: " + BeersDrunk.ToString();
        
    }

    public void AddBeer()
    {
        BeersDrunk++;
    }
}
