using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private MouseManager mouseManager;
    [SerializeField] private Texture2D _heightmap;
    [SerializeField] private MapManager _mapManager;
    [SerializeField] private TileSet _tileSet;
    [Tooltip("Allow the camera to move past the map's boundaries in relation to the camera angle")]
    [SerializeField] private bool AllowCameraAngleToInfluenceBoundaries;

    private int moneyPool;

    void Start()
    {
        _ = _heightmap ?? throw new ArgumentNullException("Heightmap for map generation has not been set! Set one in the GameManager script");


        var base_map = _mapManager.FromHeightmap(_heightmap, _tileSet, 5);
        _mapManager.DrawMap(base_map);

        var bounds = base_map.GetBoundaries();

        if (AllowCameraAngleToInfluenceBoundaries)
            bounds.OffsetBoundaries(cameraManager.CalculateOffsetFromCameraAngle());

        cameraManager.cameraBoundaries = bounds;

    }


    void Update()
    {
        // Every 60 sec 
        // tickEconomy()
    }


    private void TickEconomy()
    {
        // constant income
        moneyPool += 100;
        // pay upkeep costs
        getAllUpkeepCost();
    }


    private int getAllUpkeepCost()
    {
        // TODO
        return 10;
    }

    // Add resource of given type to warehouse
    public void winResource(ResourcesType res, int count)
    {
        // TODO
    }

    // remove resource of given type to warehouse
    public void removeResource(ResourcesType res, int count)
    {
        // TODO
    }
}
