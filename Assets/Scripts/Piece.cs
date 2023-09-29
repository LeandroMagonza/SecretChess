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

    private PieceData pieceData;
    private Image renderImage;
    private Canvas canvas;

    //public int priority = 0;
    [SerializeField]
    int owner;
 
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        kingMarker = transform.Find("KingMarker").gameObject;
        //ManagerEffects.Instance?.SquashPiece(transform, true, 0.1f, 20);
    }
    public void SetData(ref PieceData data) 
    {
        pieceData = data;
        renderImage = GetComponent<Image>();
        if (pieceData.base_sprite != null)
        {
            renderImage.sprite = pieceData.base_sprite;
            ManagerGame.Instance?.InstantiateMask(this);
        }
        else
        {
            Debug.LogError("No se pudo cargar el sprite.");
        }
    }
    public PieceData GetData() 
    { 
        return pieceData; 
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
        //_audioSource.PlayOneShot(captureClip);
        //CameraShake.Shake(0.1f, 0.1f);
        ManagerEffects.Instance.PlaySound(pieceData.capture_Clip);

    }
    public void GetCaptured(Piece capturerPiece) {
        //kingMarker.SetActive(king);
        //_audioSource.PlayOneShot(getCapturedClip);
        ManagerEffects.Instance.PlayEffectIn(pieceData.die_ParticleEffect, pieceData.die_Clip, transform.position);
        CameraShake.Shake(0.1f, 0.2f);
        PlayerController.Instance.players[GetOwnerID()].RemovePiece(this);
    }
    public void Move(Tile targetTile) {
        //kingMarker.SetActive(king);
        ManagerEffects.Instance?.PlaySound(pieceData.move_Clip);
        //_audioSource.PlayOneShot(moveClip);
    }
    #endregion
}

public enum PieceType {
    ROOK,PAWN,BISHOP,QUEEN,KING,KNIGHT
}
