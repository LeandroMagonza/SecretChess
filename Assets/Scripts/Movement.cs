using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement {
    // Start is called before the first frame update
    [SerializeField]
    // list = layers, dictionary = steps
    public List<Dictionary<(int row, int column), CellCondition>> movementStencil =
        new List<Dictionary<(int row, int column), CellCondition>>();
    //public CellCondition[,] conditionMovementStencil = new CellCondition[17, 17];
    //public CellCondition[,] endingMovementStencil = new CellCondition[17, 17];

    public (int row, int column) startPosition = (0, 0);

    // private int? tileNumber = null;
    public bool CheckMoveValidity(int tileNumberStart, int tileNumberEnd) {
        return false;
    }

    public void MarkPossibleMovements(Tile tileStart, bool invert) {
        //Debug.Log("Called markpossible movements on tile " + tileStart.tileNumber);
        bool markTilesForMovement = true;
        foreach (var stepMovements in movementStencil) {
            if (!markTilesForMovement) {
                return;
            }

            List<Tile> tilesToMark = new List<Tile>();
            foreach (var content in stepMovements) {
                (int row, int column) offset = content.Key;
                CellCondition cellCondition = content.Value;
                
                //invert hace que el movimiento se invierta para el jugador 1, que arranca al final de la grilla y mira para abajo
                // se podria reemplazar con una orientacion para la pieza?
                // podriamos crear piezas que roten al moverse y se cambie para el lado que apunte su movimiento
                
                Tile tileToCheck = BoardManager.Instance.GetTile(
                    tileStart.tileNumber.row + offset.row * (invert ? -1 : 1),
                    tileStart.tileNumber.column + offset.column * (invert ? -1 : 1)
                );

                //si la casilla que marca el movimiento esta ocupada por una pieza del mismo equipo
                //En simulchess podes comerte tus propias piezas
                //if (tileToCheck.piece.GetOwner() == tileStart.piece.GetOwner()) {break;}
                switch (cellCondition.cellContent) {
                    //conditional types

                    case CellContent.Empty:
                        if (tileToCheck is null || tileToCheck.piece is not null) {
                            markTilesForMovement = false;
                        }

                        break;
                    case CellContent.Occupied:
                        if (tileToCheck is null || tileToCheck.piece is null) {
                            markTilesForMovement = false;
                        }
                        break;
                    case CellContent.Enemy:
                        if (tileToCheck is null || tileToCheck.piece is null || tileToCheck.piece.GetOwner() == tileStart.piece.GetOwner()) {
                            markTilesForMovement = false;
                        }
                        break;
                    case CellContent.Ally:
                        if (tileToCheck is null || tileToCheck.piece is null || tileToCheck.piece.GetOwner() != tileStart.piece.GetOwner()) {
                            markTilesForMovement = false;
                        }
                        break;
                    //movement types
                    case CellContent.MoveAndCapture:
                        if (tileToCheck is not null &&
                            (tileToCheck.piece is null || tileToCheck.piece.GetOwner() != tileStart.piece.GetOwner())
                            ) {
                            tilesToMark.Add(tileToCheck);
                            
                        }
                        else if (cellCondition.obligatory) { markTilesForMovement = false; }
                        break;
                    case CellContent.Capture:
                        if (tileToCheck is not null &&
                            tileToCheck.piece is not null && tileToCheck.piece.GetOwner() != tileStart.piece.GetOwner()) {
                            tilesToMark.Add(tileToCheck);
                        }
                        else if (cellCondition.obligatory) { markTilesForMovement = false; }
                        break;
                    case CellContent.Move:
                        if (tileToCheck is not null && tileToCheck.piece is null) {
                            tilesToMark.Add(tileToCheck);
                        }
                        else if (cellCondition.obligatory) { markTilesForMovement = false; }
                        break;
                }
            }

            Debug.Log(markTilesForMovement + " on finding possible movements");
            if (markTilesForMovement) {
                foreach (var tile in tilesToMark) {
                    // Debug.Log("marking on tile " + tile.tileNumber);
                    tile.MarkAsPossibleMovement(true);
                }
            }
        }
    }

    public int AddLayer() {
        movementStencil.Add(new Dictionary<(int row, int column), CellCondition>());
        return movementStencil.Count-1;
    }
    
    public void AddStep(int layer, (int row, int column) position, CellCondition cellCondition) {
        
        movementStencil[layer].Add(
            (startPosition.row + position.row ,startPosition.column + position.column),
            cellCondition
        );
    }
}