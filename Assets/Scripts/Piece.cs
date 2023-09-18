using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Piece : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    // private int? tileNumber = null;
    public MovementPattern movementPattern;
    public GameObject kingMarker;
    public int amountOfMoves = 0;
    public int amountOfCaptures = 0;
    public int currentLevel = 0;
    public int maxLives = 1;
    public int amountOfLivesLeft = 1;
    public int value = 3;
    //public int priority = 0;
    [SerializeField]
    int owner;

 
    void Start()
    {
        kingMarker = transform.Find("KingMarker").gameObject;
    }

    public void SetOwner(int ownerID,PlayerData ownerData)
    {
        this.owner = ownerID;
        GetComponent<Image>().color = ownerData.pieceColor;
    }
    public void SetMovementPattern(MovementPattern movementPattern) {
        this.movementPattern = movementPattern;
    }
    public int GetOwnerID()
    {
        return this.owner;
    }


    public void ShowPossibleMoves(Tile tile) {
        bool invert = false;
        if (tile.piece.owner == 0) {
            invert = false;
        }
        else if(tile.piece.owner == 1) {
            invert = true;
        }
        else {
            Debug.Log("Invalid owner "+tile.piece.owner);
        }
        movementPattern.ShowPossibleMoves(tile,amountOfMoves,invert);
    }

    public void MarkAsKing(bool king) {
        kingMarker.SetActive(king);
    }
}

public enum BasicPieces {
    ROOK,PAWN,BISHOP,QUEEN,KING,KNIGHT
}
