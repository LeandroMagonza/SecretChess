using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public static class PieceLibrary
{
    public static GameObject piecePrefab;

    static PieceLibrary() {
        piecePrefab = Resources.Load("Prefabs/Piece") as GameObject;
    }
    public static Piece Pawn() {
        GameObject pieceGameObject = BoardManager.Instantiate(piecePrefab);
        Piece piece = pieceGameObject.GetComponent<Piece>();
        
        SetPieceSprite(pieceGameObject, "pawn_w");

        piece.movementPattern = new MovementPattern();
        Movement newMovement = new Movement();
        piece.movementPattern.AddMovement(newMovement);

        int layer;
        // layer = newMovement.AddLayer();
        // newMovement.AddStep(layer,(0,0),new CellCondition(CellContent.Start, true, MovementSymmetry.None));
        
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(1,0),new CellCondition(CellContent.Move));
        newMovement.AddStep(layer,(1,1),new CellCondition(CellContent.Capture));
        newMovement.AddStep(layer,(1,-1),new CellCondition(CellContent.Capture));

        return piece;
    }


    public static Piece Rook() {
        GameObject pieceGameObject = BoardManager.Instantiate(piecePrefab);
        Piece piece = pieceGameObject.GetComponent<Piece>();
        SetPieceSprite(pieceGameObject, "rook_w");
        
        piece.movementPattern = new MovementPattern();
        
        Movement newMovement = new Movement();

        int layer;
        // layer = newMovement.AddLayer();
        // newMovement.AddStep(layer,(0,0),new CellCondition(CellContent.Start, true, MovementSymmetry.None));
        
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(1,0),new CellCondition(CellContent.MoveAndCapture,true));
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(2,0),new CellCondition(CellContent.MoveAndCapture,true));        
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(3,0),new CellCondition(CellContent.MoveAndCapture,true));
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(4,0),new CellCondition(CellContent.MoveAndCapture,true));
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(5,0),new CellCondition(CellContent.MoveAndCapture,true));        
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(6,0),new CellCondition(CellContent.MoveAndCapture,true));
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(7,0),new CellCondition(CellContent.MoveAndCapture,true));        
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(8,0),new CellCondition(CellContent.MoveAndCapture,true));
        
        piece.movementPattern.AddMovement(newMovement,MovementSymmetry.Vertical);

        return piece;
    }
    private static void SetPieceSprite(GameObject pieceGameObject, string spriteName) {
        Image sprite;
        sprite = pieceGameObject.GetComponent<Image>();
        if (sprite != null) {
            Sprite loadedSprite = Resources.Load<Sprite>("Sprites/Pieces/"+spriteName);
            if (loadedSprite != null) {
                sprite.sprite = loadedSprite;
            }
            else {
                Debug.LogError("No se pudo cargar el sprite.");
            }
        }
        else {
            Debug.LogError("No se encontró el componente Image.");
        }
    }
}
