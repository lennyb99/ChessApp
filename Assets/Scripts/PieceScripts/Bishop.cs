using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Bishop : Piece, PieceInterface
{
    public void HandleMovedPiece(Field targetField)
    {
        GameObject currentField = myMoveable.getCurrentField();

        if (GetAllPossibleBishopMoves(currentField).Contains(targetField))
        {
            ExecuteMove(targetField);
        }
        else
        {
            myMoveable.ResetPosition();
        }
    }

    private List<Field> GetAllPossibleBishopMoves(GameObject currentField)
    {
        List<Field> possibleFields = new List<Field>();
        Field tempCheckField = currentField.GetComponent<Field>(); // This initially stores the position of the rook.

        List<Func<Field, Field>> directions = new List<Func<Field, Field>>
        {
            field => field.topLeft,
            field => field.topRight,
            field => field.bottomRight,
            field => field.bottomLeft,
        };

        foreach (var direction in directions)
        {
            GetAllFieldsTowardsDirection(direction, tempCheckField, possibleFields);
            tempCheckField = currentField.GetComponent<Field>();
        }

        Debug.Log("There were " + possibleFields.Count + " available squares for this bishop"); // This DOES count in the NULLFIELDS.
                                                                                              // Since NULLFIELDs cant be physically accessed, it shouldnt be a problem.

        // To Debug the possible Squares

        foreach (var possibleField in possibleFields)
        {
            Debug.Log(possibleField.getFile() + " " + possibleField.getRank());
        }
        return possibleFields;
    }
}
