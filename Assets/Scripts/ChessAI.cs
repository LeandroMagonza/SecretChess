using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
// ReSharper disable Unity.InefficientMultidimensionalArrayUsage

// keeps track of game logic and chess rules
public class ChessAI : MonoBehaviour {
    #region Singleton
    private static ChessAI instance;
    public static ChessAI Instance {
        get {
            if (instance == null)
                instance = FindObjectOfType<ChessAI>();
            if (instance == null)
                Debug.LogError("Singleton<" + typeof(ChessAI) + "> instance has been not found.");
            return instance;
        }
    }

    protected void Awake() {
        if (instance == null)
            instance = this as ChessAI;
        else if (instance != this)
            DestroySelf();
    }

    private void DestroySelf() {
        if (Application.isPlaying)
            Destroy(this);
        else
            DestroyImmediate(this);
    }

    #endregion
    
    private Piece[,] testingBoard = new Piece[8, 8];
    private Tile[,] originalBoard = new Tile[8, 8];


    public (Tile startingTile, Tile tileEnd) ChooseMove(Tile[,] instanceBoard, PlayerData currentPlayerData) {
        SetupTestingBoard(instanceBoard);

        int currentPlayerID = currentPlayerData.playerID;
        
        bool possibleMoveFound = false;
        while (!possibleMoveFound) {
            Piece testingPiece = currentPlayerData.piecesOwnedByPlayer[Random.Range(0, currentPlayerData.piecesOwnedByPlayer.Count)];
            Tile startingTile = BoardManager.Instance.FindTileOccupiedByPieceInBoard(testingPiece);
            if (startingTile is null) continue;
            List<(Tile tile, (Movement movement, int layer, (int row, int column) offset) markingData)> pieceMoves
                = testingPiece.movementPattern.GetAllMoves(startingTile, testingPiece.amountOfMoves,
                    testingPiece.InvertMovement());
            if (pieceMoves.Count != 0) {
                possibleMoveFound = true;
                (Tile tileEnd, (Movement movement, int layer, (int row, int column) offset) markingData) chosenMove =
                    pieceMoves[Random.Range(0, pieceMoves.Count)];
                return (startingTile, chosenMove.tileEnd);
            }

        }
        
        
        
        
        
        return (new Tile(), new Tile());
    }

    private void SetupTestingBoard(Tile[,] instanceBoard) {
        originalBoard = instanceBoard;
        for (int row = 0; row < 8; row++) {
            for (int column = 0; row < 8; row++) {
                testingBoard[row,column] = originalBoard[row, column].piece;
            }
        }
    }
}

