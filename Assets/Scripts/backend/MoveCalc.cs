using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MoveCalc
{
    public static bool ValidMove(int DestSquareFile, int DestSquareRank, int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure, MonoBehaviour caller)
    {   
        Board board = caller.gameObject.GetComponent<Board>();
        if (board == null){ Debug.Log("Board not correctly recognized"); }
        
        switch (checkField(currentFile, currentRank, boardStructure)) // Get the type information for the to-be-moved piece
        {
            case 10:
                return ValidPawnMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            case 11:
                return ValidBishopMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            case 12:
                return ValidKnightMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            case 13:
                return ValidRookMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            case 14:
                return ValidQueenMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            case 15:
                return ValidKingMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure, board);
            case 20:
                return ValidPawnMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            case 21:
                return ValidBishopMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            case 22:
                return ValidKnightMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            case 23:
                return ValidRookMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            case 24:
                return ValidQueenMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            case 25:
                return ValidKingMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure, board);
            default:
                return false;
        }
    }

    public static bool ValidPawnMove(int DestSquareFile, int DestSquareRank, int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        if (getAllPossiblePawnMoves(currentFile, currentRank, isWhite, boardStructure).Contains((DestSquareFile, DestSquareRank))) {
            return true;
        }
        else {
            return false;
        }
    }

    /*
     * This Method gives a list of tuples that represent all the possible moves a pawn should be able to make from the position (currentFile,currentRank)
     * that he is currently in. 
     * Following the classical rules of chess, this is (for white): 
     * - one forward, if field is free
     * - one forward diagonal, if field is occupied by opponents piece
     * - two forward, if white pawn on 2nd rank or black pawn on 7th rank, if both fields are free
     * 
     * TODO: En Passant
     */
    private static List<(int, int)> getAllPossiblePawnMoves(int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        List<(int, int)> possibleDestinationSquares = new List<(int, int)>(); // A list to store all the possible Squares in

        if (isWhite)
        {
            // One field forward 
            if (checkField(currentFile, currentRank + 1, boardStructure) == 0) { // If Piece Identifier is 0 (null). Meaning that the field is free
                possibleDestinationSquares.Add((currentFile, currentRank + 1));
            }

            // Two fields forward on 2nd rank
            if (currentRank == 2 && checkField(currentFile, currentRank + 1, boardStructure) == 0 && checkField(currentFile, currentRank + 2, boardStructure) == 0)
            {
                possibleDestinationSquares.Add((currentFile, currentRank + 2));
            }

            // Diagonal hit left
            if (currentFile != 1 && checkField(currentFile - 1, currentRank + 1, boardStructure) >= 20) // diagonal front left AND not on the a (1st) rank
            {
                possibleDestinationSquares.Add((currentFile - 1, currentRank + 1));
            }

            // Diagonal hit right
            if (currentFile != 8 && checkField(currentFile + 1, currentRank + 1, boardStructure) >= 20)
            {
                possibleDestinationSquares.Add((currentFile + 1, currentRank + 1));
            }



        }
        else // if piece is black
        {
            // One field forward
            if (checkField(currentFile, currentRank - 1, boardStructure) == 0) {
                possibleDestinationSquares.Add((currentFile, currentRank - 1));
            }

            // Two fields forward on 7th rank
            if (currentRank == 7 && checkField(currentFile, currentRank - 1, boardStructure) == 0 && checkField(currentFile, currentRank - 2, boardStructure) == 0) {
                possibleDestinationSquares.Add((currentFile, currentRank - 2));
            }

            // Diagonal hit left
            if (currentFile != 1 && checkField(currentFile - 1, currentRank - 1, boardStructure) < 20 &&
                                checkField(currentFile - 1, currentRank - 1, boardStructure) >= 10) // diagonal front left AND not on the a (1st) rank
            {
                possibleDestinationSquares.Add((currentFile - 1, currentRank - 1));
            }

            // Diagonal hit right
            if (currentFile != 8 && checkField(currentFile + 1, currentRank - 1, boardStructure) < 20 &&
                                checkField(currentFile + 1, currentRank - 1, boardStructure) >= 10) // diagonal front right AND not on the h (8th) rank
            {
                possibleDestinationSquares.Add((currentFile + 1, currentRank - 1));
            }
        }


        return possibleDestinationSquares;
    }

    public static bool ValidRookMove(int DestSquareFile, int DestSquareRank, int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        if (getAllPossibleRookMoves(currentFile, currentRank, isWhite, boardStructure).Contains((DestSquareFile, DestSquareRank)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
     * This methods gathers all the possible Squares a singular chosen rook is able to move to. This follows certain rules
     * - moving in a straight line on the board 
     * - can only move on empty squares til it "hits" the edges of the board or another piece
     * - if the piece reaches an opponents piece, it can capture it
     */
    private static List<(int, int)> getAllPossibleRookMoves(int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        List<(int, int)> possibleDestinationSquares = new List<(int, int)>(); // A list to store all the possible Squares in


        int tempRankCounter = currentRank;
        // Upwards
        while (tempRankCounter < 8)
        {
            tempRankCounter++;
            int fieldVal = checkField(currentFile, tempRankCounter, boardStructure);
            if (fieldVal == 0) // Square is empty
            {
                possibleDestinationSquares.Add((currentFile, tempRankCounter));
            }
            else if ((fieldVal >= 20 && isWhite) || (fieldVal > 0 && fieldVal < 20 && !isWhite)) // Square has black Piece and rook is white OR Square has white Piece and rook is black
            {
                possibleDestinationSquares.Add((currentFile, tempRankCounter));
                break;
            } else if ((fieldVal > 0 && fieldVal < 20 && isWhite) || (fieldVal >= 20 && !isWhite)) // Square has white piece and rook is white OR Square has black Piece and rook is black
            {
                break;
            }
        }

        // Sideways left
        int tempFileCounter = currentFile;
        while (tempFileCounter > 1)
        {
            tempFileCounter--;
            int fieldVal = checkField(tempFileCounter, currentRank, boardStructure);
            if (fieldVal == 0) // Square is empty
            {
                possibleDestinationSquares.Add((tempFileCounter, currentRank));
            }
            else if ((fieldVal >= 20 && isWhite) || (fieldVal > 0 && fieldVal < 20 && !isWhite)) // Square has black Piece
            {
                possibleDestinationSquares.Add((tempFileCounter, currentRank));
                break;
            }
            else if ((fieldVal > 0 && fieldVal < 20 && isWhite) || (fieldVal >= 20 && !isWhite)) // Square has white Piece
            {
                break;
            }
        }

        // Sideways right
        tempFileCounter = currentFile;
        while (tempFileCounter < 8)
        {
            tempFileCounter++;
            int fieldVal = checkField(tempFileCounter, currentRank, boardStructure);
            if (fieldVal == 0) // Square is empty
            {
                possibleDestinationSquares.Add((tempFileCounter, currentRank));
            }
            else if ((fieldVal >= 20 && isWhite) || (fieldVal > 0 && fieldVal < 20 && !isWhite)) // Square has black Piece
            {
                possibleDestinationSquares.Add((tempFileCounter, currentRank));
                break;
            }
            else if ((fieldVal > 0 && fieldVal < 20 && isWhite) || (fieldVal >= 20 && !isWhite)) // Square has white Piece
            {
                break;
            }
        }

        // Downwards
        tempRankCounter = currentRank;
        while (tempRankCounter > 1)
        {
            tempRankCounter--;
            int fieldVal = checkField(currentFile, tempRankCounter, boardStructure);
            if (fieldVal == 0) // Square is empty
            {
                possibleDestinationSquares.Add((currentFile, tempRankCounter));
            }
            else if ((fieldVal >= 20 && isWhite) || (fieldVal > 0 && fieldVal < 20 && !isWhite)) // Square has black Piece
            {
                possibleDestinationSquares.Add((currentFile, tempRankCounter));
                break;
            }
            else if ((fieldVal > 0 && fieldVal < 20 && isWhite) || (fieldVal >= 20 && !isWhite)) // Square has white Piece
            {
                break;
            }
        }
        Debug.Log("There were " + possibleDestinationSquares.Count + " possible moves for this rook");
        return possibleDestinationSquares;
    }

    public static bool ValidKnightMove(int DestSquareFile, int DestSquareRank, int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        if (getAllPossibleKnightMoves(currentFile, currentRank, isWhite, boardStructure).Contains((DestSquareFile, DestSquareRank)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
     * This method gives all of the possible max. 8 moves a singular knight on a given position can perform
     * - moves in an "L shape", either two upwards and on sideways or two sideways and one upwards
     * - can move if square is empty or occupied by enemy piece
     * each direction is named as a compass point for comprehension (e.g. SSW would be two sideways to the left and one down)
     */
    private static List<(int, int)> getAllPossibleKnightMoves(int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        List<(int, int)> possibleDestinationSquares = new List<(int, int)>(); // A list to store all the possible Squares in

        List<(int, int)> directions = new List<(int, int)>
        {
            (1,2), (2,1), (2,-1), (1, -2), (-1, -2), (-2, -1), (-2, 1),(-1, 2) // Clockwise movement around the knights position. Starting North-East-North
        };

        Dictionary<(int, int), int> fieldVars = new Dictionary<(int, int), int>();

        foreach ((int, int) direction in directions) {
            fieldVars.Add((currentFile + direction.Item1, currentRank + direction.Item2), checkField(currentFile + direction.Item1, currentRank + direction.Item2, boardStructure));
        }

        foreach (KeyValuePair<(int, int), int> fieldVar in fieldVars) {
            if (fieldVar.Value == 0 ||      // Square is empty
                (fieldVar.Value >= 20) && isWhite ||    // Square is occupied by black piece and knight is white
                 fieldVar.Value < 20 && fieldVar.Value >= 10 && !isWhite) // Square is occupied by white piece and knight is black
            {
                possibleDestinationSquares.Add((fieldVar.Key.Item1, fieldVar.Key.Item2));
            }
        }
        Debug.Log("There were " + possibleDestinationSquares.Count + " possible moves for this knight");
        return possibleDestinationSquares;
    }

    public static bool ValidBishopMove(int DestSquareFile, int DestSquareRank, int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        if (getAllPossibleBishopMoves(currentFile, currentRank, isWhite, boardStructure).Contains((DestSquareFile, DestSquareRank)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /*
     * Method gives a list for all possible moves a singular bishop is able to do. It follows a similar pattern to the rook.
     * - can move diagonally indefinetly in every direction for every free square
     * - if encounters an opponents piece, it can capture
     */
    private static List<(int, int)> getAllPossibleBishopMoves(int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        List<(int, int)> possibleDestinationSquares = new List<(int, int)>(); // A list to store all the possible Squares in

        int tempRankCounter = currentRank;
        int tempFileCounter = currentFile;

        // Diagonal top left
        while (tempRankCounter < 8 || tempFileCounter > 1)
        {
            tempFileCounter--;
            tempRankCounter++;

            int fieldVal = checkField(tempFileCounter, tempRankCounter, boardStructure);
            if (fieldVal == 0) // Square is empty
            {
                possibleDestinationSquares.Add((tempFileCounter, tempRankCounter));
            }
            else if ((!isWhite && fieldVal >= 10 && fieldVal < 20) || (isWhite && fieldVal >= 20)) // bishop is black and square has white piece OR white bishop and black opp. piece
            {
                possibleDestinationSquares.Add((tempFileCounter, tempRankCounter));
                break;
            }
            else if ((isWhite && fieldVal >= 10 && fieldVal < 20) || (!isWhite && fieldVal >= 20)) // bishop is white and piece on square is white OR bishop black and square black
            {
                break;
            }
        }

        tempRankCounter = currentRank;
        tempFileCounter = currentFile;

        // Diagonal top right
        while (tempRankCounter < 8 || tempFileCounter < 8)
        {
            tempFileCounter++;
            tempRankCounter++;

            int fieldVal = checkField(tempFileCounter, tempRankCounter, boardStructure);
            if (fieldVal == 0) // Square is empty
            {
                possibleDestinationSquares.Add((tempFileCounter, tempRankCounter));
            }
            else if ((!isWhite && fieldVal >= 10 && fieldVal < 20) || (isWhite && fieldVal >= 20)) // bishop is black and square has white piece OR white bishop and black opp. piece
            {
                possibleDestinationSquares.Add((tempFileCounter, tempRankCounter));
                break;
            }
            else if ((isWhite && fieldVal >= 10 && fieldVal < 20) || (!isWhite && fieldVal >= 20)) // bishop is white and piece on square is white OR bishop black and square black
            {
                break;
            }
        }

        tempRankCounter = currentRank;
        tempFileCounter = currentFile;
        // Diagonal bottom left
        while (tempRankCounter > 1 || tempFileCounter > 1)
        {
            tempFileCounter--;
            tempRankCounter--;

            int fieldVal = checkField(tempFileCounter, tempRankCounter, boardStructure);
            if (fieldVal == 0) // Square is empty
            {
                possibleDestinationSquares.Add((tempFileCounter, tempRankCounter));
            }
            else if ((!isWhite && fieldVal >= 10 && fieldVal < 20) || (isWhite && fieldVal >= 20)) // bishop is black and square has white piece OR white bishop and black opp. piece
            {
                possibleDestinationSquares.Add((tempFileCounter, tempRankCounter));
                break;
            }
            else if ((isWhite && fieldVal >= 10 && fieldVal < 20) || (!isWhite && fieldVal >= 20)) // bishop is white and piece on square is white OR bishop black and square black
            {
                break;
            }
        }

        tempRankCounter = currentRank;
        tempFileCounter = currentFile;
        // Diagonal bottom right
        while (tempRankCounter > 1 || tempFileCounter < 8)
        {
            tempFileCounter++;
            tempRankCounter--;

            int fieldVal = checkField(tempFileCounter, tempRankCounter, boardStructure);
            if (fieldVal == 0) // Square is empty
            {
                possibleDestinationSquares.Add((tempFileCounter, tempRankCounter));
            }
            else if ((!isWhite && fieldVal >= 10 && fieldVal < 20) || (isWhite && fieldVal >= 20)) // bishop is black and square has white piece OR white bishop and black opp. piece
            {
                possibleDestinationSquares.Add((tempFileCounter, tempRankCounter));
                break;
            }
            else if ((isWhite && fieldVal >= 10 && fieldVal < 20) || (!isWhite && fieldVal >= 20)) // bishop is white and piece on square is white OR bishop black and square black
            {
                break;
            }
        }


        Debug.Log("There were " + possibleDestinationSquares.Count + " possible moves for this bishop");
        return possibleDestinationSquares;
    }

    public static bool ValidQueenMove(int DestSquareFile, int DestSquareRank, int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        if (getAllPossibleQueenMoves(currentFile, currentRank, isWhite, boardStructure).Contains((DestSquareFile, DestSquareRank)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private static List<(int, int)> getAllPossibleQueenMoves(int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        List<(int, int)> possibleDestinationSquares = new List<(int, int)>(); // A list to store all the possible Squares in

        Debug.Log("Asking for possible rook and bishop moves to calculate queen moves...");

        possibleDestinationSquares.AddRange(getAllPossibleBishopMoves(currentFile, currentRank, isWhite, boardStructure));
        possibleDestinationSquares.AddRange(getAllPossibleRookMoves(currentFile, currentRank, isWhite, boardStructure));

        Debug.Log("There were " + possibleDestinationSquares.Count + " possible moves for this queen");

        return possibleDestinationSquares;
    }
    public static bool ValidKingMove(int DestSquareFile, int DestSquareRank, int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure, Board board)
    {
        if (getAllPossibleKingMoves(currentFile, currentRank, isWhite, boardStructure, board).Contains((DestSquareFile, DestSquareRank)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
     * This method gives all the possible King moves. 
     * These are a little more specific 
     * - King can move into any direction around him (8 squares).
     * - King cannot move into check (a square that is guarded by an opponents piece).
     * - King can castle with either rook on each side (king always moves two sideways and rook "jump" over him). 
     */
    private static List<(int, int)> getAllPossibleKingMoves(int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure, Board board)
    {
        List<(int, int)> possibleDestinationSquares = new List<(int, int)>(); // A list to store all the possible Squares in
        
        // Handling of the 8 squares around the king
        List<(int, int)> directions = new List<(int, int)>
        {
            (0,1), (1,1), (1,0), (1,-1), (0,-1), (-1, -1), (-1, 0),(-1, 1) // Clockwise movement around the kings position. Starting North
        };
        Dictionary<(int, int), int> fieldVars = new Dictionary<(int, int), int>();
        foreach ((int, int) direction in directions)
        {
            fieldVars.Add((currentFile + direction.Item1, currentRank + direction.Item2), checkField(currentFile + direction.Item1, currentRank + direction.Item2, boardStructure));
        }
        foreach (KeyValuePair<(int, int), int> fieldVar in fieldVars)
        {
            if (fieldVar.Value == 0 ||      // Square is empty
                (fieldVar.Value >= 20) && isWhite ||    // Square is occupied by black piece and knight is white
                 fieldVar.Value < 20 && fieldVar.Value >= 10 && !isWhite) // Square is occupied by white piece and knight is black
            {
                possibleDestinationSquares.Add((fieldVar.Key.Item1, fieldVar.Key.Item2));
            }
        }

        // Handling of the castling moves
        if (isWhite && checkField(5, 1, boardStructure) == 15)
        {
            
            if (!board.GetWhiteKingHasMoved() && !board.GetWhiteRookOneHasMoved())
            {
                
                if (checkField(1, 1, boardStructure) == 13 && checkField(2, 1, boardStructure) == 0 && checkField(3, 1, boardStructure) == 0 && checkField(4, 1, boardStructure) == 0) // Long Castle White king
                {
                    possibleDestinationSquares.Add((3, 1));
                }

            }
            if (!board.GetWhiteKingHasMoved() && !board.GetWhiteRookTwoHasMoved())
            {
                if (checkField(8, 1, boardStructure) == 13 && checkField(7, 1, boardStructure) == 0 && checkField(6, 1, boardStructure) == 0)
                {
                    possibleDestinationSquares.Add((7, 1));
                }
            }
        }
        if (!isWhite && checkField(5, 8, boardStructure) == 25)
        {
            if (!board.GetBlackKingHasMoved() && !board.GetBlackRookOneHasMoved()){
                if (checkField(1, 8, boardStructure) == 23 && checkField(2, 8, boardStructure) == 0 && checkField(3, 8, boardStructure) == 0 && checkField(4, 8, boardStructure) == 0) // Long Castle Black king
                {
                    possibleDestinationSquares.Add((3, 8));
                }
            } 
            if (!board.GetBlackKingHasMoved() && !board.GetBlackRookTwoHasMoved()) 
            {
                if (checkField(8, 8, boardStructure) == 23 && checkField(7, 8, boardStructure) == 0 && checkField(6, 8, boardStructure) == 0)
                {
                    possibleDestinationSquares.Add((7, 8));
                }
            }
        }


        Debug.Log("There were " + possibleDestinationSquares.Count + " possible moves for this king");
        return possibleDestinationSquares;
    }

    private void UpdateCastlingInformation()
    {

    }

    /*
        * Function returns Piece Identifier
        * 
        * white pawn: 10       black pawn: 21
        * white bishop: 11     black bishop: 21
        * white knight: 12     black knight: 22
        * white rook: 13       black rook: 23
        * white queen: 14      black queen: 24
        * white king: 15       black king: 25
        */
    private static int checkField(int file, int rank, Dictionary<(int, int), int> boardStructure)
    {
        if (file < 1 || file > 8 || rank < 1 || rank > 8)
        {
            //Debug.Log("Out of Board Bounds");
            return -1;
        }
        return boardStructure[(file, rank)];
    }

}