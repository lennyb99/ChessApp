using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece, PieceInterface {


    public bool hasMoved=false; // for castling

    public void HandleMovedPiece(Field targetField)
    {
        GameObject currentField = myMoveable.getCurrentField();

        if (GetAllPossibleRookMoves(currentField).Contains(targetField))
        {
            hasMoved = true;
            ExecuteMove(targetField);
        }
        else
        {
            myMoveable.ResetPosition();
        }
    }

    // Calculate all moves the rook can perform.
    public List<Field> GetAllPossibleRookMoves(GameObject currentField)
    {
        List<Field> possibleFields = new List<Field>();
        Field tempCheckField = currentField.GetComponent<Field>(); // This initially stores the position of the rook.

        List<Func<Field, Field>> directions = new List<Func<Field, Field>>
        {
            field => field.topMid,
            field => field.bottomMid,
            field => field.midLeft,
            field => field.midRight,
        };

        foreach (var direction in directions)
        {
            GetAllFieldsTowardsDirection(direction, tempCheckField, possibleFields);
            tempCheckField = currentField.GetComponent<Field>();
        }

        Debug.Log("There were " + possibleFields.Count + " available squares for this rook"); // This DOES count in the NULLFIELDS.
                                                                                              // Since NULLFIELDs cant be physically accessed, it shouldnt be a problem.
        
        // To Debug the possible Squares
        
        foreach (var possibleField in possibleFields)
        {
            Debug.Log(possibleField.getFile() + " " + possibleField.getRank());
        }
        return possibleFields;
    }





}

