using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    #region Manager References
    public static NavigationManager Instance; //Singleton of this manager. Can be called with static reference NavigationManager.Instance
    GameManager _gameManager; //Reference to GameManager.Instance
    #endregion

    int _mapDimensionX;
    int _mapDimensionY;

    //Awake is called when creating this object
    private void Awake()
    {
        if (Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDimensions(int x, int y)
    {
        _mapDimensionX = x;
        _mapDimensionY = y;
    }

    public Map GenerateMap(Building b)
    {
        Map map = new Map(_mapDimensionX, _mapDimensionY);
        map.SetValue(b._tile._coordinateWidth, b._tile._coordinateHeight, 0);

        Tile[,] tileMap = _gameManager._tileMap;

        List<Tile> currentlyVisiting = new List<Tile>();

        currentlyVisiting.Add(b._tile);


        while (currentlyVisiting.Count > 0)
        {
            Tile t = currentlyVisiting[0];
            List<Tile> neighbors = t._neighborTiles;
            int currentWeight = map.GetValue(t._coordinateWidth, t._coordinateHeight);

            foreach (Tile neighbor in neighbors)
            {
                if (map.GetValue(neighbor._coordinateWidth, neighbor._coordinateHeight) == 1000000)
                //if (map.GetValue(neighbor._coordinateWidth, neighbor._coordinateHeight) > currentWeight + neighbor._navigationWeight)
                {
                    map.SetValue(neighbor._coordinateWidth, neighbor._coordinateHeight, currentWeight + neighbor._navigationWeight);
                    currentlyVisiting.Add(neighbor);

                }

            }

            currentlyVisiting.RemoveAt(0);
        }


        return map;
    }


    //A wrapper class for a 2D array of weight values
    public class Map
    {
        private int[,] _valueArray; //The 2D array holding all the weight values for each tile
        public int _dimensionX;
        public int _dimensionY;

        public Map(int x, int y)
        {
            _dimensionX = x;
            _dimensionY = y;

            _valueArray = new int[_dimensionX, _dimensionY];

            //Initialize array with a high number for each tile
            for (int i = 0; i < _dimensionX; i++)
            {
                for (int j = 0; j < _dimensionY; j++)
                {
                    _valueArray[i, j] = 1000000;
                }
            }
        }

        public int GetValue(int x, int y)
        {
            return _valueArray[x, y];
        }

        public void SetValue(int x, int y, int value)
        {
            _valueArray[x, y] = value;
        }
    }
}
