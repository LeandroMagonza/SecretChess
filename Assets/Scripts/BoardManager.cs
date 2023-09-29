using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject rowPrefab;
    public GameObject tilePrefab;
    public GameMode gamemode = GameMode.TurnBased;

    public SortedDictionary<int,(Piece movingPiece, Tile endingTile,(Movement movement, int layer, (int row, int column) offset ))> moveQueue = new SortedDictionary<int, (Piece movingPiece, Tile endingTile, (Movement movement, int layer, (int row, int column) offset) moveData)>();
    public SortedDictionary<int,(Piece movingPiece, Tile endingTile,(Movement movement, int layer, (int row, int column) offset ))> kingMoveQueue = new SortedDictionary<int, (Piece movingPiece, Tile endingTile, (Movement movement, int layer, (int row, int column) offset) moveData)>();
    public delegate void TileSelected(bool selected);

    public static event TileSelected OnTileSelected;
    private Tile[,] board = new Tile[8, 8];
    private Row[] boardInRows = new Row[8];
    private Tile currentlySelectedTile;
    private static BoardManager instance;
    public TextMeshProUGUI matchEndText;

    public int turnNumber = 1;
    private bool processingMoves = false;

    public static BoardManager Instance {
        get {
            if (instance == null)
                instance = FindObjectOfType<BoardManager>();
            if (instance == null)
                Debug.LogError("Singleton<" + typeof(BoardManager) + "> instance has been not found.");
            return instance;
        }
    }

    protected void Awake() {
        if (instance == null)
            instance = this as BoardManager;
        else if (instance != this)
            DestroySelf();
    }

    private void DestroySelf() {
        if (Application.isPlaying)
            Destroy(this);
        else
            DestroyImmediate(this);
    }

    void Start() {
        for (int row = 0; row < 8; row++) {
            //instantiate row and place row inside of board
            GameObject currentRow;
            if (boardInRows.Length > row && boardInRows[row] != null) {
                currentRow = boardInRows[row].gameObject;
            }
            else {
                currentRow = Instantiate(rowPrefab, transform);
                boardInRows[row] = currentRow.GetComponent<Row>();
                currentRow.GetComponent<Row>().SetRowNumber(row);
            }

            for (int column = 0; column < 8; column++) {
                //instantiate tile inside row
                GameObject currentTile = Instantiate(tilePrefab, currentRow.transform);
                //set tile number
                currentTile.GetComponent<Tile>().SetTileNumber((row, column));
                //color tile white if necessary
                OnTileSelected += currentTile.GetComponent<Tile>().MarkAsSelected;
                if ((column + row) % 2 == 0) {
                    currentTile.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                }

                currentTile.GetComponent<Tile>().originalColor = currentTile.GetComponent<Image>().color;
                board[row, column] = currentTile.GetComponent<Tile>();

                Piece piece = null;
                if (row == 0 || row == 7) {
                    switch (column) {
                        case 0:
                            piece = PieceLibrary.Rook();
                            break;
                        case 1:	
                            piece = PieceLibrary.Knight();
                            break;
                        case 2:	
                            piece = PieceLibrary.Bishop();
                            break;
                        case 3:
                            piece = PieceLibrary.King();
                            if (row == 0) {
                                PlayerController.Instance.players[0].SetKing(piece);
                            }
                            else if(row == 7) {
                                PlayerController.Instance.players[1].SetKing(piece);
                            }
                            break;
                        case 4:	
                            piece = PieceLibrary.Queen();
                            break;
                        case 5:	
                            piece = PieceLibrary.Bishop();
                            break;
                        case 6:	
                            piece = PieceLibrary.Knight();
                            break;
                        case 7:
                            piece = PieceLibrary.Rook();
                            break;
                        default:
                            piece = PieceLibrary.Pawn();
                            break;
                    }
                }
                if (row == 1 || row == 6) {
                    piece = PieceLibrary.Pawn();
                }
                if (piece is not null) {
                    if (row == 0|| row == 1) {
                        piece.SetOwner(0, PlayerController.Instance.players[0]);
                        PlayerController.Instance.players[0].AddPiece(piece);
                    }else if (row == 6 || row == 7) {
                        piece.SetOwner(1, PlayerController.Instance.players[1]);
                        PlayerController.Instance.players[1].AddPiece(piece);
                    }
                    currentTile.GetComponent<Tile>().SetPiece(piece);
                }

            }
        }

        int randomPieceCrownedKing;
        randomPieceCrownedKing = Random.Range(0, PlayerController.Instance.players[0]
            .piecesOwnedByPlayer.Count);
        // Debug.Log("king 0 " + randomPieceCrownedKing + " between 0 and " + PlayerController.Instance.players[0]
        //     .piecesOwnedByPlayer.Count);
        if (PlayerController.Instance.players[0].king is null) {
            PlayerController.Instance.players[0].SetKing(
                PlayerController.Instance.players[0]
                    .piecesOwnedByPlayer[randomPieceCrownedKing]
            );
        }

        randomPieceCrownedKing = Random.Range(0, PlayerController.Instance.players[1]
            .piecesOwnedByPlayer.Count);
        // Debug.Log("king 1 " + randomPieceCrownedKing + " between 0 and " + PlayerController.Instance.players[1]
        //     .piecesOwnedByPlayer.Count);
        if (PlayerController.Instance.players[1].king is null) {
            PlayerController.Instance.players[1].SetKing(
                PlayerController.Instance.players[1]
                    .piecesOwnedByPlayer[randomPieceCrownedKing]
            );
        }
        
    }

    private void ShowPossibleMoves(Tile tile) {
        tile.piece.ShowPossibleMoves(tile);
    }

    private void UpdateBoard() { }

    public void SetSelectedTile(Tile selectedTile) {
        OnTileSelected?.Invoke(false);
        if(processingMoves){return;}
        if (currentlySelectedTile == selectedTile) {
            selectedTile = null;
        } 
        currentlySelectedTile = selectedTile;
        if (selectedTile is not null) {
            board[selectedTile.tileNumber.Item1, selectedTile.tileNumber.Item2].MarkAsSelected(true);
            ShowPossibleMoves(selectedTile);
        }
    }

    public Tile GetTile(int row, int column) {
        if (board.GetLength(0) <= row || board.GetLength(1) <= column) {
            return null;
        }

        if (row < 0 || column < 0) {
            return null;
        }

        return board[row, column];
    }

    public IEnumerator ProcessMove(Tile endingTile) {
        turnNumber++;
        Tile selectedTile = currentlySelectedTile;
        PlayerData actingPlayer = PlayerController.Instance.players[selectedTile.piece.GetOwnerID()];
        // para que deje de mostrarse los posibles movimientos en el tablero, y el tile como seleccionado
        var endingTileMarkingMovement = endingTile.markingMovement;
        SetSelectedTile(null);
        switch (gamemode) {
            case GameMode.TurnBased:
                //aca esta medio al pedo pasar por separado el markingmovement y el ending tile,
                //porque el movimiento a ejecutar es el mismo que esta marcado y no pudo haber sido modificado en el medio
                //pense en que el marking movement sea nuleable en execute move, pero me traia problemas
                ExecuteMove(selectedTile.piece,endingTile,endingTileMarkingMovement.GetValueOrDefault());
                break;
            case GameMode.Simultaneous:
                int playerInitiative = actingPlayer.initiative;
                if (endingTileMarkingMovement is null) {
                    throw new Exception("endingTileMarkingMovement is null");
                }
                if (endingTileMarkingMovement.GetValueOrDefault().movement is null) {
                    throw new Exception("endingTileMarkingMovement.GetValueOrDefault().movement is null");
                }
                if (endingTileMarkingMovement.GetValueOrDefault().movement.movementStencil is null) {
                    throw new Exception("endingTileMarkingMovement.GetValueOrDefault().movement.movementStencil is null");
                }
                if (actingPlayer.king == selectedTile.piece && !kingMoveQueue.ContainsKey(playerInitiative)) {
                    //Debug.Log("agrego moviento iniciativa KING"+playerInitiative+ "pieza "+selectedTile.piece.name+ " tile "+tileEnd.tileNumber);   
                    kingMoveQueue.Add(playerInitiative,
                        (selectedTile.piece, endingTile,endingTileMarkingMovement.GetValueOrDefault()));
                }
                else if (!moveQueue.ContainsKey(playerInitiative)) { 
                    //Debug.Log("agrego moviento iniciativa "+playerInitiative+ "pieza "+selectedTile.piece.name+ " tile "+tileEnd.tileNumber);   
                    moveQueue.Add(playerInitiative,
                        (selectedTile.piece, endingTile, endingTileMarkingMovement.GetValueOrDefault()));
                }
                else {
                    throw new Exception("Tried to add a move to the queue with an already added initiative");
                }
                //Debug.Log("1.empiezo corutina");
                actingPlayer.SetInitiative(actingPlayer.initiative + PlayerController.Instance.players.Count);
                yield return StartCoroutine(CheckMoveQueue());
                //Debug.Log("2.termino corutina");
                break;
            default:
                //Debug.Log("No gamemode has been set");
                break;
        }
        // el jugador con la iniciativa con valor mas bajo actua antes, se le suma 1 a este despues de sumar una jugada
        // para que en el proximo movimiento tenga prioridad el otro
        
        
    }

    private IEnumerator CheckMoveQueue() {
        if ( (moveQueue.Count + kingMoveQueue.Count) < 2) {
            yield return null;
        }
        else {
            //Debug.Log(moveQueue.Count +"mc / kc"+kingMoveQueue.Count);
            processingMoves = true;
            // foreach (var VARIABLE in moveQueue) {
            //     Debug.Log("iniciativa "+VARIABLE.Key +" "+VARIABLE.Value.movingPiece.name+" "+VARIABLE.Value.tileEnd.tileNumber);
            // }
             
            List<SortedDictionary<int,(Piece movingPiece, Tile endingTile,(Movement movement, int layer, (int row, int column) offset ) moveData)>> queues =
                new List<SortedDictionary<int, (Piece movingPiece, Tile endingTile, (Movement movement, int layer, (int row, int column) offset) moveData)>>()
                    {kingMoveQueue,moveQueue };
            foreach (var queue in queues) {
                foreach (var move in queue) {
                    yield return new WaitForSeconds(.5f);
                    //ManagerEffects.Instance?.MovePiece(move.Value.movingPiece.transform, move.Value.endingTile);
                    //yield return new WaitWhile(ManagerEffects.Instance?.GetFinishMove());
                    if (move.Value.moveData.movement is null) {
                        throw new Exception("move.Value.moveData. movement is null");
                    } if (move.Value.moveData.movement.movementStencil is null) {
                        throw new Exception("move.Value.moveData. movement. movementstencil is null");
                    }
                    yield return StartCoroutine(ManagerEffects.Instance.Move(move.Value.movingPiece.transform, move.Value.endingTile));
                    ExecuteMove(move.Value.movingPiece,move.Value.endingTile,move.Value.moveData);    

                }
            }
            // al jugador con mas iniciativa se le asigna la iniciativa mas baja (0) para que la proxima ronda vaya primero
            List<PlayerData> sortedPlayerList = PlayerController.Instance.players.OrderByDescending(pD=>pD.initiative).ToList();
            //Debug.Log("cant de jugadores ordenados "+sortedPlayerList.Count);
            for (int playerOrder = 0; playerOrder < sortedPlayerList.Count; playerOrder++) {
                //Debug.Log("seteando iniciativa de jugador "+sortedPlayerList[playerOrder].playerID+" iniciativa "+playerOrder);
                sortedPlayerList[playerOrder].SetInitiative(playerOrder);
                sortedPlayerList[playerOrder].DisplayInitiative(playerOrder);
            }
            moveQueue.Clear();
            kingMoveQueue.Clear();
            processingMoves = false;
            
            
        }
        //Debug.Log("Exiting checkmovequeue");
    }

    public void ExecuteMove(Piece movingPiece, Tile tileEnd, (Movement movement, int layer, (int row, int column) offset) intendedMovementEnd) 
    {
        //chequea si la pieza fue comida antes de su turno
        //if (movingPiece is null) return;
        if (movingPiece == null) return;
        
        //hay que recorrer en los movimientos del marking movement, y vamos seteando ending tile hasta encontrar un tile que permita captura pero corte movimiento despues 
        //tileEnd.markingMovement
        Tile tileStart = FindTileOccupiedByPieceInBoard(movingPiece);

        if (true) {
            if (intendedMovementEnd.movement is null) {
                throw new Exception("intendedmovementend. movement is null");
            } if (intendedMovementEnd.movement.movementStencil is null) {
                throw new Exception("intendedmovementend. movement. movementstencil is null");
            }
            foreach (var stepMovements in intendedMovementEnd.movement.movementStencil.Select((x, i) => new { dictionary = x, layer = i }) ) {
                //stepMovements.layer;
                //startingTile.tileNumber.row
                //startingTile.tileNumber.column
                //si llegamos hasta la capa del intendedMovement, siginifca que el camino estaba vacio de ser un requerimiento
                //Debug.Log("Checking layer "+stepMovements.layer+" == "+intendedMovementEnd.layer +" = "+
                          //(stepMovements.layer == intendedMovementEnd.layer));
                if (stepMovements.layer == intendedMovementEnd.layer) {
                    //no change in ending tile, movement executed as planned
                    break;
                }
                foreach (var content in stepMovements.dictionary)
                {
                    (int row, int column) offset = content.Key;
                    CellCondition cellCondition = content.Value;
                        
                    Tile tileToCheck = GetTile(
                        tileStart.tileNumber.row + offset.row * (movingPiece.InvertMovement() ? -1 : 1),
                        tileStart.tileNumber.column + offset.column * (movingPiece.InvertMovement() ? -1 : 1)
                    );
                    (bool markTilesForMovement,bool markNextLayer,bool addTileToMark) result =
                        intendedMovementEnd.movement.CheckTileForMovement(tileStart.piece, cellCondition, tileToCheck);
                    //si el resultado da que no marquemos la proxima capa, significa que encontramos un obstaculo, y como ya
                    //chequeamos arriba, sabemos que estamos una capa anterior, por lo tanto encontramos una pieza en el camino
                    //y tenemos que terminar el movimiento en la casilla del obstaculo
                    // Debug.Log("Tile"+ (tileStart.tileNumber.row + offset.row,
                    //     tileStart.tileNumber.column + offset.column)+" Mark next layer "+result.markNextLayer);
                    if (!result.markNextLayer) {
                        tileEnd = tileToCheck;
                        break;
                    }
                }

            }
        }

        tileStart.SetPiece(null);

        //ManagerEffects.Instance?.MovePiece(movingPiece.transform, tileEnd);


        Piece capturedPiece = tileEnd.piece;
        movingPiece.amountOfMoves++;
        tileEnd.SetPiece(movingPiece);
        movingPiece.Move(tileEnd);

        if (capturedPiece) {
            movingPiece.Capture(capturedPiece);
            capturedPiece.GetCaptured(movingPiece);
            
                
            if (capturedPiece == PlayerController.Instance.players[0].king) {
                matchEndText.gameObject.SetActive(true);
                matchEndText.text = "BLUE WON";
                StartCoroutine(RestartMatch());
            }
            else if (capturedPiece == PlayerController.Instance.players[1].king) {
                matchEndText.gameObject.SetActive(true);
                matchEndText.text = "RED WON";
                StartCoroutine(RestartMatch());
            }
            Destroy(capturedPiece.gameObject);
        }
    }

    private IEnumerator RestartMatch() {
        yield return new WaitForSeconds(5);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        foreach (var tile in board) {
            Destroy(tile.gameObject);
        }

        board = new Tile[8, 8];
        //Debug.Log("About to reset players " + PlayerController.Instance.players.Count);
        PlayerController.Instance.ResetPlayer(0);
        PlayerController.Instance.ResetPlayer(1);
        PlayerController.Instance.currentPlayer = 0;
        matchEndText.gameObject.SetActive(false);
        turnNumber = 1; 
        Start();
    }

    public Tile FindTileOccupiedByPieceInBoard(Piece pieceToFind) {
        foreach (Tile tile in board) {
            if (tile.piece == pieceToFind) return tile;
        }

        return null;
    }
}

public enum Phase {
    Dawn,Dusk,Night
}
public enum GameMode {
    TurnBased,Simultaneous
}