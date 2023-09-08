using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    //public Piece selectedPiece;
    public List<PlayerData> players = new List<PlayerData>();
   
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
            players.Add(new PlayerData(PlayerType.LOCAL_PLAYER).SetPieceColor(Color.red));
            players.Add(new PlayerData(PlayerType.IA).SetPieceColor(Color.blue));
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
        if (1-BoardManager.Instance.turnNumber%2 == currentPlayer && tile.piece && tile.piece.GetOwner() == Instance.currentPlayer)
        {
            //selectedPiece = tile.piece;
            BoardManager.Instance.SetSelectedTile(tile);
        }

    }


    public void ExecuteMove(Tile tile) {
        BoardManager.Instance.ExecuteMove(tile);
        currentPlayer = 1 - currentPlayer;
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
            foreach (Piece piece in players[playerID].piecesOwnedByPlayer.ToArray()) {
               //Destroy(piece.gameObject);
            }
            players[playerID].piecesOwnedByPlayer = new List<Piece>();
        }
    }
}

public class PlayerData {
    public PlayerType playerType;
    public Piece king;
    public List<Piece> piecesOwnedByPlayer = new List<Piece>();
    public Color pieceColor;

    public PlayerData(PlayerType playerType) {
        this.playerType = playerType;
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