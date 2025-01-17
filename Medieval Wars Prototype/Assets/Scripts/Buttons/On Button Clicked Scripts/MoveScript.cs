using UnityEngine;
using UnityEngine.UI;

public class MoveScript : MonoBehaviour
{
    public void OnMoveButtonDown()
    {
        Debug.Log("Move button pressed");

        ButtonsUI.Instance.UpdateActionButtonsToDisplayWhenAButtonIsClicked(ActionsHandler.Instance.actionButtons[0]);

        MovementSystem.Instance.GetWalkableTiles(UnitController.Instance.selectedUnit);

        UnitController.Instance.selectedUnit.unitView.HighlightWalkablesCells();

        // ManageInteractableObjects.Instance.MakeOnlySpecificCellsInteractable(UnitController.Instance.selectedUnit.walkableGridCells);

        UnitController.Instance.CurrentActionStateBasedOnClickedButton = UnitUtil.ActionToDoWhenButtonIsClicked.MOVE;

        // wait for they player to click on a walkable cell.

    }

}