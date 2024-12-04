using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece, PieceInterface
{
    private void Awake()
    {
        GameObject board = GameObject.Find("Board");
        if (board != null)
        {
            board.GetComponent<Board>().RegisterPiece(this);
        }
    }

    public void HandleMovedPiece(Field targetField)
    {
        if (!IsItMyTurn())
        {
            myMoveable.ResetPosition();
            return;
        }

        GameObject currentField = myMoveable.getCurrentField();

        if (GetAllPossibleKnightMoves(currentField).Contains(targetField))
        {
            ExecuteMove(targetField, true);
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

    public bool AmIGuardingField(Field field, bool whiteGuarding)
    {
        GameObject currentField = myMoveable.currentField;

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
            if (isWhite == whiteGuarding && pair.Item1 == field.getFile() && pair.Item2 == field.getRank())
            {
                return true;
            }
        }
        return false;
    }
}
