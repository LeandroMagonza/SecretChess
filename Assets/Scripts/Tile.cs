using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    // private int? tileNumber = null;
    public (int row,int column) tileNumber;
    public Piece? piece = null;
    public GameObject possibleMoveMarker;

    public Color32 originalColor;

    public void SetTileNumber((int,int) tileNumber) {
        possibleMoveMarker = transform.Find("PossibleMoveMarker").gameObject;
        this.tileNumber = tileNumber;
    }
    public void SetPiece(Piece piece)
    {
        // if (tileNumber == null)
        // {
        this.piece = piece;
        if (piece is not null) {
            piece.transform.SetParent(this.transform);
            piece.transform.position = this.transform.position;
            piece.transform.SetAsFirstSibling();
            piece.transform.localScale = new Vector3(0.75f,0.85f,1);
        }
        // }
    }

    public void OnClick()
    {
        if (possibleMoveMarker.activeInHierarchy) {
            PlayerController.Instance.ExecuteMove(this);
        }
        else {
            PlayerController.Instance.SelectTile(this);
        }
    }

    public void MarkAsSelected(bool selected)
    {
        if (selected) {
            Color32 previousColor = GetComponent<Image>().color;
            GetComponent<Image>().color = new Color32(
                255,
                255,
                0,
                255);
        }
        else {
            GetComponent<Image>().color = new Color32(
                originalColor.r,
                originalColor.g,
                originalColor.b,
                originalColor.a);
        }
        MarkAsPossibleMovement(false);
    }

    public void MarkAsPossibleMovement(bool possible)
    {
        if (possible) {
            //Debug.Log("Mark as possiblemovement called on tile "+tileNumber);
            possibleMoveMarker.SetActive(true);
        }
        else {
            possibleMoveMarker.SetActive(false);
        }
    }

    void OnDestroy() {
        BoardManager.OnTileSelected -= MarkAsSelected;
    }
}
