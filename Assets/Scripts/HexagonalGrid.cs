using System;
using UnityEngine;

internal class HexagonalGrid
{
    public Vector3 Origin { get; }

    private int width;
    private int height;
    private HexComponent[,] grid;
    //Hardcoded as the orientation and scale in unity 
    //Need to assume that all the tiles are equally sized
    private float CELL_HEIGHT = 10;
    private float HeightScale = 5;


    public HexagonalGrid(int width, int height, Vector3 origin = new Vector3())
    {
        this.width = width;
        this.height = height;
        Origin = origin;
        grid = new HexComponent[width, height];
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

    internal void DrawGrid()
    {
        var parent = new GameObject("MapGrid");
        parent.transform.position = Origin;

        //Calculate once as we assume all tiles gameobjects are equally sized
        var cellHeight = CELL_HEIGHT;
        var cellWidth = (cellHeight / Mathf.Sqrt(3)) * 2;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                HexComponent gridComponent = grid[x, y];
                bool insettedRow = x % 2 == 0;
                var inset = Convert.ToInt32(insettedRow) * (cellHeight * 0.5f);

                var worldPosition = new Vector3(x * cellWidth * 0.75f, gridComponent.Height * HeightScale, y * cellHeight + inset);
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
