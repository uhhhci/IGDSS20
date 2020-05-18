using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject[] _tilePrefabs; //References to the tile prefabs

    public Texture2D _heightMap; //Reference to the height map texture file

    private float _heightFactor = 50; //Multiplier for placement of tiles on the Y-axis

    public Transform _tileHolder; //Reference to the parent object in the hierarchy for all spawned tiles

    public MouseManager _mouseManager; //Reference to the MouseManager.Instance

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
        _mouseManager.InitializeBounds(0, _heightMap.width * 10, 0, _heightMap.height * 8.66f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateMap()
    {
        for (int h = 0; h < _heightMap.height; h++)
        {
            for (int w = 0; w < _heightMap.width; w++)
            {

                Color c = _heightMap.GetPixel(w, h);
                float max = c.maxColorComponent;
                float tileHeight = _heightFactor * max;

                //Determine tile type
                GameObject selectedPrefab;

                if (max == 0.0f)
                {
                    selectedPrefab = _tilePrefabs[0];
                }
                else if (max < 0.2f)
                {
                    selectedPrefab = _tilePrefabs[1];
                }
                else if (max < 0.4f)
                {
                    selectedPrefab = _tilePrefabs[2];
                }
                else if (max < 0.6f)
                {
                    selectedPrefab = _tilePrefabs[3];
                }
                else if (max < 0.8f)
                {
                    selectedPrefab = _tilePrefabs[4];
                }
                else
                {
                    selectedPrefab = _tilePrefabs[5];
                }

                GameObject go = Instantiate(selectedPrefab, _tileHolder);
                go.transform.position = new Vector3(h * 8.66f, tileHeight, w * 10f + (h % 2 == 0 ? 0f : 5f));
            }
        }
    }
}
