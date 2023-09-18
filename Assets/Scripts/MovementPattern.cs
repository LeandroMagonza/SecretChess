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

    public List<Move> GetAllMoves() {
        return new List<Move>();
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
