using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPattern {
    [SerializeField]
    private List<Movement> movements = new List<Movement>();
    // private int? tileNumber = null;

    public void ShowPossibleMoves(Tile tile, int amountOfMoves, bool invert) {
        foreach (var movement in movements) {
            //throw new NotImplementedException();
           movement.MarkPossibleMovements(tile, invert);
        }
    }

    public List<(Tile tile, (Movement movement, int layer, (int row, int column) offset) markingData)> GetAllMoves(Tile tile, int amountOfMoves, bool invert) {
        List<(Tile tile, (Movement movement, int layer, (int row, int column) offset) markingData)> moves = new List<(Tile tile, (Movement movement, int layer, (int row, int column) offset) markingData)>(); 
        foreach (var movement in movements) {
            foreach (var endTileAndMarkingData in movement.GetPossibleMoves(tile, invert)) {
                moves.Add(endTileAndMarkingData);
            }
        }
        return moves;
    }

    public void AddMovement(Movement movement,MovementSymmetry symmetry = MovementSymmetry.None) {
        movements.Add(movement);
        
        switch (symmetry) {
            case MovementSymmetry.Horizontal:
                AddMovement(movement.GenerateMovementInvertingAxis((-1,1)));
                break;
            case MovementSymmetry.Vertical:
                AddMovement(movement.GenerateMovementInvertingAxis((1,-1)));
                break;
            case MovementSymmetry.Radial:
                AddMovement(movement.GenerateMovementInvertingAxis((1,-1),true));
                AddMovement(movement.GenerateMovementInvertingAxis((-1,-1)));
                AddMovement(movement.GenerateMovementInvertingAxis((-1,1),true));
                break;
        }
        
    }

    
    
}
