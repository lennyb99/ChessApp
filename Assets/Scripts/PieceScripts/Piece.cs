using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] public bool isWhite;

    public Moveable myMoveable;
    public static bool whiteTurn;

    public King whiteKing;
    public King blackKing;

    void Start()
    {
        whiteTurn = true;

        myMoveable = GetComponent<Moveable>();
        if (myMoveable == null)
        {
            Debug.Log("Piece was not properly initialized.");
        }
    }

    public bool IsItMyTurn()
    {
        if (whiteTurn == isWhite)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
     * Handling of moving a piece on the board. bool validMove is used to determine if technical moving of a piece counts as a turn, eg. castling (moving the rook).
     */
    public void ExecuteMove(Field targetField, bool validMove)
    {
        myMoveable.getCurrentField().GetComponent<Field>().SetCurrentGameobject(null); // Removes piece information of old square
        if(targetField.GetCurrentGameObject() != null) // Take piece if existing on target square
        {
            targetField.GetCurrentGameObject().GetComponent<Piece>().TakeThisPiece();
        }
        myMoveable.SetCurrentField(targetField.gameObject); // updates square information for the piece
        targetField.SetCurrentGameobject(this.gameObject); // sets piece information for new square

        if (!(this is Pawn)) // Disables en passant for the next move for when another piece moves and makes en passant inactive
        {
            myMoveable.board.SetEnPassantPossible(false);
        }

        // Checking for checks, toggling King in Check status
        if (!whiteTurn && whiteKing.IsInCheck())
        {
            whiteKing.inCheck = true;
        }
        else if (whiteTurn && blackKing.IsInCheck())
        {
            blackKing.inCheck = true;
        }

        // Toggling whos turn is next
        if (validMove) { 
            whiteTurn = !whiteTurn;
        }
    }

    public void TakeThisPiece()
    {
        myMoveable.RemovePiece();
    }

    /*
    * Support function to reduce redundancy
    * 
    * Returns TRUE if square is available for current piece
    * FALSE if not
    */
    public bool IsFieldWithTargetPieceTakeable(PieceInterface piece)
    {
        // Rook is WHITE and target piece is WHITE OR Rook is BLACK and target piece is BLACK
        if (isWhite && piece.IsWhite() || !isWhite && !piece.IsWhite())
        {
            return false;
        }
        else        // Rook is WHITE and target piece is BLACK OR Rook is BLACK and target piece is WHITE
                    //if( IsWhite && !piece.IsWhite || !IsWhite && piece.IsWhite)
        {
            return true;
        }
    }

    public bool IsWhite()
    {
        if (isWhite)
        {
            return true;
        }
        return false;
    }

    /*
     * Support function to check for diagonal movements, eg. bishop, rook, queen
     */
    public void GetAllFieldsTowardsDirection(Func<Field, Field> getNextField, Field currentField, List<Field> possibleFields)
    {
        Field tempCheckField = currentField; // Resets position to current position
        while (getNextField(tempCheckField) != null)
        {
            Field nextField = getNextField(tempCheckField);

            if (nextField.GetCurrentGameObject() == null)
            {
                possibleFields.Add(nextField);
                tempCheckField = nextField;
                continue;
            }

            PieceInterface piece = nextField.GetCurrentGameObject().GetComponent<Moveable>().GetScript(); // Stores reference to the piece on the viewed square
            if (IsFieldWithTargetPieceTakeable(piece))
            {
                possibleFields.Add(nextField); // Add Square to the possible destination squares 
            }
            break;
        }
    }

    /*
 * Support function to check for diagonal movements, eg. bishop, rook, queen
 */
    public void GetAllGuardedFieldsTowardsDirection(Func<Field, Field> getNextField, Field currentField, List<Field> possibleFields)
    {
        Field tempCheckField = currentField; // Resets position to current position
        while (getNextField(tempCheckField) != null)
        {
            Field nextField = getNextField(tempCheckField);

            possibleFields.Add(nextField);
            tempCheckField = nextField;

            if (nextField.GetCurrentGameObject() != null)
            {
                possibleFields.Add(nextField);
                break;
            }

            
            //possibleFields.Add(nextField); // Add Square to the possible destination squares 
            
        }
    }

}
