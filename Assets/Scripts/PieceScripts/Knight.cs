using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece, PieceInterface
{
    public void HandleMovedPiece(Field targetField)
    {
        GameObject currentField = myMoveable.getCurrentField();

        if (GetAllPossibleKnightMoves(currentField).Contains(targetField))
        {
            ExecuteMove(targetField);
        }
        else
        {
            myMoveable.ResetPosition();
        }
    }

    public List<Field> GetAllPossibleKnightMoves(GameObject currentField)
    {
        List<Field> possibleFields = new List<Field>();

        Board board = myMoveable.board;
        int file = currentField.GetComponent<Field>().getFile();
        int rank = currentField.GetComponent<Field>().getRank();

        List<(int, int)> destinationSquareCoordinates = new List<(int, int)>
        {
            // 1 clock 
            (file + 1, rank + 2),
            // 2 clock
            (file + 2, rank + 1),
            // 4 clock
            (file + 2, rank - 1),
            // 5 clock
            (file + 1, rank - 2),
            // 7 clock
            (file - 1, rank - 2),
            // 8 clock
            (file - 2, rank - 1),
            // 10 clock
            (file - 2, rank + 1),
            // 11 clock
            (file - 1, rank + 2),
        };

        foreach (var pair in destinationSquareCoordinates)
        {
            if (board.FindSquareByCoordinates(pair.Item1, pair.Item2) != null)
            {
                Field field = board.FindSquareByCoordinates(pair.Item1, pair.Item2);
                if (field.GetCurrentGameObject() == null)
                {
                    possibleFields.Add(field);
                }
                else
                {
                    PieceInterface piece = field.GetCurrentGameObject().GetComponent<Moveable>().GetScript(); // Stores reference to the piece on the viewed square
                    if (IsFieldWithTargetPieceTakeable(piece))
                    {
                        possibleFields.Add(field); // Add Square to the possible destination squares of the rook
                    }
                }
            }
        }

        return possibleFields;
    }
}
