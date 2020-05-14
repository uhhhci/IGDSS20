using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour

    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private MouseManager mouseManager;
    [SerializeField] private Texture2D _heightmap;
    [SerializeField] private TileSet _tileSet;
    

    void Start()
    {
        _ = _heightmap ?? throw new ArgumentNullException("Heightmap for map generation has not been set! Set one in the GameManager script");

        var width = _heightmap.width;
        var height = _heightmap.height;
        var grid = new HexagonalGrid(width,height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var color = _heightmap.GetPixel(x, y);
                var grayValue = color.grayscale;
                GameObject tile = GetTileFromColor(color);

                grid.AddCell(tile, x, y);
                grid.SetHeightOfCell(grayValue, x, y);
            }
        }


        cameraManager.cameraBoundaries = new Boundaries(lowerBounds, upperBounds);

        grid.DrawGrid();
    }
}
