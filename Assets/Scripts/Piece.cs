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
    [SerializeField]
    int owner;

    private int amountOfMoves;
 
    void Start()
    {
        kingMarker = transform.Find("KingMarker").gameObject;
    }

    public void SetOwner(int ownerID,PlayerData ownerData)
    {
        this.owner = ownerID;
        GetComponent<Image>().color = ownerData.pieceColor;
    }
    public void SetMovementPatter(MovementPattern movementPattern) {
        this.movementPattern = movementPattern;
    }
    public int GetOwner()
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
