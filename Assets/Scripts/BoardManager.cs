using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

    public SortedDictionary<int,(Piece movingPiece, Tile endingTile)> moveQueue = new SortedDictionary<int, (Piece movingPiece, Tile endingTile)>();
    public SortedDictionary<int,(Piece movingPiece, Tile endingTile)> kingMoveQueue = new SortedDictionary<int, (Piece movingPiece, Tile endingTile)>();
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
        Debug.Log("king 0 " + randomPieceCrownedKing + " between 0 and " + PlayerController.Instance.players[0]
            .piecesOwnedByPlayer.Count);
        if (PlayerController.Instance.players[0].king is null) {
            PlayerController.Instance.players[0].SetKing(
                PlayerController.Instance.players[0]
                    .piecesOwnedByPlayer[randomPieceCrownedKing]
            );
        }

        randomPieceCrownedKing = Random.Range(0, PlayerController.Instance.players[1]
            .piecesOwnedByPlayer.Count);
        Debug.Log("king 1 " + randomPieceCrownedKing + " between 0 and " + PlayerController.Instance.players[1]
            .piecesOwnedByPlayer.Count);
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
        SetSelectedTile(null);
        switch (gamemode) {
            case GameMode.TurnBased:
                ExecuteMove(selectedTile.piece,endingTile);
                break;
            case GameMode.Simultaneous:
                int playerInitiative = actingPlayer.initiative;
                if (actingPlayer.king == selectedTile.piece && !kingMoveQueue.ContainsKey(playerInitiative)) {
                    //Debug.Log("agrego moviento iniciativa KING"+playerInitiative+ "pieza "+selectedTile.piece.name+ " tile "+tileEnd.tileNumber);   
                    kingMoveQueue.Add(playerInitiative,
                        (selectedTile.piece, endingTile));
                }
                else if (!moveQueue.ContainsKey(playerInitiative)) { 
                    //Debug.Log("agrego moviento iniciativa "+playerInitiative+ "pieza "+selectedTile.piece.name+ " tile "+tileEnd.tileNumber);   
                    moveQueue.Add(playerInitiative,
                        (selectedTile.piece, endingTile));
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
            List<SortedDictionary<int, (Piece movingPiece, Tile endingTile)>> queues =
                new List<SortedDictionary<int, (Piece movingPiece, Tile endingTile)>>()
                    {kingMoveQueue,moveQueue };
            foreach (var queue in queues) {
                foreach (var move in queue) {
                    yield return new WaitForSeconds(.5f);
                    ExecuteMove(move.Value.movingPiece,move.Value.endingTile);    
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
        Debug.Log("Exiting checkmovequeue");
    }

    public void ExecuteMove(Piece movingPiece, Tile tileEnd) {
        //chequea si la pieza fue comida antes de su turno
        //if (movingPiece is null) return;
        if (movingPiece == null) return;
        
        //hay que recorrer en los movimientos del marking movement, y vamos seteando ending tile hasta encontrar un tile que permita captura pero corte movimiento despues 
        //tileEnd.markingMovement
        Tile tileStart = FindTileOccupiedByPieceInBoard(movingPiece);
        foreach (var stepMovements in tileEnd.markingMovement.movementStencil){
                //startingTile.tileNumber.row
                //startingTile.tileNumber.column
                    /*stepMovements.ContainsKey((
                        startingTile.tileNumber.row - tileEnd.tileNumber.row,
                        startingTile.tileNumber.column - tileEnd.tileNumber.column
                        ))*/
                    foreach (var content in stepMovements)
                    {
                        (int row, int column) offset = content.Key;
                        CellCondition cellCondition = content.Value;
                        
                        Tile tileToCheck = BoardManager.Instance.GetTile(
                            tileStart.tileNumber.row + offset.row * (movingPiece.InvertMovement() ? -1 : 1),
                            tileStart.tileNumber.column + offset.column * (movingPiece.InvertMovement() ? -1 : 1)
                        );
                    }

        }
        tileStart?.SetPiece(null);
        
        Piece capturedPiece = tileEnd.piece;
        movingPiece.amountOfMoves++;
        tileEnd.SetPiece(movingPiece);

        if (capturedPiece) {
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
        Debug.Log("About to reset players " + PlayerController.Instance.players.Count);
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