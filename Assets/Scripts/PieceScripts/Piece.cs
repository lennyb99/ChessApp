using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] public bool isWhite;

    public Moveable myMoveable;

    void Start()
    {
        myMoveable = GetComponent<Moveable>();
        if (myMoveable == null)
        {
            Debug.Log("Piece was not properly initialized.");
        }
    }



    public void ExecuteMove(Field targetField)
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
        Field tempCheckField = currentField; // Resets position to current rook position
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

}
