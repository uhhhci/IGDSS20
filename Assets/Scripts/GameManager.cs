using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private MouseManager mouseManager;
    [SerializeField] private Texture2D _heightmap;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private TileSet _tileSet;


    void Start()
    {
        _ = _heightmap ?? throw new ArgumentNullException("Heightmap for map generation has not been set! Set one in the GameManager script");

        cameraManager.cameraBoundaries = new Boundaries(lowerBounds, upperBounds);
        
        var base_map = _mapManager.FromHeightmap(_heightmap, _tileSet, 5);
        _mapManager.DrawMap(base_map);
    }
}
