using UnityEngine;
public static class PieceLibrary
{
    public static GameObject piecePrefab;

    static PieceLibrary() {
        piecePrefab = Resources.Load("Prefabs/Piece") as GameObject;
    }
    public static Piece Pawn() {
        GameObject pieceGameObject = BoardManager.Instantiate(piecePrefab);
        Piece piece = pieceGameObject.GetComponent<Piece>();
        piece.value = 3;
        
        //SetPieceSprite(pieceGameObject, "pawn_w");
        SetPieceData(pieceGameObject, PieceType.PAWN);

        piece.movementPattern = new MovementPattern();
        Movement baseMovement = new Movement();
        
        piece.movementPattern.AddMovement(baseMovement);

        int layer;
        // layer = newMovement.AddLayer();
        // newMovement.AddStep(layer,(0,0),new CellCondition(CellContent.Start, true, MovementSymmetry.None));
        
        layer = baseMovement.AddLayer();
        baseMovement.AddStep(layer,(1,0),new CellCondition(CellContent.Move));
        baseMovement.AddStep(layer,(1,1),new CellCondition(CellContent.Capture));
        baseMovement.AddStep(layer,(1,-1),new CellCondition(CellContent.Capture));
        
        Movement firstMovement = new Movement();
        firstMovement.SetEnableMovementFromMoveAndToMove(0,0);
        
        piece.movementPattern.AddMovement(firstMovement);
        
        layer = firstMovement.AddLayer();
        firstMovement.AddStep(layer,(2,0),new CellCondition(CellContent.Move));

        return piece;
    }


