using System;
using System.Collections;
using System.Collections.Generic;
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

    public delegate void TileSelected(bool selected);

    public static event TileSelected OnTileSelected;
    private Tile[,] board = new Tile[8, 8];
    private Row[] boardInRows = new Row[8];
    private Tile currentlySelectedTile;
    private static BoardManager instance;
    public TextMeshProUGUI matchEndText;

    public int turnNumber = 1;

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
                        // case 1:	
                        //     piece = PieceLibrary.Knight();
                        //     break;
                        // case 2:	
                        //     piece = PieceLibrary.Bishop();
                        //     break;
                        // case 3:	
                        //     piece = PieceLibrary.King();
                        //     break;
                        // case 4:	
                        //     piece = PieceLibrary.Queen();
                        //     break;
                        // case 5:	
                        //     piece = PieceLibrary.Bishop();
                        //     break;
                        // case 6:	
                        //     piece = PieceLibrary.Knight();
                        //     break;
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
        currentlySelectedTile = selectedTile;
        OnTileSelected?.Invoke(false);
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

    public void ExecuteMove(Tile endingTile) {
        Piece movingPiece = currentlySelectedTile.piece;
        currentlySelectedTile.SetPiece(null);

        Piece capturedPiece = endingTile.piece;

        endingTile.SetPiece(movingPiece);

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

        SetSelectedTile(null);
        turnNumber++;
        //UpdateBoard();
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
        matchEndText.gameObject.SetActive(false);
        Start();
    }
}