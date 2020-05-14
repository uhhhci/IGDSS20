using System;
using UnityEngine;

internal class HexagonalGrid
{
    private int width;
    private int height;

    public HexagonalGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    internal void AddCell(GameObject instantiatedTile, int x, int y)
    {
        throw new NotImplementedException();
    }

    internal void SetHeightOfCell(float grayValue, int x, int y)
    {
        throw new NotImplementedException();
    }

    internal void DrawGrid()
    {
        throw new NotImplementedException();
    }
}