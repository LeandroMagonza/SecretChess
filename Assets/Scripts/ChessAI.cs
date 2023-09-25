using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

// keeps track of game logic and chess rules
public class ChessAI : MonoBehaviour {


    private Tile[,] testingBoard = new Tile[8, 8];

    /*
     * 
     *   GAME LOGIC
     * 
     * 
     */

    const int PLAYER = -1;
    const int NEUTRAL = 0;
    const int AI = 1;
    
    // the piece the player has selected to move.
    // the piece goes FROM here TO somewhere else
    private (int row,int column) from;

    // handles the given posn being clicked
   
    // sets a move and updates it's tiles

    // public void UpdateTile((int row, int column) posn)
    // {
    //     testingBoard[posn.row, posn.column].SetPiece(GetPiece(posn));
    // }
    

    // returns every move the given team can make
    List<Move> AllMoves(int team) {
        List<Move> moves = new List<Move>();
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                (int row,int column) from = (x, y);
                if (WhatTeam(from) == team)
                {
                    moves.AddRange(MovesFromPosn(from));
                }
            }
        }
        return moves;
    }

    // returns all the moves from the given posn
    List<Move> MovesFromPosn ((int row,int column) from)
    {
        throw new Exception("called uninmplemente chess ai method");
        //return GetPiece(from)?.movementPattern.GetAllMoves(from);
    }

    // changes the board state to reflect the given move being played
    public void SetMove(Move move)
    {
        // if (move.HasSpecialRule())
        // {
        //     if (move.rule.Equals("CASTLE"))
        //     {
        //         int y = WhatTeam(move.from) == PLAYER ? 0 : 7;
        //         SetPiece(move.to, GetPiece(move.from));
        //         SetPiece(move.from, EMPTY);
        //         Posn from, to;
        //         if (move.to.x < move.from.x)
        //         {
        //             from = new Posn(0, y);
        //             to = new Posn(3, y);
        //         }
        //         else
        //         {
        //             from = new Posn(7, y);
        //             to = new Posn(5, y);
        //         }
        //         SetPiece(to, GetPiece(from));
        //         SetPiece(from, EMPTY);
        //     }
        //     else // PROMOTION
        //     {
        //         int team = WhatTeam(move.from);
        //         SetPiece(move.to, QUEEN * team);
        //         SetPiece(move.from, EMPTY);
        //         value += (QUEEN - PAWN) * team;
        //         value -= move.removedPiece;
        //     }
        // }
        // else
        // {
        Piece piece = GetPiece(move.from);
        SetPiece(move.from, null);
        SetPiece(move.to, piece);
        value -= move.capturedPiece.value * OwnerToTeam(move.capturedPiece.GetOwnerID());
        // }
    }

    // changes the board state to reflect the given move being undone
    public void UndoMove(Move move)
    {
        SetPiece(move.from, GetPiece(move.to));
        SetPiece(move.to, move.capturedPiece);
        value += move.capturedPiece.value*OwnerToTeam(move.capturedPiece.GetOwnerID());
    }

    /*
     * 
     * 
     *   BASIC HELPER METHODS
     * 
     * 
     */
