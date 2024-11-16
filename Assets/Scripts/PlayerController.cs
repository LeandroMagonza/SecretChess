using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour {
    // Start is called before the first frame update
    //public Piece selectedPiece;
    public List<PlayerData> players = new List<PlayerData>();
    public PlayerAvatar[] playerAvatars;
    public int currentPlayer = 1;
    private static PlayerController instance;
    public bool inputEnabled = true;

    public static PlayerController Instance {
        get {
            if (instance == null)
                instance = FindObjectOfType<PlayerController>();
            if (instance == null)
                Debug.LogError("Singleton<" + typeof(PlayerController) + "> instance has been not found.");
            return instance;
        }
    }

    protected void Awake() {
        if (instance == null) {
            instance = this as PlayerController;
            players.Add(
                new PlayerData(PlayerType.LOCAL_PLAYER, 0, 0)
                    .SetPieceColor(Color.red)
                    .SetPlayerAvatar(playerAvatars[0])
            );
            playerAvatars[0].UpdatePlayerTypeText(players[0].playerType);
            players.Add(
                new PlayerData(PlayerType.LOCAL_PLAYER, 1, 1)
                    .SetPieceColor(Color.blue)
                    .SetPlayerAvatar(playerAvatars[1])
            );
            playerAvatars[1].UpdatePlayerTypeText(players[1].playerType);
            players[currentPlayer].DisplayTurn(true);
        }
        else if (instance != this)
            DestroySelf();
    }

    private void DestroySelf() {
        if (Application.isPlaying)
            Destroy(this);
        else
            DestroyImmediate(this);
    }

    public void SelectTile(Tile tile) {
        //Debug.Log(Instance.currentPlayer + " tried to select a piece owned by"+tile.piece?.GetOwner());
        if (1 - BoardManager.Instance.turnNumber % 2 == currentPlayer && tile.piece &&
            tile.piece.GetOwnerID() == Instance.currentPlayer) {
            //selectedPiece = tile.piece;
            //BoardManager.Instance
            BoardManager.Instance.SetSelectedTile(tile);
        }
    }


    public IEnumerator ProcessMove(Tile tile) {
        foreach (var playerData in players) {
            playerData.DisplayTurn(false);
        }

        yield return StartCoroutine(BoardManager.Instance.ProcessMove(tile));
        currentPlayer = 1 - currentPlayer;
        //a cada avatar de jugador le pasa el id del jugador actual,si coincide con el propio hace una animacion 
        players[currentPlayer].DisplayTurn(true);

        if (players[currentPlayer].playerType == PlayerType.IA) {
            yield return StartCoroutine(ChooseMove(currentPlayer, players[currentPlayer]));
        }
    }

    public IEnumerator ChooseMove(int playerID, PlayerData playerData) {
        bool possibleMoveFound = false;
        while (!possibleMoveFound) {
            Piece testingPiece = playerData.piecesOwnedByPlayer[Random.Range(0, playerData.piecesOwnedByPlayer.Count)];
            Tile startingTile = BoardManager.Instance.FindTileOccupiedByPieceInBoard(testingPiece);
            if (startingTile is null) continue;
            List<(Tile tile, (Movement movement, int layer, (int row, int column) offset) markingData)> pieceMoves
                = testingPiece.movementPattern.GetAllMoves(startingTile, testingPiece.amountOfMoves,
                    testingPiece.InvertMovement());
            if (pieceMoves.Count != 0) {
                possibleMoveFound = true;
                (Tile tileEnd, (Movement movement, int layer, (int row, int column) offset) markingData) chosenMove =
                    pieceMoves[Random.Range(0, pieceMoves.Count)];
                BoardManager.Instance.SetSelectedTile(startingTile);
                yield return ProcessMove(chosenMove.tileEnd);
            }

            yield return null;
        }
    }

    public void ResetPlayer(int playerID) {
        Debug.Log("Resetting player " + playerID);
        if (playerID >= 0 && playerID < players.Count) {
            Debug.Log("Reseted player " + playerID);
            players[playerID].king = null;
            players[playerID].piecesOwnedByPlayer = new List<Piece>();
            players[playerID].SetInitiative(playerID);
            players[playerID].playerAvatar.UpdatePlayerTypeText(players[playerID].playerType);
        }
    }

    public void TogglePlayerType(int playerID)
    {
        if (playerID >= 0 && playerID < players.Count)
        {
            PlayerData playerData = players[playerID];
            
            // Toggle between IA and LOCAL_PLAYER
            playerData.playerType = playerData.playerType == PlayerType.IA 
                ? PlayerType.LOCAL_PLAYER 
                : PlayerType.IA;

            // Update the UI text
            playerData.playerAvatar.UpdatePlayerTypeText(playerData.playerType);
        }
    }
}

public class PlayerData {
    public int playerID;
    public PlayerType playerType;
    public Piece king;
    public List<Piece> piecesOwnedByPlayer = new List<Piece>();
    public Color pieceColor;
    public PlayerAvatar playerAvatar;
    public int initiative { get; private set; }

    public PlayerData(PlayerType playerType, int playerID, int initiative) {
        this.playerType = playerType;
        this.playerID = playerID;
        SetInitiative(initiative);
    }

    public void SetInitiative(int initiative) {
        //Debug.Log("seteada iniciativa "+ initiative);
        this.initiative = initiative;
    }

    public PlayerData SetPlayerAvatar(PlayerAvatar playerAvatar) {
        this.playerAvatar = playerAvatar;
        playerAvatar.owner = playerID;
        playerAvatar.DisplayInitiative(initiative);
        //Debug.Log(playerAvatar.gameObject.name+" setting owner to: "+playerAvatar.owner);
        playerAvatar.SetOriginalColor(pieceColor);
        return this;
    }

    public void DisplayTurn(bool active) {
        playerAvatar.DisplayTurn(active);
    }

    public void DisplayInitiative(int initiative) {
        playerAvatar.DisplayInitiative(initiative);
    }

    public void SetKing(Piece appointedKing) {
        if (king is not null) king.MarkAsKing(false);
        king = appointedKing;
        if (king is not null) king.MarkAsKing(true);
    }

    public void AddPiece(Piece pieceToAdd) {
        piecesOwnedByPlayer.Add(pieceToAdd);
    }

    public PlayerData SetPieceColor(Color color) {
        pieceColor = color;
        return this;
    }

    public void RemovePiece(Piece pieceToDelete) {
        foreach (var currentCheckingPiece in piecesOwnedByPlayer.ToList()) {
            if (currentCheckingPiece == pieceToDelete) {
                piecesOwnedByPlayer.Remove(currentCheckingPiece);
            }
        }
    }
}

public enum PlayerType {
    LOCAL_PLAYER,
    REMOTE_PLAYER,
    IA
}