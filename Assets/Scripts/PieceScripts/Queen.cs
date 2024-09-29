using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece, PieceInterface
{

    public void HandleMovedPiece(Field targetField)
    {
        GameObject currentField = myMoveable.getCurrentField();

        if (GetAllPossibleQueenMoves(currentField).Contains(targetField))
        {
            ExecuteMove(targetField);
        }
        else
        {
            myMoveable.ResetPosition();
        }
    }

    public List<Field> GetAllPossibleQueenMoves(GameObject currentField)
    {
        List<Field> possibleFields = new List<Field>();
        Field tempCheckField = currentField.GetComponent<Field>(); // This initially stores the position of the rook.

        List<Func<Field, Field>> directions = new List<Func<Field, Field>>
        {
            field => field.topMid,
            field => field.topRight,
            field => field.midRight,
            field => field.bottomRight,
            field => field.bottomMid,
            field => field.bottomLeft,
            field => field.midLeft,
            field => field.topLeft,
        };

        foreach (var direction in directions)
        {
            GetAllFieldsTowardsDirection(direction, tempCheckField, possibleFields);
            tempCheckField = currentField.GetComponent<Field>();
        }

        Debug.Log("There were " + possibleFields.Count + " available squares for this queen"); // This DOES count in the NULLFIELDS.
                                                                                              // Since NULLFIELDs cant be physically accessed, it shouldnt be a problem.

        // To Debug the possible Squares

        foreach (var possibleField in possibleFields)
        {
            Debug.Log(possibleField.getFile() + " " + possibleField.getRank());
        }
        return possibleFields;
    }

}
