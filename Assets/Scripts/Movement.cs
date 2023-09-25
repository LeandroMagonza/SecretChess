using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Movement {
    // Start is called before the first frame update
    [SerializeField]
    // list = layers, dictionary = steps
    public List<Dictionary<(int row, int column), CellCondition>> movementStencil =
        new List<Dictionary<(int row, int column), CellCondition>>();
    //public CellCondition[,] conditionMovementStencil = new CellCondition[17, 17];
    //public CellCondition[,] endingMovementStencil = new CellCondition[17, 17];

    public (int row, int column) startPosition = (0, 0);

    private int? enableMovementFromMove = null;
    private int? enableMovementToMove = null;

    private int? enableMovementFromTurn = null;
    private int? enableMovementToTurn = null;

    private int? enableMovementFromLevel = null;
    private int? enableMovementToLevel = null;

    private List<Phase> disabledOnPhase;

    public void MarkPossibleMovements(Tile tileStart, bool invert) {
        foreach (var tileAndMovement in GetPossibleMoves(tileStart, invert)) { 
            tileAndMovement.endTile.MarkAsPossibleMovement((tileAndMovement.markingData));
        }
    }
    public List<(Tile endTile, (Movement movement, int layer, (int row, int column) offset) markingData)> GetPossibleMoves(Tile tileStart, bool invert) {
        List<(Tile endTile, (Movement movement, int layer, (int row, int column) offset) markingData)> markedTiles =
            new List<(Tile tile, (Movement movement, int layer, (int row, int column) offset))>();
        if (tileStart.piece is null) {
            //devuelve la lista vacia
            Debug.Log("Selected piece was null, this shouldnt happen but idk");
            return markedTiles;
            
        }
        if (
            (enableMovementFromMove is not null &&
             enableMovementFromMove > tileStart.piece.amountOfMoves)
            ||
            (enableMovementToMove is not null &&
             tileStart.piece.amountOfMoves > enableMovementToMove)
        ) {
            Debug.Log("Movement disabled on this amount of moves " + enableMovementFromMove + ">" +
                      tileStart.piece.amountOfMoves + ">" + enableMovementToMove);
            //devuelve la lista vacia
            return markedTiles;
        }

        //Debug.Log("Called markpossible movements on tile " + tileStart.tileNumber);
        bool markTilesForMovement = true;
        bool markNextLayer = true;
        
        for (int layer = 0; layer < movementStencil.Count; layer++) {
            var stepMovements = movementStencil[layer];
            if (!markTilesForMovement || !markNextLayer) {
                //devuelve la lista vacia
                return markedTiles;
            }

            List<(Tile tile, (Movement movement, int layer, (int row, int column) offset) markingData)> tilesToMark =
                new List<(Tile tile, (Movement movement, int layer, (int row, int column) offset))>();
            foreach (var content in stepMovements) {
                bool addTileToMark = false;
                if (!markTilesForMovement) {
                    //devuelve la lista vacia
                    return markedTiles;
                }

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
                (markTilesForMovement, markNextLayer, addTileToMark) =
                    CheckTileForMovement(tileStart.piece, cellCondition, tileToCheck);
                if (addTileToMark) {
                    tilesToMark.Add((tileToCheck, (this, layer, offset)));
                }
            }

            //Debug.Log(markTilesForMovement + " on finding possible movements");
            if (markTilesForMovement) {
                foreach (var tileAndMovement in tilesToMark) {
                    // Debug.Log("marking on tile " + tile.tileNumber);
                    markedTiles.Add((tileAndMovement.tile, tileAndMovement.markingData));
                }
            }
        }
        return markedTiles;
    }

    public (bool markTilesForMovement, bool markNextLayer, bool addTileToMark) CheckTileForMovement(
        Piece movingPiece,
        CellCondition cellCondition, Tile tileToCheck) {
        bool addTileToMark = false;
        bool markTilesForMovement = true;
        bool markNextLayer = true;

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
                if (tileToCheck is null || tileToCheck.piece is null ||
                    tileToCheck.piece.GetOwnerID() == movingPiece.GetOwnerID()) {
                    markTilesForMovement = false;
                }

                break;
            case CellContent.Ally:
                if (tileToCheck is null || tileToCheck.piece is null ||
                    tileToCheck.piece.GetOwnerID() != movingPiece.GetOwnerID()) {
                    markTilesForMovement = false;
                }

                break;
            //movement types
            case CellContent.MoveAndCapture:
                if (tileToCheck is not null &&
                    (tileToCheck.piece is null || tileToCheck.piece.GetOwnerID() != movingPiece.GetOwnerID())
                   ) {
                    addTileToMark = true;
                    if (cellCondition.endOnCapture && tileToCheck.piece is not null) {
                        markNextLayer = false;
                    }
                }
                else if (cellCondition.obligatory) {
                    markTilesForMovement = false;
                }

                break;
            case CellContent.MoveAndCaptureAny:
                if (tileToCheck is not null &&
                    (tileToCheck.piece is null)
                   ) {
                    addTileToMark = true;
                    if (cellCondition.endOnCapture && tileToCheck.piece is not null) {
                        markNextLayer = false;
                    }
                }
                else if (cellCondition.obligatory) {
                    markTilesForMovement = false;
                }

                break;
            case CellContent.Capture:
                if (tileToCheck is not null &&
                    tileToCheck.piece is not null &&
                    tileToCheck.piece.GetOwnerID() != movingPiece.GetOwnerID()) {
                    addTileToMark = true;
                    if (cellCondition.endOnCapture) {
                        markNextLayer = false;
                    }
                }
                else if (cellCondition.obligatory) {
                    markTilesForMovement = false;
                }

                break;
            case CellContent.CaptureAny:
                if (tileToCheck is not null &&
                    tileToCheck.piece is not null) {
                    addTileToMark = true;
                    if (cellCondition.endOnCapture) {
                        markNextLayer = false;
                    }
                }
                else if (cellCondition.obligatory) {
                    markTilesForMovement = false;
                }

                break;
            case CellContent.Move:
                if (tileToCheck is not null && tileToCheck.piece is null) {
                    addTileToMark = true;
                }
                else if (cellCondition.obligatory) {
                    markTilesForMovement = false;
                }

                break;
        }

        return (markTilesForMovement, markNextLayer, addTileToMark);
    }

    public int AddLayer() {
        movementStencil.Add(new Dictionary<(int row, int column), CellCondition>());
        return movementStencil.Count - 1;
    }

    public void AddStep(int layer, (int row, int column) position, CellCondition cellCondition) {
        movementStencil[layer].Add(
            (startPosition.row + position.row, startPosition.column + position.column),
            cellCondition
        );
    }

    public Movement GenerateMovementInvertingAxis((int row, int column) invert, bool swap = false) {
        // swap intercambia la coordenada de la fila con la de la columna
        // invert multiplica por un numero la fila y por otro la columno, esta pensado para usarse solo con 1 y -1 por ahora
        // para trasladar el movimiento a otro cuadrante
        Movement newMovement = new Movement();
        newMovement.SetEnableMovementFromMoveAndToMove(enableMovementFromMove, enableMovementToMove);

        for (int layerIndex = 0; layerIndex < movementStencil.Count; layerIndex++) {
            int layer;
            var layerData = movementStencil[layerIndex];
            layer = newMovement.AddLayer();


            foreach (var movementStep in layerData) {
                (int row, int column) resultingCoordinates;
                if (!swap) {
                    resultingCoordinates = (
                        invert.row * movementStep.Key.row,
                        invert.column * movementStep.Key.column);
                }
                else {
                    resultingCoordinates = (
                        invert.column * movementStep.Key.column,
                        invert.row * movementStep.Key.row
                    );
                }

                // el if hace que no puedas volver a agregar un movimiento a una celda que ya tenia un movimiento
                if (!movementStencil[layer].ContainsKey(resultingCoordinates)
                    ||
                    movementStencil[layer][resultingCoordinates] is null) {
                    newMovement.AddStep(layer,
                        resultingCoordinates,
                        new CellCondition(movementStep.Value.cellContent, movementStep.Value.obligatory,
                            movementStep.Value.endOnCapture));
                }
            }
        }

        return newMovement;
    }

    public void SetEnableMovementFromMoveAndToMove(int? fromMove, int? toMove = null) {
        enableMovementFromMove = fromMove;
        enableMovementToMove = toMove;
    }
}