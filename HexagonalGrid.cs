using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class HexagonalGrid
{
    private HexagonalComponent[,] _grid;
    private int _height;
    private int _width;

    private readonly float _cellSize;
    private readonly float _cellGap;
    private readonly Vector2 _origin;

    public HexagonalGrid(int width, int height, float cellSize, float cellGap = 0, Vector2 gridOrigin = new Vector2())
    {
        _grid = new HexagonalComponent[width, height];
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _cellGap = cellGap;
        _origin = gridOrigin;
    }

    internal void AddCell(GameObject instantiatedTile, int x, int y)
    {
        CheckPositionInBounds(x, y);
        _grid[x, y] = new HexagonalComponent(0, instantiatedTile,_cellSize);
    }

    internal void SetHeightOfCell(float height, int x, int y)
    {
        CheckPositionInBounds(x, y);
        var component = _grid[x, y];
        component.Height = height;
    }


    private void CheckPositionInBounds(int x, int y)
    {
        var xInRange = x > 0 && x < _width;
        var yInRange = y > 0 && y < _height;

        if (!(xInRange && yInRange))
            throw new ArgumentOutOfRangeException($"Grid position [{x},{y}] is out of bounds");
    }

    internal void DrawGrid()
    {
        //From grid position to world space
        //ASSUME cellsize is the longest edge hence the outer radius
        var outerRadius = _cellSize;
        var innerRadius = (Math.Sqrt(3) / 2) * outerRadius;
        var horizontal_pos_neighbour = x + innerRadius * 2;
        var vertical_pos_neighbour = z + outerRadius * 1.5;
        var offset_every_other_row = x + innerRadius;
        //Cover empty tiles ? 

        throw new NotImplementedException();
    }


    private class HexagonalComponent
    {
        public float Height { get; set; }
        public GameObject TileObject { get; set; }

        public HexagonalComponent(float height, GameObject tileObject, float size) 
        {
            Height = height;
            //multiply by requested size
            TileObject = tileObject;
        }
    }
}
