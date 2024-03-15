using System;
using Unity.VisualScripting;
using UnityEngine;

public class GetWalkableTiles : MonoBehaviour
{
    MapGrid mapGrid;
    
    void Start()
    {
        // HandelPlayerInput t9der ttne7a mena wndiroha parametre ll getWalkableTilesMethod .
        mapGrid = FindObjectOfType<MapGrid>();
    }

    // // Method to get the walkable tiles for the selected unit 
    // //!!!! we should check if the cell we want to doesn't already contain another unit , in our case , we can put two units on the same cell
    public void getWalkableTilesMethod( Unit unit )
    {
        int startRow = unit.row;
        int startCol = unit.col;
        int moveRange = unit.moveRange;

        //! we should make sure that there is only one instance of the MapGrid in the scene .
        //! we can also pass the MapGrid as a parameter to the getWalkableTiles method 
        

        // Get the current position of the selected unit
        Vector2Int currentPos = new Vector2Int(startRow, startCol);

        for (int row = -moveRange; row <= moveRange; row++)
        {
            for (int col = -moveRange; col <= moveRange; col++)
            {

                // where the unit want go
                int nextRow = currentPos.x + row;
                int nextCol = currentPos.y + col;

                if (nextRow >= 0 && nextRow < MapGrid.Rows && nextCol >= 0 && nextCol < MapGrid.Columns)
                {
                    // If the distance between the current position and the next position is less than or equal to the moveRange of the unit 
                    // and the next position is not highlighted, highlight it .
                    if (MathF.Abs(row) + MathF.Abs(col) <= moveRange)
                    {
                        mapGrid.grid[nextRow, nextCol].Highlight();
                    }
                }
            }
        }
    }


    // n9dro nzido hna get enemy in all possible move cases . tssema hadik get enemt tdirha men ckamel lplayess possible li t9der temchilhom l unit ta3k
    //  (meme t3 l'enemy bch tchof win y9der y'attacker howa ) .



}