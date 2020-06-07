using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    #region Attributes

    public BuildingTypes buildingType;

    public int upkeepCost; // The money cost per minute
    public int buildCostMoney; // placement money cost
    public int buildCostPlanks; // placement planks cost
    public int resourceGenerationIntervalSeconds; // If operating at 100% efficiency, this is the time in seconds it takes for one production cycle to finish
    public int outputCountPerCycle; //  The number of output resources per generation cycle (for example the Sawmill produces 2 planks at a time)
    public GameManager.ResourceTypes inputResourceType; // A choice for input resource types (0, 1 or 2 types)
    public GameManager.ResourceTypes outputResourceType; // A choice for output resource type 

    public Tile.TileTypes[] compatibleTileTypes; // A restriction on which types of tiles it can be placed on
    public Tile.TileTypes efficiencyTileType; // A choice if its efficiency scales with a specific type of surrounding tile
    public int maxNumEfficiencyNeighbours;  // The minimum and maximum number of surrounding tiles its efficiency scales with (0-6)

    #endregion

    public GameManager gameManager;
    public Tile tileBuildOn; // Reference to the tile it is built on
    public float efficiency; // Calculated based on the surrounding tile types
    System.DateTime cycleStartTime;

    #region Enumerations
    public enum BuildingTypes {Fishery, LumberJack };
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        cycleStartTime = System.DateTime.UtcNow;

        efficiency = calcEfficiency();
        
        if(efficiency > 0)
            resourceGenerationIntervalSeconds = Convert.ToInt32 ((double)resourceGenerationIntervalSeconds * (1.0 / efficiency));
    }

    private float calcEfficiency()
    {
        if(efficiencyTileType == Tile.TileTypes.Empty)
            return 1;

        int nEfficiencyNeighbours = 0;
        foreach(Tile neighbourTile in tileBuildOn._neighborTiles)
        {
            if (neighbourTile._type == efficiencyTileType)
                nEfficiencyNeighbours++;
        }

        if (nEfficiencyNeighbours == 0)
            return 0;

        if (nEfficiencyNeighbours >= maxNumEfficiencyNeighbours)
            return 1;

        return (float)nEfficiencyNeighbours / (float)maxNumEfficiencyNeighbours;
    }

    // Update is called once per frame
    void Update()
    {
        if(startNewCycle())
        {
            processResources();
        }
    }


    private void processResources()
    {
        if(getInputResources())
        {
            gameManager.PutResourceInWareHouse(outputResourceType, outputCountPerCycle);
        }
        
    }

    private bool getInputResources()
    {
        if(inputResourceType == GameManager.ResourceTypes.None)
        {
            return true;
        }

        if(gameManager.HasResourceInWarehoues(inputResourceType))
        {
            gameManager.TakeResourceFromWareHouse(inputResourceType);
            return true;
        }

        return false;
    }


    private bool startNewCycle()
    {
        if (efficiency <= 0)
            return false;

        System.TimeSpan timeSpan = System.DateTime.UtcNow - cycleStartTime;
        int secondsSinceLastCycleStart = timeSpan.Seconds;
        if (secondsSinceLastCycleStart >= resourceGenerationIntervalSeconds)
        {
            cycleStartTime = System.DateTime.UtcNow;
            return true;
        }

        return false;
    }
}
