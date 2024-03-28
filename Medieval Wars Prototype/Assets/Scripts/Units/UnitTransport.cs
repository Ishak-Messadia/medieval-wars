using System.Collections.Generic;
using UnityEngine;

public class UnitTransport : Unit
{

    public Unit loadedUnit;
    public List<GridCell> dropableCells = new List<GridCell>();  // cells where the transporter can drop the loaded unit .
    public List<Unit> suppliableUnits = new List<Unit>();  // unit that can get supplyRation from the transporter .


    public float AvailableRationToShare; //!!! ch7al rahi rafda ration , bach tmed ll units lo5rin 

    // lazemna valuere max t3 AvailableRationToShare

    // Method to load a unit into the transporterUnit
    public void Load(Unit unit)
    {
        loadedUnit = unit;
        unit.unitView.HideUnitWhenLoaded();
    }

    // Method to drop a unit onto a grid cell
    public void Drop(GridCell cell)
    {
        //!!!!! remove the hide from the unit .

        // ... logic to drop the unit into the grid cell

        cell.occupantUnit = this.loadedUnit;
        this.loadedUnit.occupiedCell = cell;
        this.loadedUnit.row = cell.row;
        this.loadedUnit.col = cell.column;

        // set the new position of the unit .
        this.loadedUnit.unitView.SetUnitPosition(cell.row , cell.column);
        this.loadedUnit.unitView.ShowUnitAfterDrop();
        
        loadedUnit = null;

    }

    // Method to supply a unit with something
    public void Supply(Unit unitToSupply, float supplyAmount)
    {
        //!!!! supplier 3lach ??? kanet parametre doka n7itha .
        // transporter howa selected unit fl Unitcontroller , omb3d UnitToSupply hya li tselectionniha omb3d (mor l7kaya t3 layer wg3) 
        AvailableRationToShare -= supplyAmount;
        unitToSupply.RecievRationSupply(supplyAmount);
    }



    // method to get teh dropable units
    public void GetdropableCells()
    {

        int currentRow = row;
        int currentCol = col;

        List<GridCell> dropableCellsCondidates = new List<GridCell>();

        if (currentRow - 1 >= 0) dropableCellsCondidates.Add(mapGrid.grid[currentRow - 1, currentCol]);   // top cell
        if (currentRow + 1 < mapGrid.grid.GetLength(0)) dropableCellsCondidates.Add(mapGrid.grid[currentRow + 1, currentCol]);   // bottom cell
        if (currentCol - 1 >= 0) dropableCellsCondidates.Add(mapGrid.grid[currentRow, currentCol - 1]);  // left cell
        if (currentCol + 1 < mapGrid.grid.GetLength(1)) dropableCellsCondidates.Add(mapGrid.grid[currentRow, currentCol + 1]);// right cell

        foreach (GridCell cell in dropableCellsCondidates)
        {
            //!!!!!!! there is more than only this condition to check .
            if (cell.occupantUnit == null)
            {
                this.dropableCells.Add(cell);
            }
        }

    }

    // Method to get a list of suppliable units
    public void GetSuppliableUnits()
    {
        // Implement logic to retrieve suppliable units here
        // search in the foor direction for unit can be supplied
    }



    // 
    public void HighlightDropableCells()
    {
        foreach (GridCell cell in dropableCells)
        {
            cell.HighlightAsDropable();
        }
        // highlight the dropable cells
    }

    // 
    public void HighlightSuppliableUnits()
    {
        foreach (Unit unit in suppliableUnits)
        {
            unit.unitView.HighlightAsSuppliable();
        }
        // highlight the dropable cells
    }



    //
    public void ResetDropableCells()
    {
        foreach (GridCell cell in dropableCells)
        {
            cell.ResetHighlitedCell();
        }
        // highlight the dropable cells
        dropableCells.Clear();
    }

    //
    public void ResetSuppliableUnits()
    {
        foreach (Unit unit in suppliableUnits)
        {
            unit.unitView.ResetHighlightedUnit();
        }
        suppliableUnits.Clear();
    }




}