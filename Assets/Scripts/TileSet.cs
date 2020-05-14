using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "ScriptableObjects/TileSet", order = 1)]
public class TileSet : ScriptableObject
{
    public GameObject WaterTile;
    public GameObject SandTile;
    public GameObject GrassTile;
    public GameObject ForestTile;
    public GameObject StoneTile;
    public GameObject MountainTile;
}