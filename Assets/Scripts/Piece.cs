using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    
    //Sound
    private AudioSource _audioSource;

    public AudioClip captureClip;
    public AudioClip getCapturedClip;
    public AudioClip moveClip;
    //public int priority = 0;
    [SerializeField]
    int owner;
 
    void Start()
    {
        kingMarker = transform.Find("KingMarker").gameObject;
        _audioSource = GetComponent<AudioSource>();
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


    public void ShowPossibleMoves(Tile tile)
    {
        movementPattern.ShowPossibleMoves(tile, amountOfMoves, tile.piece.InvertMovement());
    }

    public bool InvertMovement()
    {
        bool invert = false;
        if (owner == 0) {
            invert = false;
        }
        else if(owner == 1) {
            invert = true;
        }
        else {
            Debug.Log("Invalid owner "+owner);
        }

        return invert;
    }
    public void MarkAsKing(bool king) {
        kingMarker.SetActive(king);
    }

    #region OnEvent
    public void Capture(Piece capturedPiece) {
        //kingMarker.SetActive(king);
        _audioSource.PlayOneShot(captureClip);
    }
    public void GetCaptured(Piece capturerPiece) {
        //kingMarker.SetActive(king);
        _audioSource.PlayOneShot(getCapturedClip);
        PlayerController.Instance.players[GetOwnerID()].RemovePiece(this);
    }
    public void Move(Tile targetTile) {
        //kingMarker.SetActive(king);
        _audioSource.PlayOneShot(moveClip);
    }
    #endregion
}

public enum BasicPieces {
    ROOK,PAWN,BISHOP,QUEEN,KING,KNIGHT
}
