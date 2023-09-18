using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    //public Piece selectedPiece;
    public List<PlayerData> players = new List<PlayerData>();
    public PlayerAvatar[] playerAvatars;
    public int currentPlayer = 1;
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerController>();
            if (instance == null)
                Debug.LogError("Singleton<" + typeof(PlayerController) + "> instance has been not found.");
            return instance;
        }
    }

    protected void Awake()
    {
        if (instance == null) {
            instance = this as PlayerController;
            players.Add(
                new PlayerData(PlayerType.LOCAL_PLAYER,0,0)
                    .SetPieceColor(Color.red)
                    .SetPlayerAvatar(playerAvatars[0])
                );
            players.Add(
                new PlayerData(PlayerType.IA,1,1)
                    .SetPieceColor(Color.blue)
                    .SetPlayerAvatar(playerAvatars[1])
                );
            players[currentPlayer].DisplayTurn(true);
        }
        else if (instance != this)
            DestroySelf();
        
    }
    private void DestroySelf()
    {
        if (Application.isPlaying)
            Destroy(this);
        else
            DestroyImmediate(this);
    }
    public void SelectTile(Tile tile)
    {
        //Debug.Log(Instance.currentPlayer + " tried to select a piece owned by"+tile.piece?.GetOwner());
        if (1-BoardManager.Instance.turnNumber%2 == currentPlayer && tile.piece && tile.piece.GetOwnerID() == Instance.currentPlayer)
        {
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

        if (currentPlayer == 1 && players[1].playerType == PlayerType.IA) {
            StartCoroutine(ChooseMove());
        }
        else if (currentPlayer == 0 && players[0].playerType == PlayerType.IA) {
            StartCoroutine(ChooseMove());
        }
    }

    public IEnumerator ChooseMove() {
        yield return null;
    }

    public void ResetPlayer(int playerID) {
        Debug.Log("Resetting player "+playerID);
        if (playerID >= 0 && playerID < players.Count)
        {
            Debug.Log("Reseted player "+playerID);
            players[playerID].king = null;
            players[playerID].piecesOwnedByPlayer = new List<Piece>();
            players[playerID].SetInitiative(playerID);
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
        Debug.Log("seteada iniciativa "+ initiative);
        this.initiative = initiative;
    }

    public PlayerData SetPlayerAvatar(PlayerAvatar playerAvatar) {
        this.playerAvatar = playerAvatar;
        playerAvatar.owner = playerID;
        playerAvatar.DisplayInitiative(initiative);
        Debug.Log(playerAvatar.gameObject.name+" setting owner to: "+playerAvatar.owner);
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
}

public enum PlayerType{
    LOCAL_PLAYER,
    REMOTE_PLAYER,
    IA
}