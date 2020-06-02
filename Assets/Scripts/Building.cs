using System.Collections.Generic;
using UnityEngine;
using System;


public class Building : MonoBehaviour
{
    [Tooltip("Type of Building")]
    public BuildingType buildingType;
    [Tooltip("Money cost per economy cycle")]
    public int upkeep = 0;
    [Tooltip("Costs in money/ planks to build building.")]
    public int buildCost_Money, buildCost_Plank;

    // After merge, type should be Tile
    [Tooltip("The tile where building is built on. ")]
    public GameObject tile;
    [Tooltip("Type of neighboring tile that boosts efficiency of production. ")]
    public TileType efficientNeighboringTile;

    [Tooltip("min/ max count of efficiency pushing neighbars that are used for calculations.")]
    [Range (0,6)]
    public int minEfficientNeigbor, maxEfficientNeighbor;

    // this list hast to contain 0-2 items --> limit size
    [Tooltip("0 - 2 resource types that are production input")]
    public List<ResourcesType> inputResources;
    [Tooltip("That's what the building produces.")]
    public ResourcesType outputResource;
    [Tooltip("How long is a production cycle in sec.")]
    public float resourceGenerationInterval;
    [Tooltip("How many resources are produced in a cycle. ")]
    public int outputCount;


    private float generationState;
    private float effectiveGenerationTime;
    private bool productionRunning;


    // Start is called before the first frame update
    void Start()
    {
        //effectiveGenerationTime = (float) (2 - Efficiency()) * resourceGenerationInterval;
        effectiveGenerationTime = (2 - 0.5f) * resourceGenerationInterval;
    }

    // Update is called once per frame
    void Update()
    {      

        if (productionRunning)
        {
            // working
            generationState += Time.deltaTime;

            // finished cycle
            if (generationState >= effectiveGenerationTime)
            {
                // create resource and reset cycle
                GameManager.winResource(outputResource, outputCount);
                generationState -= effectiveGenerationTime;    
                productionRunning = false;

                // restart if possible
                tryToStartProduction();
            }
        }
        else
        {
            // try to start production            
            tryToStartProduction();
        }
    }

    /* 
     * checks if production can be started. 
     * If start is possible, the input res are taken from warehouse. 
     */ 
    private void tryToStartProduction()
    {
        if (CanProductionStart())
        {
            // start production
            productionRunning = true;
            inputResources.ForEach(delegate (ResourcesType it)
            {
                GameManager.removeResource(it, 1);
            });
        }
        else
        {
            // Production start failed aka GameManager said no. 
            productionRunning = false;
        }
    }

    /* 
     * calculates efficiency of the building respective to neighboring tiles 
     * and specific min/ max needed number of a specific tile type. 
     */
    float Efficiency()
    {
        // save neighbors. 
        List<GameObject> neighbors = new List<GameObject>();
        


        // neighbors = tile.getNeighboringTiles();
        // TODO: or however you saved the neighbors. + adapt next if
        // 




        // count 'efficient' neighbors
        int counter = 0;
        float efficiency;
        neighbors.ForEach(delegate (GameObject it)
        {

            // TODO

            // check if item equals the efficient neighbor
            if (it.name == efficientNeighboringTile.ToString()) counter++;
        });

        // not efficient/ min max not right
        if (counter == 0 || (maxEfficientNeighbor < minEfficientNeigbor)) return 0;

        // clamp calculated efficiency into range [0,1]
        efficiency = (float) counter / (1 + maxEfficientNeighbor - minEfficientNeigbor);
        return Mathf.Clamp(efficiency, 0, 1);
    }

    /* asks GameManager if input resources are available --> production is startable
     */ 
    private bool CanProductionStart()
    {
        bool val = true; 
        inputResources.ForEach(delegate (ResourcesType it)
        {
            if (!GameManager.checkAvailability(it, 1))
            {
                val = false;
            }
        });
        return val;
    }





}
