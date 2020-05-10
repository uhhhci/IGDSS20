using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        createTileField();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void createTileField()
    {
        int nRowsZ = heightMap.width;
        int nRowsX = heightMap.height;

        Vector3[][] tilePositions = generateTileGrid(nRowsZ, nRowsX);

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
                    Instantiate(newTile, position, Quaternion.identity);
            }
        }
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
}
