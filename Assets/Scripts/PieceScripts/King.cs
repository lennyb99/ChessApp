using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece, PieceInterface
{
    public bool hasMoved = false; // for castling
    public bool inCheck = false;

    private void Awake()
    {
        GameObject board = GameObject.Find("Board");
        if (board != null)
        {
            board.GetComponent<Board>().RegisterPiece(this);
        }
    }

    void Start()
    {
        myMoveable.board.ShareKingInformation(this, isWhite);
    }

    public void HandleMovedPiece(Field targetField)
    {

        if (!IsItMyTurn())
        {
            myMoveable.ResetPosition();
            return;
        }

        GameObject currentField = myMoveable.getCurrentField();

        if (GetAllPossibleKingMoves(currentField).Contains(targetField))
        {
            hasMoved = true;
            if (whiteTurn && !whiteKing.inCheck || !whiteTurn && !blackKing.inCheck)
            {
                if (currentField.GetComponent<Field>().midRight.midRight == targetField) // Detect short castle
                {
                    Piece rook = currentField.GetComponent<Field>().midRight.midRight.midRight.GetCurrentGameObject().GetComponent<Piece>();
                    rook.ExecuteMove(currentField.GetComponent<Field>().midRight, false);
                }
                if (currentField.GetComponent<Field>().midLeft.midLeft == targetField) // Detect long castle
                {
                    Piece rook = currentField.GetComponent<Field>().midLeft.midLeft.midLeft.midLeft.GetCurrentGameObject().GetComponent<Piece>();
                    rook.ExecuteMove(currentField.GetComponent<Field>().midLeft, false);
                }
            }
            ExecuteMove(targetField, true);
        }
        else
        {
            myMoveable.ResetPosition();
        }
    }

    private bool IsCastlingPossible(bool shortCastle, GameObject currentField)
    {
        Field tempField = currentField.GetComponent<Field>();

        if (shortCastle)
        {
            while (tempField.midRight.gameObject.name != "NULLFIELD")
            {
                tempField = tempField.midRight;
                if (tempField.GetCurrentGameObject() != null) // Check whether square is empty
                {
                    if(tempField.GetCurrentGameObject().GetComponent<Rook>() != null && tempField.GetCurrentGameObject().GetComponent<Rook>().isWhite == isWhite
                        && tempField.GetCurrentGameObject().GetComponent<Rook>().hasMoved == false)
                    {
                        return true;
                    }
                    return false;
                }
                if (tempField.IsFieldGuarded(!isWhite)) // Checks whether the square is guarded. Parameter is false, because white wants to check if black pieces are guarding
                {
                    return false;
                }
            }
        }else
        {
            while (tempField.midLeft.gameObject.name != "NULLFIELD")
            {
                tempField = tempField.midLeft;
                if (tempField.GetCurrentGameObject() != null) // Check whether square is empty
                {
                    if (tempField.GetCurrentGameObject().GetComponent<Rook>() != null && tempField.GetCurrentGameObject().GetComponent<Rook>().isWhite == isWhite
                        && tempField.GetCurrentGameObject().GetComponent<Rook>().hasMoved == false)
                    {
                        return true;
                    }
                    return false;
                }
                if (tempField.IsFieldGuarded(!isWhite)) // Checks whether the square is guarded. Parameter is false, because white wants to check if black pieces are guarding
                {
                    return false;
                }
            }
        }

        return false;
    }

    public List<Field> GetAllPossibleKingMoves(GameObject currentField)
    {
        List<Field> possibleFields = new List<Field>();
        Field myField = currentField.GetComponent<Field>();

        if (!inCheck && !hasMoved) {
            // Castling long
            if (IsCastlingPossible(false, currentField))
            {
                possibleFields.Add(myField.midLeft.midLeft);
            }

            // Castling short
            if (IsCastlingPossible(true, currentField))
            {
                possibleFields.Add(myField.midRight.midRight);
            }
        }


        // Top Mid
        if (currentField.GetComponent<Field>().topMid.GetCurrentGameObject() == null) // Does the next field hold a piece?
        {
            possibleFields.Add(currentField.GetComponent<Field>().topMid);
        }
        else { 
            PieceInterface piece = currentField.GetComponent<Field>().topMid.GetCurrentGameObject().GetComponent<Moveable>().GetScript(); // Stores reference to the piece on the viewed square
            if (IsFieldWithTargetPieceTakeable(piece))
            {
                possibleFields.Add(currentField.GetComponent<Field>().topMid); 
            }
        }

        // Top Right
        if (currentField.GetComponent<Field>().topRight.GetCurrentGameObject() == null) // Does the next field hold a piece?
        {
            possibleFields.Add(currentField.GetComponent<Field>().topRight);
        }
        else
        {
            PieceInterface piece = currentField.GetComponent<Field>().topRight.GetCurrentGameObject().GetComponent<Moveable>().GetScript(); // Stores reference to the piece on the viewed square
            if (IsFieldWithTargetPieceTakeable(piece))
            {
                possibleFields.Add(currentField.GetComponent<Field>().topRight); 
            }
        }
        // Mid Right
        if (currentField.GetComponent<Field>().midRight.GetCurrentGameObject() == null) // Does the next field hold a piece?
        {
            possibleFields.Add(currentField.GetComponent<Field>().midRight);
        }
        else
        {
            PieceInterface piece = currentField.GetComponent<Field>().midRight.GetCurrentGameObject().GetComponent<Moveable>().GetScript(); // Stores reference to the piece on the viewed square
            if (IsFieldWithTargetPieceTakeable(piece))
            {
                possibleFields.Add(currentField.GetComponent<Field>().midRight); 
            }
        }
        // Bottom Right
        if (currentField.GetComponent<Field>().bottomRight.GetCurrentGameObject() == null) // Does the next field hold a piece?
        {
            possibleFields.Add(currentField.GetComponent<Field>().bottomRight);
        }
        else
        {
            PieceInterface piece = currentField.GetComponent<Field>().bottomRight.GetCurrentGameObject().GetComponent<Moveable>().GetScript(); // Stores reference to the piece on the viewed square
            if (IsFieldWithTargetPieceTakeable(piece))
            {
                possibleFields.Add(currentField.GetComponent<Field>().bottomRight); // Add Square to the possible destination squares of the rook
            }
        }
        // Bottom Mid
        if (currentField.GetComponent<Field>().bottomMid.GetCurrentGameObject() == null) // Does the next field hold a piece?
        {
            possibleFields.Add(currentField.GetComponent<Field>().bottomMid);
        }
        else
        {
            PieceInterface piece = currentField.GetComponent<Field>().bottomMid.GetCurrentGameObject().GetComponent<Moveable>().GetScript(); // Stores reference to the piece on the viewed square
            if (IsFieldWithTargetPieceTakeable(piece))
            {
                possibleFields.Add(currentField.GetComponent<Field>().bottomMid); // Add Square to the possible destination squares of the rook
            }
        }
        // Bottom Left
        if (currentField.GetComponent<Field>().bottomLeft.GetCurrentGameObject() == null) // Does the next field hold a piece?
        {
            possibleFields.Add(currentField.GetComponent<Field>().bottomLeft);
        }
        else
        {
            PieceInterface piece = currentField.GetComponent<Field>().bottomLeft.GetCurrentGameObject().GetComponent<Moveable>().GetScript(); // Stores reference to the piece on the viewed square
            if (IsFieldWithTargetPieceTakeable(piece))
            {
                possibleFields.Add(currentField.GetComponent<Field>().bottomLeft); // Add Square to the possible destination squares of the rook
            }
        }
        // Mid Left
        if (currentField.GetComponent<Field>().midLeft.GetCurrentGameObject() == null) // Does the next field hold a piece?
        {
            possibleFields.Add(currentField.GetComponent<Field>().midLeft);
        }
        else
        {
            PieceInterface piece = currentField.GetComponent<Field>().midLeft.GetCurrentGameObject().GetComponent<Moveable>().GetScript(); // Stores reference to the piece on the viewed square
            if (IsFieldWithTargetPieceTakeable(piece))
            {
                possibleFields.Add(currentField.GetComponent<Field>().midLeft); // Add Square to the possible destination squares of the rook
            }
        }
        // Top Left
        if (currentField.GetComponent<Field>().topLeft.GetCurrentGameObject() == null) // Does the next field hold a piece?
        {
            possibleFields.Add(currentField.GetComponent<Field>().topLeft);
        }
        else
        {
            PieceInterface piece = currentField.GetComponent<Field>().topLeft.GetCurrentGameObject().GetComponent<Moveable>().GetScript(); // Stores reference to the piece on the viewed square
            if (IsFieldWithTargetPieceTakeable(piece))
            {
                possibleFields.Add(currentField.GetComponent<Field>().topLeft); // Add Square to the possible destination squares of the rook
            }
        }

        // Subtract all squares that are covered by enemy pieces
        for (int i = possibleFields.Count -1; i >= 0; i--)
        {
            if (possibleFields[i].IsFieldGuarded(!isWhite))
            {
                possibleFields.RemoveAt(i);
            }
        }

        // To Debug the possible Squares
        /*
        foreach (var possibleField in possibleFields)
        {
            Debug.Log(possibleField.getFile() + " " + possibleField.getRank());
        }
        */


        return possibleFields;
    }

    public bool IsInCheck()
    {
        if (myMoveable.currentField.GetComponent<Field>().IsFieldGuarded(!isWhite))
        {
            return true;
        }
        return false;
    }

    public bool AmIGuardingField(Field field, bool whiteGuarding)
    {
        if (whiteGuarding != isWhite)
        {
            return false;
        }
        Field currentField = myMoveable.currentField.GetComponent<Field>();
        if(currentField.topMid == field ||
            currentField.topRight == field ||
            currentField.midRight == field ||
            currentField.bottomRight == field ||
            currentField.bottomMid == field ||
            currentField.bottomLeft == field ||
            currentField.midLeft == field ||
            currentField.topLeft == field)
        {
            return true;
        }

        return false;
    }
}