    public static Piece Rook() {
        GameObject pieceGameObject = BoardManager.Instantiate(piecePrefab);
        Piece piece = pieceGameObject.GetComponent<Piece>();
        piece.value = 15;
        //SetPieceSprite(pieceGameObject, "rook_w");
        SetPieceData(pieceGameObject, PieceType.ROOK);

        piece.movementPattern = new MovementPattern();
        
        Movement newMovement = new Movement();

        int layer;
        // layer = newMovement.AddLayer();
        // newMovement.AddStep(layer,(0,0),new CellCondition(CellContent.Start, true, MovementSymmetry.None));
        
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(1,0),new CellCondition(CellContent.MoveAndCapture,true,true));
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(2,0),new CellCondition(CellContent.MoveAndCapture,true,true));        
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(3,0),new CellCondition(CellContent.MoveAndCapture,true,true));
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(4,0),new CellCondition(CellContent.MoveAndCapture,true,true));
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(5,0),new CellCondition(CellContent.MoveAndCapture,true,true));        
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(6,0),new CellCondition(CellContent.MoveAndCapture,true,true));
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(7,0),new CellCondition(CellContent.MoveAndCapture,true,true));        
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(8,0),new CellCondition(CellContent.MoveAndCapture,true,true));
        
        // piece.movementPattern.AddMovement(newMovement,MovementSymmetry.Horizontal);
        // piece.movementPattern.AddMovement(newMovement.GenerateMovementInvertingAxis((1,1),true),MovementSymmetry.Vertical);
        
        piece.movementPattern.AddMovement(newMovement,MovementSymmetry.Radial);
        return piece;
    }
    public static Piece Knight() {
        GameObject pieceGameObject = BoardManager.Instantiate(piecePrefab);
        Piece piece = pieceGameObject.GetComponent<Piece>();
        piece.value = 10;
        //SetPieceSprite(pieceGameObject, "knight_w");
        SetPieceData(pieceGameObject, PieceType.KNIGHT);
        
        piece.movementPattern = new MovementPattern();
        
        Movement newMovement = new Movement();

        int layer;
        // layer = newMovement.AddLayer();
        // newMovement.AddStep(layer,(0,0),new CellCondition(CellContent.Start, true, MovementSymmetry.None));
        
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(1,2),new CellCondition(CellContent.MoveAndCapture));
        
        
        piece.movementPattern.AddMovement(newMovement.GenerateMovementInvertingAxis((1,1),true),MovementSymmetry.Radial);
        piece.movementPattern.AddMovement(newMovement,MovementSymmetry.Radial);
        
        return piece;
    }
   public static Piece Bishop() {
       GameObject pieceGameObject = BoardManager.Instantiate(piecePrefab);
       Piece piece = pieceGameObject.GetComponent<Piece>();
       piece.value = 9;
        //SetPieceSprite(pieceGameObject, "bishop_w");
        SetPieceData(pieceGameObject, PieceType.BISHOP);

        piece.movementPattern = new MovementPattern();
        
       Movement diagonalMovement = new Movement();

       int layer;
       // layer = newMovement.AddLayer();
       // newMovement.AddStep(layer,(0,0),new CellCondition(CellContent.Start, true, MovementSymmetry.None));
        
       layer = diagonalMovement.AddLayer();
       diagonalMovement.AddStep(layer,(1,1),new CellCondition(CellContent.MoveAndCapture,true,true));
       layer = diagonalMovement.AddLayer();
       diagonalMovement.AddStep(layer,(2,2),new CellCondition(CellContent.MoveAndCapture,true,true));        
       layer = diagonalMovement.AddLayer();
       diagonalMovement.AddStep(layer,(3,3),new CellCondition(CellContent.MoveAndCapture,true,true));
       layer = diagonalMovement.AddLayer();
       diagonalMovement.AddStep(layer,(4,4),new CellCondition(CellContent.MoveAndCapture,true,true));
       layer = diagonalMovement.AddLayer();
       diagonalMovement.AddStep(layer,(5,5),new CellCondition(CellContent.MoveAndCapture,true,true));        
       layer = diagonalMovement.AddLayer();
       diagonalMovement.AddStep(layer,(6,6),new CellCondition(CellContent.MoveAndCapture,true,true));
       layer = diagonalMovement.AddLayer();
       diagonalMovement.AddStep(layer,(7,7),new CellCondition(CellContent.MoveAndCapture,true,true));        
       layer = diagonalMovement.AddLayer();
       diagonalMovement.AddStep(layer,(8,8),new CellCondition(CellContent.MoveAndCapture,true,true));
        
       piece.movementPattern.AddMovement(diagonalMovement,MovementSymmetry.Radial);
        
       return piece;
    }

    public static Piece King() {
        GameObject pieceGameObject = BoardManager.Instantiate(piecePrefab);
        Piece piece = pieceGameObject.GetComponent<Piece>();
        piece.value = 10000;
        //SetPieceSprite(pieceGameObject, "king_w");
        SetPieceData(pieceGameObject, PieceType.KING);

        piece.movementPattern = new MovementPattern();
        
        Movement newMovement = new Movement();

        int layer;
        // layer = newMovement.AddLayer();
        // newMovement.AddStep(layer,(0,0),new CellCondition(CellContent.Start, true, MovementSymmetry.None));
        
        layer = newMovement.AddLayer();
        newMovement.AddStep(layer,(1,0),new CellCondition(CellContent.MoveAndCapture));
        newMovement.AddStep(layer,(1,1),new CellCondition(CellContent.MoveAndCapture));
        
        
        //piece.movementPattern.AddMovement(newMovement.GenerateMovementInvertingAxis((1,1),true),MovementSymmetry.Radial);
        piece.movementPattern.AddMovement(newMovement,MovementSymmetry.Radial);
        
        return piece;
    }

    public static Piece Queen() {
        GameObject pieceGameObject = BoardManager.Instantiate(piecePrefab);
        Piece piece = pieceGameObject.GetComponent<Piece>();
        piece.value = 27;
        //SetPieceSprite(pieceGameObject, "queen_w");
        SetPieceData(pieceGameObject, PieceType.QUEEN);

        piece.movementPattern = new MovementPattern();
        

        int layer;
        Movement linearMovement = new Movement();
        
        layer = linearMovement.AddLayer();
        linearMovement.AddStep(layer,(1,0),new CellCondition(CellContent.MoveAndCapture,true,true));
        layer = linearMovement.AddLayer();
        linearMovement.AddStep(layer,(2,0),new CellCondition(CellContent.MoveAndCapture,true,true));        
        layer = linearMovement.AddLayer();
        linearMovement.AddStep(layer,(3,0),new CellCondition(CellContent.MoveAndCapture,true,true));
        layer = linearMovement.AddLayer();
        linearMovement.AddStep(layer,(4,0),new CellCondition(CellContent.MoveAndCapture,true,true));
        layer = linearMovement.AddLayer();
        linearMovement.AddStep(layer,(5,0),new CellCondition(CellContent.MoveAndCapture,true,true));        
        layer = linearMovement.AddLayer();
        linearMovement.AddStep(layer,(6,0),new CellCondition(CellContent.MoveAndCapture,true,true));
        layer = linearMovement.AddLayer();
        linearMovement.AddStep(layer,(7,0),new CellCondition(CellContent.MoveAndCapture,true,true));        
        layer = linearMovement.AddLayer();
        linearMovement.AddStep(layer,(8,0),new CellCondition(CellContent.MoveAndCapture,true,true));
        
        Movement diagonalMovement = new Movement();

        layer = diagonalMovement.AddLayer();
        diagonalMovement.AddStep(layer,(1,1),new CellCondition(CellContent.MoveAndCapture,true,true));
        layer = diagonalMovement.AddLayer();
        diagonalMovement.AddStep(layer,(2,2),new CellCondition(CellContent.MoveAndCapture,true,true));        
        layer = diagonalMovement.AddLayer();
        diagonalMovement.AddStep(layer,(3,3),new CellCondition(CellContent.MoveAndCapture,true,true));
        layer = diagonalMovement.AddLayer();
        diagonalMovement.AddStep(layer,(4,4),new CellCondition(CellContent.MoveAndCapture,true,true));
        layer = diagonalMovement.AddLayer();
        diagonalMovement.AddStep(layer,(5,5),new CellCondition(CellContent.MoveAndCapture,true,true));        
        layer = diagonalMovement.AddLayer();
        diagonalMovement.AddStep(layer,(6,6),new CellCondition(CellContent.MoveAndCapture,true,true));
        layer = diagonalMovement.AddLayer();
        diagonalMovement.AddStep(layer,(7,7),new CellCondition(CellContent.MoveAndCapture,true,true));        
        layer = diagonalMovement.AddLayer();
        diagonalMovement.AddStep(layer,(8,8),new CellCondition(CellContent.MoveAndCapture,true,true));
        
        piece.movementPattern.AddMovement(diagonalMovement,MovementSymmetry.Radial);
        piece.movementPattern.AddMovement(linearMovement,MovementSymmetry.Radial);
        
        return piece;
    }
    private static void SetPieceData(GameObject pieceGameObject, PieceType pieceType)
    {
        Piece piece = pieceGameObject.GetComponent<Piece>();
        if (piece != null)
        {
            PieceData data = Resources.Load<PieceData>("Data/Pieces/" + pieceType.ToString());
            if (data != null)
            {
                piece.SetData(ref data);
            }
            else
            {
                Debug.LogError("No se pudo cargar los datos.");
            }
        }
        else
        {
            Debug.LogError("No se encontró el componente Piece.");
        }
    }
}
