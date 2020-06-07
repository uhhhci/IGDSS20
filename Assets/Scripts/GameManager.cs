using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject forestTilePrefab;
    public GameObject grassTilePrefab;
    public GameObject mountainTilePrefab;
    public GameObject sandTilePrefab;
    public GameObject stoneTilePrefab;
    public GameObject waterTilePrefab;

    public Texture2D heightMap;

    public float tileDistanceZ;
    public float tileDistanceX;
    public float tileOffsetZOddRow;
    public float tileMaxOffsetY;

    public float money;

    #region Map generation
    private Tile[,] _tileMap; //2D array of all spawned tiles
    #endregion

    #region Buildings
    public GameObject[] _buildingPrefabs; //References to the building prefabs
    public int _selectedBuildingPrefabIndex = 0; //The current index used for choosing a prefab to spawn from the _buildingPrefabs list
    #endregion


    #region Resources
    private Dictionary<ResourceTypes, float> _resourcesInWarehouse = new Dictionary<ResourceTypes, float>(); //Holds a number of stored resources for every ResourceType

    //A representation of _resourcesInWarehouse, broken into individual floats. Only for display in inspector, will be removed and replaced with UI later
    [SerializeField]
    private float _ResourcesInWarehouse_Fish;
    [SerializeField]
    private float _ResourcesInWarehouse_Wood;
    [SerializeField]
    private float _ResourcesInWarehouse_Planks;
    [SerializeField]
    private float _ResourcesInWarehouse_Wool;
    [SerializeField]
    private float _ResourcesInWarehouse_Clothes;
    [SerializeField]
    private float _ResourcesInWarehouse_Potato;
    [SerializeField]
    private float _ResourcesInWarehouse_Schnapps;
    #endregion

    #region Enumerations
    public enum ResourceTypes { None, Fish, Wood, Planks, Wool, Clothes, Potato, Schnapps }; //Enumeration of all available resource types. Can be addressed from other scripts by calling GameManager.ResourceTypes
    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        PopulateResourceDictionary();
		createTileField();
        InvokeRepeating("handleEconomyTick", 60, 60);
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyboardInput();
        UpdateInspectorNumbersForResources();
        handleClickOnTile();
    }
    #endregion

    #region Methods

    void handleEconomyTick()
    {
        money += 100;

        // set tile neighbours
        for (int z = 0; z < _tileMap.GetLength(1); z++)
        {
            for (int x = 0; x < _tileMap.GetLength(0); x++)
            {
                Tile tile = _tileMap[x, z];
                if(tile._building != null)
                {
                    money -= tile._building.upkeepCost;
                }
            }
        }
    }

    private void handleClickOnTile()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Tile tile = null;

                tile = hit.collider.GetComponent<Tile>();
                if (!tile)
                    tile = hit.collider.GetComponentInParent<Tile>();

                if (tile)
                {
                    Debug.Log("Click on Tile: " + tile.name);
                    if(canBuildOntile(_buildingPrefabs[_selectedBuildingPrefabIndex], tile))
                    {
                        //invisible decorations to place building
                        foreach(MeshRenderer decoration in tile.GetComponentsInChildren<MeshRenderer>())
                        {
                            if (decoration.name.Contains("Tile"))
                                continue;
                            decoration.enabled = false;
                        }

                        //place building
                        Vector3 position = tile.transform.position + _buildingPrefabs[_selectedBuildingPrefabIndex].transform.position;
                        GameObject buildingObject = Instantiate(_buildingPrefabs[_selectedBuildingPrefabIndex], position, Quaternion.identity);
                        Building building = buildingObject.GetComponent<Building>();
                        tile._building = building;
                        building.tileBuildOn = tile;
                        building.gameManager = this;

                        money -= building.buildCostMoney;
                    }
                }
                    
            }
        }
    }

    bool canBuildOntile(GameObject buildingPrefab, Tile tile)
    {
        //workarround: instantiate to get attributes and than destroy again...
        GameObject buildingObject = Instantiate(_buildingPrefabs[_selectedBuildingPrefabIndex], new Vector3(0, 0, 0), Quaternion.identity);
        Building building = buildingObject.GetComponent<Building>();

        bool tileTypeCompatible = Array.Exists(building.compatibleTileTypes, type => type == tile._type);
        bool enoughMoney = money >= building.buildCostMoney;
        bool enoughPlanks = _resourcesInWarehouse[ResourceTypes.Planks] >= building.buildCostPlanks;

        Destroy(buildingObject);

        return tileTypeCompatible && enoughMoney && enoughPlanks;
    }

    //Makes the resource dictionary usable by populating the values and keys
    void PopulateResourceDictionary()
    {
        _resourcesInWarehouse.Add(ResourceTypes.None, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Fish, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Wood, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Planks, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Wool, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Clothes, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Potato, 0);
        _resourcesInWarehouse.Add(ResourceTypes.Schnapps, 0);
    }

    //Sets the index for the currently selected building prefab by checking key presses on the numbers 1 to 0
    void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selectedBuildingPrefabIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _selectedBuildingPrefabIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _selectedBuildingPrefabIndex = 2;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _selectedBuildingPrefabIndex = 3;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _selectedBuildingPrefabIndex = 4;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            _selectedBuildingPrefabIndex = 5;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _selectedBuildingPrefabIndex = 6;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _selectedBuildingPrefabIndex = 7;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _selectedBuildingPrefabIndex = 8;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _selectedBuildingPrefabIndex = 9;
        }
    }

    //Updates the visual representation of the resource dictionary in the inspector. Only for debugging
    void UpdateInspectorNumbersForResources()
    {
        _ResourcesInWarehouse_Fish = _resourcesInWarehouse[ResourceTypes.Fish];
        _ResourcesInWarehouse_Wood = _resourcesInWarehouse[ResourceTypes.Wood];
        _ResourcesInWarehouse_Planks = _resourcesInWarehouse[ResourceTypes.Planks];
        _ResourcesInWarehouse_Wool = _resourcesInWarehouse[ResourceTypes.Wool];
        _ResourcesInWarehouse_Clothes = _resourcesInWarehouse[ResourceTypes.Clothes];
        _ResourcesInWarehouse_Potato = _resourcesInWarehouse[ResourceTypes.Potato];
        _ResourcesInWarehouse_Schnapps = _resourcesInWarehouse[ResourceTypes.Schnapps];
    }

    //Checks if there is at least one material for the queried resource type in the warehouse
    public bool HasResourceInWarehoues(ResourceTypes resource)
    {
        return _resourcesInWarehouse[resource] >= 1;
    }

    public void TakeResourceFromWareHouse(ResourceTypes resource)
    {
        _resourcesInWarehouse[resource] -= 1;
    }

    public void PutResourceInWareHouse(ResourceTypes resource, float ammount)
    {
        _resourcesInWarehouse[resource] += ammount;
    }

    //Is called by MouseManager when a tile was clicked
    //Forwards the tile to the method for spawning buildings
    public void TileClicked(int height, int width)
    {
        Tile t = _tileMap[height, width];

        PlaceBuildingOnTile(t);
    }

    //Checks if the currently selected building type can be placed on the given tile and then instantiates an instance of the prefab
    private void PlaceBuildingOnTile(Tile t)
    {
        //if there is building prefab for the number input
        if (_selectedBuildingPrefabIndex < _buildingPrefabs.Length)
        {
            //TODO: check if building can be placed and then istantiate it

        }
    }

    //Returns a list of all neighbors of a given tile
    private List<Tile> FindNeighborsOfTile(Tile t)
    {
        List<Tile> result = new List<Tile>();

        int x = t._coordinateWidth;
        int z = t._coordinateHeight;

        tryAddTileAsNeighbour(t, result, x, z - 1);
        tryAddTileAsNeighbour(t, result, x, z + 1);

        bool isOddRow = (x % 2) != 0;
        if (isOddRow)
        {
            tryAddTileAsNeighbour(t, result, x - 1, z);
            tryAddTileAsNeighbour(t, result, x - 1, z + 1);

            tryAddTileAsNeighbour(t, result, x + 1, z);
            tryAddTileAsNeighbour(t, result, x + 1, z + 1);
        }
        else
        {
            tryAddTileAsNeighbour(t, result, x - 1, z);
            tryAddTileAsNeighbour(t, result, x - 1, z - 1);

            tryAddTileAsNeighbour(t, result, x + 1, z);
            tryAddTileAsNeighbour(t, result, x + 1, z - 1);
        }

        return result;
    }

    private void createTileField()
    {
        int nRowsZ = heightMap.width;
        int nRowsX = heightMap.height;
        _tileMap = new Tile[nRowsX, nRowsZ];

        Vector3[][] tilePositions = generateTileGrid(nRowsZ, nRowsX);

        // spwan tiles
        for (int z = 0; z < nRowsZ; z++)
        {
            for (int x = 0; x < nRowsX; x++)
            {
                Color color = heightMap.GetPixel(z, x);
                GameObject newTile = null;
                if (color.maxColorComponent == 0.0f)
                {
                    newTile = waterTilePrefab;
                }
                else if (color.maxColorComponent <= 0.2f)
                {
                    newTile = sandTilePrefab;
                }
                else if (color.maxColorComponent <= 0.4f)
                {
                    newTile = grassTilePrefab;
                }
                else if (color.maxColorComponent <= 0.6f)
                {
                    newTile = forestTilePrefab;
                }
                else if (color.maxColorComponent <= 0.8f)
                {
                    newTile = stoneTilePrefab;
                }
                else if (color.maxColorComponent <= 1.0f)
                {
                    newTile = mountainTilePrefab;
                }

                Vector3 position = tilePositions[z][x];
                position.y = color.maxColorComponent * tileMaxOffsetY;

                if (newTile)
                {
                    GameObject tileObject = Instantiate(newTile, position, Quaternion.identity);
                    Tile tile = tileObject.GetComponent<Tile>();
                    tile._coordinateWidth = x;
                    tile._coordinateHeight = z;

                    _tileMap[x, z] = tile;
                }

            }
        }

        // set tile neighbours
        for (int z = 0; z < _tileMap.GetLength(1); z++)
        {
            for (int x = 0; x < _tileMap.GetLength(0); x++)
            {
                Tile tile = _tileMap[x, z];
                tile._neighborTiles = FindNeighborsOfTile(tile);
            }
        }
    }


    private void tryAddTileAsNeighbour(Tile tile, List<Tile> tileList, int neighbourX, int neighbourZ)
    {
        if ((neighbourX >= 0) && (neighbourX < _tileMap.GetLength(0)) && (neighbourZ >= 0) && (neighbourZ < _tileMap.GetLength(1)))
            tileList.Add(_tileMap[neighbourX, neighbourZ]);
    }


    private Vector3[][] generateTileGrid(int nRowsZ, int nRowsX)
    {
        Vector3[][] tilePositions = new Vector3[nRowsZ][];

        for (int z = 0; z < nRowsZ; z++)
        {
            tilePositions[z] = new Vector3[nRowsX];

            for (int x = 0; x < nRowsX; x++)
            {
                bool isOddRow = (x % 2) != 0;

                tilePositions[z][x].x = tileDistanceX * x;
                tilePositions[z][x].y = 0;
                tilePositions[z][x].z = tileDistanceZ * z;
                if (isOddRow)
                    tilePositions[z][x].z += tileOffsetZOddRow;

            }
        }

        return tilePositions;
    }
	
    #endregion
}
