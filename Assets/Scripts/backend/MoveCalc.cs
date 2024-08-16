using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MoveCalc 
{
    public static bool ValidMove(int DestSquareFile, int DestSquareRank, int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        switch(checkField(currentFile, currentRank, boardStructure)) // Get the type information for the to-be-moved piece
        {
            case 10:
                return ValidPawnMove(DestSquareFile,DestSquareRank,currentFile,currentRank,isWhite,boardStructure);
            case 11:
                return ValidBishopMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            case 12:
                return ValidKnightMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            case 13:
                return ValidRookMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            case 14:
                return ValidQueenMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            case 15:
                return ValidKingMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
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
                return ValidKingMove(DestSquareFile, DestSquareRank, currentFile, currentRank, isWhite, boardStructure);
            default:
                return false;
        }
    }

    public static bool ValidPawnMove(int DestSquareFile, int DestSquareRank, int currentFile, int currentRank, bool isWhite, Dictionary<(int,int), int> boardStructure)
    {
        if(getAllPossiblePawnMoves(currentFile,currentRank,isWhite,boardStructure).Contains((DestSquareFile, DestSquareRank))){
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
    private static List<(int,int)> getAllPossiblePawnMoves(int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        List<(int, int)> possibleDestinationSquares = new List<(int, int)>(); // A list to store all the possible Squares in

        if (isWhite)
        {
            // One field forward 
            if(checkField(currentFile, currentRank + 1, boardStructure) == 0) { // If Piece Identifier is 0 (null). Meaning that the field is free
                possibleDestinationSquares.Add((currentFile, currentRank + 1));
            }

            // Two fields forward on 2nd rank
            if (currentRank == 2 && checkField(currentFile, currentRank + 1, boardStructure)==0 && checkField(currentFile, currentRank + 2, boardStructure)==0)
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
            if(checkField(currentFile, currentRank -1, boardStructure) == 0) { 
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
        while(tempRankCounter < 8)
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
            }else if ((fieldVal > 0 && fieldVal < 20 && isWhite ) || (fieldVal >= 20 && !isWhite)) // Square has white piece and rook is white OR Square has black Piece and rook is black
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

        
        Debug.Log(possibleDestinationSquares.Count);
        foreach (var possibleDestinationSquare in possibleDestinationSquares)
        {
            Debug.Log(possibleDestinationSquare);
        }

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

    private static List<(int, int)> getAllPossibleKnightMoves(int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        List<(int, int)> possibleDestinationSquares = new List<(int, int)>(); // A list to store all the possible Squares in
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
    private static List<(int, int)> getAllPossibleBishopMoves(int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        List<(int, int)> possibleDestinationSquares = new List<(int, int)>(); // A list to store all the possible Squares in
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
        return possibleDestinationSquares;
    }
    public static bool ValidKingMove(int DestSquareFile, int DestSquareRank, int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        if (getAllPossibleKingMoves(currentFile, currentRank, isWhite, boardStructure).Contains((DestSquareFile, DestSquareRank)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private static List<(int, int)> getAllPossibleKingMoves(int currentFile, int currentRank, bool isWhite, Dictionary<(int, int), int> boardStructure)
    {
        List<(int, int)> possibleDestinationSquares = new List<(int, int)>(); // A list to store all the possible Squares in
        return possibleDestinationSquares;
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
        if(file < 1 || file > 8 || rank < 1 || rank > 8)
        {
            Debug.Log("Out of Board Bounds");
            return -1;
        }
        return boardStructure[(file, rank)];
    }


}