//
    public int OwnerToTeam(int owner) {
        if (owner == 0) {
            return 1;
        }
        else if (owner == 1) {
            return -1;
        }
        else {
            return 0;
        }
    }

    // returns which team a given posn belongs to
    public int WhatTeam((int row,int column) posn)
    {
        return WhatTeam(GetPiece(posn));
    }

    // returns which team a given int belongs to
    public int WhatTeam(Piece piece) {
        return (piece is not null) ? OwnerToTeam(piece.GetOwnerID()):0;
    }

    // returns the piece at the given posn
    public Piece GetPiece((int row,int column) posn)
    {
        return testingBoard[posn.column, posn.row].piece;
    }

    // sets the given piece at the given posn
    public void SetPiece((int row,int column) posn, Piece piece)
    {
        testingBoard[posn.column, posn.row].SetPiece(piece);
    }

    // is the piece at the given posn an enemy of the given team?
    // bool IsEnemy((int row,int column) posn, int team)
    // {
    //     return WhatTeam(posn) + team == 0;
    // }
    //
    // // is there no piece at the given posn?
    // bool IsEmpty((int row,int column) posn)
    // {
    //     return WhatTeam(posn) == NEUTRAL;
    // }
    //
    // // is the given posn on the map?
    // bool IsOnMap((int row,int column) posn)
    // {
    //     return posn.column >= 0 && posn.column < 8 && posn.row >= 0 && posn.row < 8;
    // }

    // has someone won the game?
    public bool GameOver()
    {
        return Winner() != NEUTRAL;
    }

    // returns the current winner.
    // if a king has not been taken, NEUTRAL is returned
    public int Winner()
    {
        if (Mathf.Abs(value) > WINSCORE)
        {
            return value < 0 ? PLAYER : AI;
        }
        else
        {
            return NEUTRAL;
        }
    }


    /*
     * 
     * 
     *   PIECE MOVES
     * 
     * 
     */

    // a list of moves that the pawn at the given posn can make
    // List<Move> PawnMoves((int row,int column) from, int team)
    // {
    //     int x = from.column;
    //     int y = from.row;
    //     List<Move> moves = new List<Move>();
    //     (int row,int column) to = new (x, y - team);
    //     if (IsEmpty(to))
    //     {
    //
    //             AddMoveIf(from, to, moves, true);
    //   
    //         if ((y + team) % 7 == 0) {
    //             to = (x, y - team * 2);
    //             AddMoveIf(from, to, moves, IsEmpty(to));
    //         }
    //     }
    //     (int row,int column) to1 = (x - 1, y - team);
    //     (int row,int column) to2 = (x + 1, y - team);
    //     if ((y - team) % 7 == 0)
    //     {
    //         if (x != 0 && IsEnemy(to1, team))
    //         {
    //             moves.Add(new Move(from, to1, GetPiece(to1), "PROMOTION"));
    //         }
    //         if (x != 7 && IsEnemy(to2, team))
    //         {
    //             moves.Add(new Move(from, to2, GetPiece(to2), "PROMOTION"));
    //         }
    //     }
    //     else
    //     {
    //         AddMoveIf(from, to1, moves, x != 0 && IsEnemy(to1, team));
    //         AddMoveIf(from, to2, moves, x != 7 && IsEnemy(to2, team));
    //     }
    //     return moves;
    // }
    //
    // //a list of moves that the knight at the given posn can make
    // List<Move> KnightMoves((int row,int column) from, int team)
    // {
    //     List<Move> moves = new List<Move>();
    //     int x = from.column;
    //     int y = from.row;
    //     for (int i = -2; i <= 2; i++)
    //     {
    //         for (int j = 3 - Mathf.Abs(i); j > -3; j -= 2)
    //         {
    //             if (i == 0 || j == 0)
    //                 continue;
    //             (int row,int column) to = (x + i, y + j);
    //             AddMoveIf(from, to, moves, IsOnMap(to) && WhatTeam(to) != team);
    //         }
    //     }
    //     return moves;
    // }
    //
    // //a list of moves that the bishop at the given posn can make
    // List<Move> BishopMoves((int row,int column) from, int team)
    // {
    //     List<Move> moves = new List<Move>();
    //     AddMovesInDirection(1, 1, moves, from, team);
    //     AddMovesInDirection(1, -1, moves, from, team);
    //     AddMovesInDirection(-1, -1, moves, from, team);
    //     AddMovesInDirection(-1, 1, moves, from, team);
    //     return moves;
    // }
    //
    // //a list of moves that the rook at the given posn can make
    // List<Move> RookMoves((int row,int column) from, int team)
    // {
    //     List<Move> moves = new List<Move>();
    //     AddMovesInDirection(0, 1, moves, from, team);
    //     AddMovesInDirection(1, 0, moves, from, team);
    //     AddMovesInDirection(0, -1, moves, from, team);
    //     AddMovesInDirection(-1, 0, moves, from, team);
    //     return moves;
    // }
    //
    // //a list of moves that the queen at the given posn can make
    // List<Move> QueenMoves((int row,int column) from, int team)
    // {
    //     List<Move> moves = BishopMoves(from, team);
    //     moves.AddRange(RookMoves(from, team));
    //     return moves;
    // }
    //
    // //a list of moves that the king at the given posn can make
    // List<Move> KingMoves((int row,int column) from, int team)
    // {
    //     List<Move> moves = new List<Move>();
    //     int x = from.column;
    //     int y = from.row;
    //     for (int i = -1; i <= 1; i++)
    //     {
    //         for (int j = -1; j <= 1; j++)
    //         {
    //             if (i == 0 && j == 0)
    //                 continue;
    //             (int row,int column) to = (x + i, y + j);
    //             AddMoveIf(from, to, moves, (IsOnMap(to) && WhatTeam(to) != team));
    //         }
    //     }
    //     // castling
    //     if (x == 4 && y == (team == AI ? 7 : 0))
    //     {
    //         if (testingBoard[0, y] == ROOK * team
    //             && testingBoard[1, y] == EMPTY
    //             && testingBoard[2, y] == EMPTY
    //             && testingBoard[3, y] == EMPTY)
    //         {
    //             moves.Add(new Move(from, (x - 2, y), EMPTY, "CASTLE"));
    //         }
    //         if (testingBoard[7, y] == ROOK * team
    //             && testingBoard[6, y] == EMPTY
    //             && testingBoard[5, y] == EMPTY)
    //         {
    //             moves.Add(new Move(from, (x + 2, y), EMPTY, "CASTLE"));
    //         }
    //     }
    //     return moves;
    // }

    // adds every empty/enemy space in the given (x,y) direction as a move
    // void AddMovesInDirection(int xDir, int yDir, List<Move> moves, (int row,int column) from, int team)
    // {
    //     (int row,int column) to = (from.column + xDir, from.row + yDir);
    //     while (IsOnMap(to))
    //     {
    //         if (!IsEmpty(to))
    //         {
    //             AddMoveIf(from, to, moves, IsEnemy(to, team));
    //             return;
    //         }
    //         AddMoveIf(from, to, moves, true);
    //         to = (to.column + xDir, to.row + yDir);
    //     }
    // }

    // adds a move to moves if the given condition is true
    // void AddMoveIf((int row,int column) from, (int row,int column) to, List<Move> moves, bool condition)
    // {
    //     if (condition)
    //     {
    //         moves.Add(new Move(from, to, GetPiece(to)));
    //     }
    // }

    /*
     * 
     *   AI METHODS
     * 
     * 
     */

    //the value of the board
    int value;

    public Slider difficultySlider;
    private int turnLookahead;

    public float switchOdds;

    const int WINSCORE = 5000;
    const int MINALPHA = -5001;

    // changes the turn lookahead based on the difficulty slider
    public void ChangeDifficulty()
    {
        turnLookahead = (int)Mathf.Clamp(difficultySlider.value, 1, 5);
    }

    //handles the ai's turn
    public IEnumerator AITurn()
    {
        yield return new WaitForSeconds(0);
        GetAIMove();
    }
    
    // returns the next move for the AI
    public Move GetAIMove()
    {
        Move bestMove = null;
        int alpha = MINALPHA;
        foreach (Move move in AllMoves(AI))
        {
            SetMove(move);
            int score = GetMoveValue(move, AI, alpha, turnLookahead - 1);
            UndoMove(move);
            if (score > alpha || (score == alpha && Random.value < switchOdds))
            {
                alpha = score;
                bestMove = move;
            }
        }
        return bestMove;
    }

    // returns the heuristic value of the given move
    int GetMoveValue(Move move, int team, int alpha, int beta)
    {
        if (beta <= 0)
            return value;
        if (GameOver())
            return Winner() * WINSCORE;
        int enemyBestMove = MINALPHA * -team;
        foreach (Move nextMove in AllMoves(-team))
        {
            SetMove(nextMove);
            int score = GetMoveValue(nextMove, -team, alpha, beta - 1);
            UndoMove(nextMove);
            if (team == AI && score < alpha)
                return alpha - 1;
            enemyBestMove = team == AI ?
                Mathf.Min(enemyBestMove, score) :
                Mathf.Max(enemyBestMove, score);
        }
        return enemyBestMove;
    }
    
}

// posns represent board positions
// public class Posn
// {
//     public int x;
//     public int y;
//
//     public Posn(int x, int y)
//     {
//         this.x = x;
//         this.y = y;
//     }
//
//     public bool Equals(Posn posn)
//     {
//         return this.x == posn.x && this.y == posn.y;
//     }
// }

// moves represent a piece moving FROM one posn TO another posn
public class Move
{
    public (int row,int column) from;
    public (int row,int column) to;

    // remembers the piece that will be destroyed if this move goes through
    public Piece capturedPiece;

    public Move((int row,int column) from, (int row,int column) to, Piece capturedPiece)
    {
        this.from = from;
        this.to = to;
        this.capturedPiece = capturedPiece;
    }

}