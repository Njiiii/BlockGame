using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {

    public static int gridWidth = 10;
    public static int gridHeight = 20;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];


    //Round is helper function to round up vectors
    //For example (1.0000001, 2) -> (1, 2)
    public static Vector2 Round(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), 
                           Mathf.Round(v.y));
    }


    //This function helps us to find if a coordinate is inside the grid or no
    public static bool CoordinateInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 &&
                (int)pos.x < gridWidth &&
                (int)pos.y >= 0);
    }


    //Delete a row of blocks
    public static void DeleteRow(int y)
    {
        for(int x = 0; x < gridWidth; ++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }


    //If some rows are deleted, the rows above will be dropped down 
    public static void DropRows(int y)
    {

        for(int x = 0; x < gridWidth; ++x)
        {

            if (grid[x, y] != null)
            {

                //Move one unit downwards
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;

                //Update blocks position
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    
    //Now we use DropRows() to on every row above a certain index, because we want to drop all rows above deleted row, not just one
    public static void DropAllRowAbowe(int y)
    {
        for(int i = y; i < gridHeight; ++i)
        {
            DropRows(i);
        }
    }

    
    //Here we check if a row is full of blocks
    public static bool IsRowFull(int y)
    {

        for(int x = 0; x  < gridWidth; ++x)
        {

            if (grid[x, y] == null)
            {

                return false;
            }
        }

        return true;
    }



    //If one or more rows full, delete rows
    public static void DeleteFullRows()
    {

        for(int y = 0; y < gridHeight; ++y)
        {

            if (IsRowFull(y))
            {
                DeleteRow(y);
                DropAllRowAbowe(y + 1);
                --y;
            }
        }
    }





}
