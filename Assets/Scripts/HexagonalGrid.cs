using System;
using UnityEngine;

internal class HexagonalGrid : Map
{
    public Vector3 Origin { get; }

    private int gridWidth;
    private int gridHeight;
    private HexComponent[,] grid;

    private float cellHeight;
    private float cellWidth;


    public HexagonalGrid(int width, int height, Vector2 cellDimensions, Vector3 origin = new Vector3())
    {
        gridWidth = width;
        gridHeight = height;
        Origin = origin;
        grid = new HexComponent[width, height];

        //Use these, under the assumption that all tiles are equally sized
        cellWidth = cellDimensions[0];
        cellHeight = cellDimensions[1];

    }

    internal void AddCell(GameObject tile, int x, int y)
    {
        var gridComponent = new HexComponent { RenderObject = tile };
        grid[x, y] = gridComponent;
    }

    internal void SetHeightOfCell(float height, int x, int y)
    {
        grid[x, y].Height = height;
    }

    public override void Show()
    {
        var parent = new GameObject("MapGrid");
        parent.transform.position = Origin;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                HexComponent gridComponent = grid[x, y];
                bool insettedRow = x % 2 == 0;
                var inset = Convert.ToInt32(insettedRow) * (cellHeight * 0.5f);

                var worldPosition = new Vector3(x * cellWidth * 0.75f, gridComponent.Height, y * cellHeight + inset);
                var obj = UnityEngine.Object.Instantiate(gridComponent.RenderObject, worldPosition, Quaternion.identity);
                obj.name = $"{x}:{y}";

                obj.transform.SetParent(parent.transform);
            }
        }
    }

    private class HexComponent
    {
        public GameObject RenderObject { get; set; }
        public float Height { get; set; }
    }
}
