using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PieceInterface
{
    public bool IsWhite();
    public void HandleMovedPiece(Field targetField);
    void TakeThisPiece();
}
