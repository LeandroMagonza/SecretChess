using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPattern {
    [SerializeField]
    private List<Movement> movements = new ();
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
    public List<Move> GetAllMovesForAI(Piece movingPiece,(int row, int column) startingTileNumber, int amountOfMoves, bool invert)
    {
        List<Move> moves = new List<Move>();
        foreach (var movement in movements) {
            //modify get possible moves so it also works with ai, might have to copy it and change stuff
            
            // foreach (var endTileAndMarkingData in movement.GetPossibleMoves(tile, invert)) {
            //     moves.Add(new Move(startingTileNumber, endTileAndMarkingData.endTile.tileNumber,endTileAndMarkingData.endTile.piece));
            // }
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
