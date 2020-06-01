using System.Collections.Generic;
using UnityEngine;
using System;


public class Building : MonoBehaviour
{
    private TileSet _tileSet; 
    // public enum TileTypeEnum { WaterTile, SandTile, GrassTile, ForestTile, StoneTile, MountainTile  };
    // public enum BuildingTypeEnum { Fishery, Lumberjack, Sawmill, SheepFarm, FrameworkKnitters, PotatoFarm, SchnappsDistillery };

    //public enum ResourcesTypeEnum { Fish, Wood, Plank, Wool, Cloth, Potato, Schnapps};

    public BuildingType buildingType;
    public int upkeep = 0;
    public int buildCost_Money, buildCost_Plank;

    public GameObject tile;
    public TileType efficientNeighboringTile;
    public List<TileSet> goodNeighbor;

    // has to be limited to 0-6
    public int minEfficientNeigbor, maxEfficientNeighbor;
    public RangeInt minAndMax;

    // TODO this list hast to contain 0-2 items --> limit size
    public List<ResourcesType> inputResources;
    public ResourcesType outputResources;
    public float resourceGenerationInterval;
    public int outputCount;





    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // TODO manage production cycle
        // use efficiency
        // GameManager.removeResource(type, 1)
        // GameMananger.WinResource(type, int count)
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
        // TODO: or however you saved the neighbors.

        // count 'efficient' neighbors
        int counter = 0;
        float efficiency;
        neighbors.ForEach(delegate (GameObject it)
        {
            if (it.name == efficientNeighboringTile.ToString()) counter++;
        });

        // not efficient/ min max not right
        if (counter == 0 || (maxEfficientNeighbor < minEfficientNeigbor)) return 0;

        // clamp calculated efficiency into range [0,1]
        efficiency = (float) counter / (1 + maxEfficientNeighbor - minEfficientNeigbor);
        return Mathf.Clamp(efficiency, 0, 1);
    }


    





}
