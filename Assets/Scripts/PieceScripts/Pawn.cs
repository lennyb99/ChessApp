using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Pawn : Piece, PieceInterface
{
    bool enPassantTake = false;
    
    private GameObject panel;
    public PromotePanel promotePanel;

    public GameObject whiteQueen;
    public GameObject whiteRook;
    public GameObject whiteBishop;
    public GameObject whiteKnight;
    public GameObject blackQueen;
    public GameObject blackRook;
    public GameObject blackBishop;
    public GameObject blackKnight;

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
       
        panel = GameObject.Find("PromotePanel");
        
        if(panel != null)
        {
            promotePanel = panel.GetComponent<PromotePanel>();

            promotePanel.DisablePromotePanel();
        }
        else
        {
            Debug.Log("PromotePanel was not found");
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
        if (isWhite)
        {
            if (GetAllPossibleWhitePawnMoves(currentField).Contains(targetField))
            {
                // Handling a Pawn reaching promoting square
                if (targetField.GetComponent<Field>().getRank() == 8)
                {
                    promotePanel.EnablePromotePanel(this);
                }

                // Handling en passant capture
                if (myMoveable.board.GetEnPassantPossible()) // checks if en passant is possible. Is yes, if last move a 2 step pawn move was made.
                {
                    if (currentField.GetComponent<Field>().topRight == targetField || currentField.GetComponent<Field>().topLeft == targetField) // checks if diagonal takes is a possible move. if yes, its en passant
                    {
                        if (targetField.GetComponent<Field>().bottomMid.GetCurrentGameObject() != null) // checks the field beneath the targetfield if existing piece is on it to delete then
                        {
                            if (targetField.GetComponent<Field>().bottomMid.GetCurrentGameObject().GetComponent<Piece>() == myMoveable.board.GetEnPassantTakeablePiece()) // checks if existing piece is actually the pawn to take.
                            {
                                targetField.GetComponent<Field>().bottomMid.GetCurrentGameObject().GetComponent<Piece>().TakeThisPiece();
                            }
                        }
                    }
                }

                // Handling the enabling of en passant for after the move has been made.
                if (currentField.GetComponent<Field>().getRank() == 2 && targetField.GetComponent<Field>().getRank() == 4)
                {
                    // Notify the board of two square passing which enables en passant captures for this pawn.
                    myMoveable.board.SetEnPassantTakeablePiece(this);
                }
                else
                {
                    myMoveable.board.SetEnPassantPossible(false);
                }

                // Having the move executed
                ExecuteMove(targetField, true);
            }
            else
            {
                myMoveable.ResetPosition();
            }
        }
        else if (!isWhite)
        {
            if (GetAllPossibleBlackPawnMoves(currentField).Contains(targetField))
            {
                // Handling a Pawn reaching promoting square
                if (targetField.GetComponent<Field>().getRank() == 1)
                {
                    promotePanel.EnablePromotePanel(this);
                }

                // Handling en passant capture
                if (myMoveable.board.GetEnPassantPossible()) // checks if en passant is possible. Is yes, if last move a 2 step pawn move was made.
                {
                    if (currentField.GetComponent<Field>().bottomRight == targetField || currentField.GetComponent<Field>().bottomLeft == targetField) // checks if diagonal takes is a possible move. if yes, its en passant
                    {
                        if (targetField.GetComponent<Field>().topMid.GetCurrentGameObject() != null) // checks the field beneath the targetfield if existing piece is on it to delete then
                        {
                            if (targetField.GetComponent<Field>().topMid.GetCurrentGameObject().GetComponent<Piece>() == myMoveable.board.GetEnPassantTakeablePiece()) // checks if existing piece is actually the pawn to take.
                            {
                                targetField.GetComponent<Field>().topMid.GetCurrentGameObject().GetComponent<Piece>().TakeThisPiece();
                            }
                        }
                    }
                }
                if (currentField.GetComponent<Field>().getRank() == 7 && targetField.GetComponent<Field>().getRank() == 5)
                {
                    // notify the board of two square passing which enables en passant captures for this pawn.
                    myMoveable.board.SetEnPassantTakeablePiece(this);
                }
                else
                {
                    myMoveable.board.SetEnPassantPossible(false);
                }
                ExecuteMove(targetField, true);
            }
            else
            {
                myMoveable.ResetPosition();
            }
        }
    }

    /*
     * Returns the possible fields and corresponding bool value, whether the piece was taken en passant. 
     */
    public List<Field> GetAllPossibleWhitePawnMoves(GameObject currentField)
    {
        List<Field> possibleFields = new List<Field>();

        Board board = myMoveable.board;
        // int file = currentField.GetComponent<Field>().getFile(); // Not needed
        int rank = currentField.GetComponent<Field>().getRank();

        // Check for En Passant options
        // This will check if en passant is currently a possibility. If it is, this pawn will check, if the target pawn is horizontally next to it. If yes, it can initiate en passant. 
        if (myMoveable.board.GetEnPassantPossible())
        {
            if(currentField.GetComponent<Field>().midLeft.GetCurrentGameObject() != null) { 
                if(myMoveable.board.GetEnPassantTakeablePiece().gameObject == currentField.GetComponent<Field>().midLeft.GetCurrentGameObject())
                {
                    possibleFields.Add(currentField.GetComponent<Field>().topLeft);
                }
            }
            if (currentField.GetComponent<Field>().midRight.GetCurrentGameObject() != null) {
                if (myMoveable.board.GetEnPassantTakeablePiece() == currentField.GetComponent<Field>().midRight.GetCurrentGameObject().GetComponent<Piece>())
                {
                    possibleFields.Add(currentField.GetComponent<Field>().topRight);
                }
            }
        }

        // Two squares forward
        if(rank == 2 && currentField.GetComponent<Field>().topMid.GetCurrentGameObject() == null && currentField.GetComponent<Field>().topMid.topMid.GetCurrentGameObject() == null)
        {
            possibleFields.Add(currentField.GetComponent<Field>().topMid.topMid);
        }

        // One square forward
        if (currentField.GetComponent<Field>().topMid != null && currentField.GetComponent<Field>().topMid.GetCurrentGameObject() == null)
        {
            possibleFields.Add(currentField.GetComponent<Field>().topMid);
        }

        // Diagonal hit Left
        if(currentField.GetComponent<Field>().topLeft.GetCurrentGameObject() != null)
        {
            PieceInterface piece = currentField.GetComponent<Field>().topLeft.GetCurrentGameObject().GetComponent<Moveable>().GetScript();
            if (IsFieldWithTargetPieceTakeable(piece))
            {
                possibleFields.Add(currentField.GetComponent <Field>().topLeft);
            }
        }

        // Diagonal hit Right
        if (currentField.GetComponent<Field>().topRight.GetCurrentGameObject() != null)
        {
            PieceInterface piece = currentField.GetComponent<Field>().topRight.GetCurrentGameObject().GetComponent<Moveable>().GetScript();
            if (IsFieldWithTargetPieceTakeable(piece))
            {
                possibleFields.Add(currentField.GetComponent<Field>().topRight);
            }
        }
        return possibleFields;
    }

    public List<Field> GetAllPossibleBlackPawnMoves(GameObject currentField)
    {
        List<Field> possibleFields = new List<Field>();

        Board board = myMoveable.board;
        // int file = currentField.GetComponent<Field>().getFile(); // Not needed
        int rank = currentField.GetComponent<Field>().getRank();

        // Check for En Passant options
        // This will check if en passant is currently a possibility. If it is, this pawn will check, if the target pawn is horizontally next to it. If yes, it can initiate en passant. 
        if (myMoveable.board.GetEnPassantPossible())
        {
            if (currentField.GetComponent<Field>().midLeft.GetCurrentGameObject() != null)
            {
                if (myMoveable.board.GetEnPassantTakeablePiece().gameObject == currentField.GetComponent<Field>().midLeft.GetCurrentGameObject())
                {
                    possibleFields.Add(currentField.GetComponent<Field>().bottomLeft);
                }
            }
            if (currentField.GetComponent<Field>().midRight.GetCurrentGameObject() != null)
            {
                if (myMoveable.board.GetEnPassantTakeablePiece() == currentField.GetComponent<Field>().midRight.GetCurrentGameObject().GetComponent<Piece>())
                {
                    possibleFields.Add(currentField.GetComponent<Field>().bottomRight);
                }
            }
        }

        // Two squares forward
        if (rank == 7 && currentField.GetComponent<Field>().bottomMid.GetCurrentGameObject() == null && currentField.GetComponent<Field>().bottomMid.bottomMid.GetCurrentGameObject() == null)
        {
            possibleFields.Add(currentField.GetComponent<Field>().bottomMid.bottomMid);
        }

        // One square forward
        if (currentField.GetComponent<Field>().bottomMid != null && currentField.GetComponent<Field>().bottomMid.GetCurrentGameObject() == null)
        {
            possibleFields.Add(currentField.GetComponent<Field>().bottomMid);
        }

        // Diagonal hit Left
        if (currentField.GetComponent<Field>().bottomLeft.GetCurrentGameObject() != null)
        {
            PieceInterface piece = currentField.GetComponent<Field>().bottomLeft.GetCurrentGameObject().GetComponent<Moveable>().GetScript();
            if (IsFieldWithTargetPieceTakeable(piece))
            {
                possibleFields.Add(currentField.GetComponent<Field>().bottomLeft);
            }
        }

        // Diagonal hit Right
        if (currentField.GetComponent<Field>().bottomRight.GetCurrentGameObject() != null)
        {
            PieceInterface piece = currentField.GetComponent<Field>().bottomRight.GetCurrentGameObject().GetComponent<Moveable>().GetScript();
            if (IsFieldWithTargetPieceTakeable(piece))
            {
                possibleFields.Add(currentField.GetComponent<Field>().bottomRight);
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

    
    public void PromotePawn(char pieceType)
    {
        GameObject piece = null;
        if (isWhite) { 
            switch (pieceType)
            {
                case 'q':
                    piece = Instantiate(whiteQueen, new Vector3(myMoveable.currentField.transform.position.x, myMoveable.currentField.transform.position.y, myMoveable.currentField.transform.position.z - 1), Quaternion.identity);
                    break;
                case 'r':
                    piece = Instantiate(whiteRook, new Vector3(myMoveable.currentField.transform.position.x, myMoveable.currentField.transform.position.y, myMoveable.currentField.transform.position.z - 1), Quaternion.identity);
                    break;
                case 'b':
                    piece = Instantiate(whiteBishop, new Vector3(myMoveable.currentField.transform.position.x, myMoveable.currentField.transform.position.y, myMoveable.currentField.transform.position.z - 1), Quaternion.identity);
                    break;
                case 'k':
                    piece = Instantiate(whiteKnight, new Vector3(myMoveable.currentField.transform.position.x, myMoveable.currentField.transform.position.y, myMoveable.currentField.transform.position.z - 1), Quaternion.identity);
                    break;
            }
        }
        else
        {
            switch (pieceType)
            {
                case 'q':
                    piece = Instantiate(blackQueen, new Vector3(myMoveable.currentField.transform.position.x, myMoveable.currentField.transform.position.y, myMoveable.currentField.transform.position.z - 1), Quaternion.identity);
                    break;
                case 'r':
                    piece = Instantiate(blackRook, new Vector3(myMoveable.currentField.transform.position.x, myMoveable.currentField.transform.position.y, myMoveable.currentField.transform.position.z - 1), Quaternion.identity);
                    break;
                case 'b':
                    piece = Instantiate(blackBishop, new Vector3(myMoveable.currentField.transform.position.x, myMoveable.currentField.transform.position.y, myMoveable.currentField.transform.position.z - 1), Quaternion.identity);
                    break;
                case 'k':
                    piece = Instantiate(blackKnight, new Vector3(myMoveable.currentField.transform.position.x, myMoveable.currentField.transform.position.y, myMoveable.currentField.transform.position.z - 1), Quaternion.identity);
                    break;
            }
        }
        if (piece != null)
        {
            piece.GetComponent<Piece>().myMoveable.SetCurrentField(myMoveable.currentField);
            myMoveable.currentField.GetComponent<Field>().SetCurrentGameobject(piece);
            myMoveable.RemovePiece();
        }
        
    }
    public bool AmIGuardingField(Field field, bool whiteGuarding)
    {
        if (whiteGuarding != isWhite)
        {
            return false;
        }
        if (isWhite) { 
            if (myMoveable.currentField.GetComponent<Field>().topLeft == field)
            {
                return true;
            }
            else if (myMoveable.currentField.GetComponent<Field>().topRight == field)
            {
                return true;
            }
        }
        else
        {
            if (myMoveable.currentField.GetComponent<Field>().bottomLeft == field)
            {
                return true;
            }
            else if (myMoveable.currentField.GetComponent<Field>().bottomRight == field)
            {
                return true;
            }
        }

        return false;
    }
}
