using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    //Time since last gravity tick
    float lastFall = 0;


	// Use this for initialization
	void Start ()
    {

        //If block has no valid positions -> game over
	    if (!IsValidGridPos())
        {
            Debug.Log("Game over");
            Destroy(gameObject);
        }    
	}
	


	// Update is called once per frame
    //Here we define blocks movement controls
	void Update ()
    {

        //When LEFT key is pressed, move block left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {

            //Modify blocks position
            transform.position += new Vector3(-1, 0, 0);

            //Check if blocks position is valid
            if (IsValidGridPos())
            {
                UpdateGrid();
            }

            //If blocks position is not valid, then revert
            else
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }


        //When RIGHT key is pressed, move block right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {

            //Modify blocks position
            transform.position += new Vector3(1, 0, 0);

            //Check if blocks postion is valid
            if (IsValidGridPos())
            {
                UpdateGrid();
            }

            //If blocks position is not valid, revert
            else
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }


        //Rotate block when UP key is pressed
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            //Rotate block
            transform.Rotate(0, 0, -90);

            //Check if blocks position valid
            if (IsValidGridPos())
            {
                UpdateGrid();
            }

            else
            {
                transform.Rotate(0, 0, 90);
            }
        }


        //Drop block faster if DOWN key is pressed
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - lastFall >= 0.5) //The lower this number is, the faster the blocks will fall (keep it over 0)
        {
            
            //Move block downwards
            transform.position += new Vector3(0, -1, 0);


            if (IsValidGridPos())
            {
                UpdateGrid();
            }

            else
            {
                transform.position += new Vector3(0, 1, 0);

                //Clear full line of blocks
                Grid.DeleteFullRows();

                //Spawn next block
                FindObjectOfType<Spawner>().SpawnNextBlock();

                //Disable script
                enabled = false;
            }

            lastFall = Time.time;
        }
    }




    //This function makes sure of the child blocks positions
    bool IsValidGridPos()
    {
        foreach(Transform child in transform)
        {
            Vector2 v = Grid.Round(child.position);

            //Is block not inside borders?
            if (!Grid.CoordinateInsideGrid(v))
            {
                return false;
            }

            //Is block in grid cell and not part of the same block group?
            if (Grid.grid[(int)v.x, (int)v.y] != null &&
                Grid.grid[(int)v.x, (int)v.y].parent != transform)
            {
                return false;
            }

        }

        return true;
  
    }



    void UpdateGrid()
    {

        //Remove old children from grid
        for (int y = 0; y < Grid.gridHeight; ++y)
        {
            
            for (int x = 0; x < Grid.gridWidth; ++x)
            {

                if (Grid.grid[x, y] != null)
                {

                    if (Grid.grid[x, y].parent == transform)
                    {

                        Grid.grid[x, y] = null;
                    }
                }
            }
        }

        //Add new children to grid
        foreach (Transform child in transform)
        {
            Vector2 v = Grid.Round(child.position);
            Grid.grid[(int)v.x, (int)v.y] = child;
        }
    }
    
}
