using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPattern {
    [SerializeField]
    private List<Movement> movements = new List<Movement>();
    // private int? tileNumber = null;
    public bool CheckMoveValidity(int tileNumberStart, int tileNumberEnd,int amountOfMoves){

        foreach(Movement movement in movements){
            if (movement.CheckMoveValidity(tileNumberStart, tileNumberEnd))
                return true;
        }
        return false;
    }

    public void ShowPossibleMoves(Tile tile, int amountOfMoves, bool invert) {
        foreach (var movement in movements) {
            //throw new NotImplementedException();
           movement.MarkPossibleMovements(tile, invert);
        }
    }

    public void AddMovement(Movement movement,MovementSymmetry symmetry = MovementSymmetry.None) {
        movements.Add(movement);
        int layer;
        switch (symmetry) {
            case MovementSymmetry.Horizontal:
                Movement horizontalMovement = new Movement();
                AddMovement(horizontalMovement);

                for (int layerIndex = 0; layerIndex < movement.movementStencil.Count; layerIndex++) {
                    var layerData = movement.movementStencil[layerIndex];
                    layer = horizontalMovement.AddLayer();
                    foreach (var movementStep in layerData) {
                        // el if hace que no puedas volver a agregar un movimiento a una celda que ya tenia un movimiento
                        if (!movement.movementStencil[layer].ContainsKey((-movementStep.Key.row, movementStep.Key.column))
                            ||
                            movement.movementStencil[layer][(-movementStep.Key.row,-movementStep.Key.column)] is null) {
                            
                            horizontalMovement.AddStep(layer,
                                (-movementStep.Key.row,movementStep.Key.column),
                                new CellCondition(movementStep.Value.cellContent, movementStep.Value.obligatory , movementStep.Value.endOnCapture));
                        }
                    }
                }
                break;
            case MovementSymmetry.Vertical:
                Movement verticalMovement = new Movement();
                AddMovement(verticalMovement);

                for (int layerIndex = 0; layerIndex < movement.movementStencil.Count; layerIndex++) {
                    var layerData = movement.movementStencil[layerIndex];
                    layer = verticalMovement.AddLayer();
                    foreach (var movementStep in layerData) {
                        // el if hace que no puedas volver a agregar un movimiento a una celda que ya tenia un movimiento
                        if (!movement.movementStencil[layer].ContainsKey((movementStep.Key.row, -movementStep.Key.column))
                            ||
                            movement.movementStencil[layer][(movementStep.Key.row,-movementStep.Key.column)] is null) {
                            
                            verticalMovement.AddStep(layer,
                                (movementStep.Key.row,-movementStep.Key.column),
                                new CellCondition(movementStep.Value.cellContent, movementStep.Value.obligatory , movementStep.Value.endOnCapture));
                        }
                    }
                }
                break;
            case MovementSymmetry.Radial:
                break;
        }
    }
    
}
