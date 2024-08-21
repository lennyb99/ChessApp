using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private List<Field> fields = new List<Field>();

    [SerializeField] private GameObject playerControlManagerObj;
    private PlayerControlManager playerControlManager;

    private bool whiteKingHasMoved, blackKingHasMoved, whiteRookOneHasMoved, whiteRookTwoHasMoved, blackRookOneHasMoved, blackRookTwoHasMoved = true; // Sets these to true, will be set false if initialized the correct way.
    private bool whiteTurn;

    [SerializeField] private GameObject whitePawn;
    [SerializeField] private GameObject whiteBishop;
    [SerializeField] private GameObject whiteKnight;
    [SerializeField] private GameObject whiteRook;
    [SerializeField] private GameObject whiteQueen;
    [SerializeField] private GameObject whiteKing;
    [SerializeField] private GameObject blackPawn;
    [SerializeField] private GameObject blackBishop;
    [SerializeField] private GameObject blackKnight;
    [SerializeField] private GameObject blackRook;
    [SerializeField] private GameObject blackQueen;
    [SerializeField] private GameObject blackKing;

    // Start is called before the first frame update
    void Start()
    {
        playerControlManager = playerControlManagerObj.GetComponent<PlayerControlManager>();
        if (playerControlManager != null)
        {
            Debug.Log("PlayerCtrlManager initialized");
        }
        else
        {
            Debug.Log("PlayerCtrlManager not initialized");
        }
        whiteTurn = true;
        //InitializeChessboardStandard();
    }

    public void FindPieceTarget(MonoBehaviour caller)
    {
        if (caller.GetComponent<Piece>() == null)   // Checks if the caller component was a Piece
        {
            Debug.Log("function can only be called by a Piece");
            return;
        }

        foreach (var field in fields) // Queue through all fields of the board 
        {
            if (playerControlManager.IsMouseHovering(field.gameObject)) // Check if mouse is hovering over field. Method triggers if mouse is lifted
            {
                // Handling of moving the Piece
                Field oldField = caller.gameObject.GetComponent<Piece>().getCurrentField().GetComponent<Field>(); // Gets the field that the to-be-moved piece was previously standing on

                if (oldField != null)   // Check if move is valid. Calling static methods in moveCalc for that. If not valid, quit method.
                {
                    Dictionary<(int, int), int> boardStructure = GetBoardStructure(); // Create snapshots of the board to give context for moveCalc Class. 
                    
                    // Asking MoveCalc if move is NOT valid. If thats true, the method gets stopped immediately. It proceeds, if move IS VALID. 
                    if (!MoveCalc.ValidMove(field.getFile(), field.getRank(), oldField.getFile(), oldField.getRank(),
                        oldField.getCurrentPiece().GetComponent<Piece>().IsWhite, boardStructure, this))
                    {
                        caller.GetComponent<Piece>().ResetPosition();
                        return;
                    }

                    Debug.Log(MoveCalc.CheckForWhiteKingInCheck(boardStructure));
                    Debug.Log(MoveCalc.CheckForBlackKingInCheck(boardStructure));
                    ExecuteMoveOnBoard(oldField, field, caller.gameObject.GetComponent<Piece>());

                    // If the moved piece was a king or a rook, modify the castling rights and perform castling if necessary. 
                    if (caller.GetComponent<Piece>().GetPieceIdentifier() % 10 == 5 || caller.GetComponent<Piece>().GetPieceIdentifier() % 10 == 3)
                    {
                        HandleCastling(oldField.getFile(), oldField.getRank(), caller.GetComponent<Piece>().GetPieceIdentifier(), field.getFile(), field.getRank());
                    }
                     

                    return;

                }
                else
                {
                    Debug.Log("Failed to set NULL to old field");
                }
            }
        }
        caller.GetComponent<Piece>().ResetPosition();
    }

    private void ExecuteMoveOnBoard(Field oldField, Field newField, Piece piece)
    {
        if (oldField == null || newField == null || piece == null)
        {
            Debug.Log("Null Error while moving a piece");
            return; 
        }
        if (newField.getCurrentPiece() != null) // checks to see if field is occupied by piece. if yes, then destroy piece
        {
            Destroy(newField.getCurrentPiece()); // Deletes Gameobject of captured Piece
        }

        oldField.SetCurrentPiece(null); // Remove the piece from the knowledge of old Field
        newField.SetCurrentPiece(piece.gameObject); // Give the new Field the information of the new piece

        piece.SetCurrentField(newField.gameObject); // Give the piece information about its new position

        piece.PlacePiece(newField.transform.position.x, newField.transform.position.y); // Move the piece physically to its new position on the board
    }


    private void HandleCastling(int file, int rank, int pieceIdentifier, int newFile, int newRank)
    {
        if (file == 1 && rank == 1 && pieceIdentifier == 13)
        {
            whiteRookOneHasMoved = true;
        }
        else if (file == 8 && rank == 1 && pieceIdentifier == 13)
        {
            whiteRookTwoHasMoved = true;
        }
        else if (file == 1 && rank == 8 && pieceIdentifier == 23)
        {
            blackRookOneHasMoved = true;
        }
        else if (file == 8 && rank == 8 && pieceIdentifier == 23)
        {
            blackRookTwoHasMoved = true;
        }
        else if (file == 5 && rank == 1 && pieceIdentifier == 15)
        {
            whiteKingHasMoved = true;
            if(newFile == 7 && newRank == 1) // If a castling move was made
            {
                Field oldRookField = null;
                Field newRookField = null;
                GameObject rook = null;
                foreach (Field field in fields)
                {
                    if (field.getFile() == 8 && field.getRank() == 1) // Retrieve the field of the rook
                    {
                        rook = field.getCurrentPiece();
                        oldRookField = field;
                    }else if (field.getFile() == 6 && field.getRank() == 1)
                    {
                        newRookField = field;
                    }
                }
                ExecuteMoveOnBoard(oldRookField, newRookField, rook.GetComponent<Piece>());
            }
            else if(newFile == 3 && newRank == 1)
            {
                Field oldRookField = null;
                Field newRookField = null;
                GameObject rook = null;
                foreach (Field field in fields)
                {
                    if (field.getFile() == 1 && field.getRank() == 1) // Retrieve the field of the rook
                    {
                        rook = field.getCurrentPiece();
                        oldRookField = field;
                    }
                    else if (field.getFile() == 4 && field.getRank() == 1)
                    {
                        newRookField = field;
                    }
                }
                ExecuteMoveOnBoard(oldRookField, newRookField, rook.GetComponent<Piece>());
            }
        }
        else if (file == 5 && rank == 8 && pieceIdentifier == 25)
        {
            blackKingHasMoved = true;
            if (newFile == 7 && newRank == 8) // If a castling move was made
            {
                Field oldRookField = null;
                Field newRookField = null;
                GameObject rook = null;
                foreach (Field field in fields)
                {
                    if (field.getFile() == 8 && field.getRank() == 8) // Retrieve the field of the rook
                    {
                        rook = field.getCurrentPiece();
                        oldRookField = field;
                    }
                    else if (field.getFile() == 6 && field.getRank() == 8)
                    {
                        newRookField = field;
                    }
                }
                ExecuteMoveOnBoard(oldRookField, newRookField, rook.GetComponent<Piece>());
            }
            else if (newFile == 3 && newRank == 8)
            {
                Field oldRookField = null;
                Field newRookField = null;
                GameObject rook = null;
                foreach (Field field in fields)
                {
                    if (field.getFile() == 1 && field.getRank() == 8) // Retrieve the field of the rook
                    {
                        rook = field.getCurrentPiece();
                        oldRookField = field;
                    }
                    else if (field.getFile() == 4 && field.getRank() == 8)
                    {
                        newRookField = field;
                    }
                }
                ExecuteMoveOnBoard(oldRookField, newRookField, rook.GetComponent<Piece>());
            }
        }
    }

    

    private Dictionary<(int, int), int> GetBoardStructure()
    {
        Dictionary<(int, int), int> boardStructure = new Dictionary<(int, int), int>();

        foreach (var field in fields)
        {
            Field fieldScript = field.GetComponent<Field>();
            if (fieldScript.getCurrentPiece() != null)
            {
                boardStructure.Add((fieldScript.getFile(), fieldScript.getRank()), fieldScript.getCurrentPiece().GetComponent<Piece>().GetPieceIdentifier());
                //Debug.Log(field.gameObject.name +" : "+fieldScript.getCurrentPiece().GetComponent<Piece>().GetPieceIdentifier());
            }
            else
            {
                boardStructure.Add((fieldScript.getFile(), fieldScript.getRank()), 0); // Sets Piece Identifier to Value: 0
                //Debug.Log(field.gameObject.name + " : 0");
            }
        }
        return boardStructure;
    }

    public void InitializeChessboardStandard()
    {

        foreach (var field in fields)
        {
            GameObject piece = null;
            if (field.getRank() == 2) // Fills with white pawns
            {
                piece = Instantiate(whitePawn, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z - 1), Quaternion.identity);


            }
            if (field.getRank() == 7) // Fills with black pawns
            {
                piece = Instantiate(blackPawn, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z - 1), Quaternion.identity);
            }
            if (field.getRank() == 1)  // Filling with supposed white pieces
            {
                if (field.getFile() == 1 || field.getFile() == 8) // White rooks
                {
                    piece = Instantiate(whiteRook, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z - 1), Quaternion.identity);
                }
                else if (field.getFile() == 2 || field.getFile() == 7) // White knights
                {
                    piece = Instantiate(whiteKnight, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z - 1), Quaternion.identity);
                }
                else if (field.getFile() == 3 || field.getFile() == 6) // White bishops
                {
                    piece = Instantiate(whiteBishop, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z - 1), Quaternion.identity);
                }
                else if (field.getFile() == 4) // White queen
                {
                    piece = Instantiate(whiteQueen, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z - 1), Quaternion.identity);
                }
                else // White king
                {
                    piece = Instantiate(whiteKing, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z - 1), Quaternion.identity);
                }
            }

            if (field.getRank() == 8)  // Filling with supposed black pieces
            {
                if (field.getFile() == 1 || field.getFile() == 8) // Black rooks
                {
                    piece = Instantiate(blackRook, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z - 1), Quaternion.identity);
                }
                else if (field.getFile() == 2 || field.getFile() == 7) // black knights
                {
                    piece = Instantiate(blackKnight, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z - 1), Quaternion.identity);
                }
                else if (field.getFile() == 3 || field.getFile() == 6) // black bishops
                {
                    piece = Instantiate(blackBishop, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z - 1), Quaternion.identity);
                }
                else if (field.getFile() == 4) // black queen
                {
                    piece = Instantiate(blackQueen, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z - 1), Quaternion.identity);
                }
                else // black king
                {
                    piece = Instantiate(blackKing, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z - 1), Quaternion.identity);
                }
            }
            if (piece != null)  // Gives the piece and the field the information about each other
            {
                field.SetCurrentPiece(piece);
                piece.GetComponent<Piece>().SetCurrentField(field.gameObject);
            }

        }

        blackRookTwoHasMoved = false;
        blackRookOneHasMoved = false;
        whiteKingHasMoved = false;
        blackKingHasMoved = false;
        whiteRookOneHasMoved = false;
        whiteRookTwoHasMoved = false;
    }

    public bool GetWhiteKingHasMoved() { return whiteKingHasMoved; }
    public bool GetBlackKingHasMoved() { return blackKingHasMoved; }

    public bool GetWhiteRookOneHasMoved() { return whiteRookOneHasMoved; }
    public bool GetWhiteRookTwoHasMoved() { return whiteRookTwoHasMoved; }

    public bool GetBlackRookOneHasMoved() { return blackRookOneHasMoved; }
    public bool GetBlackRookTwoHasMoved() { return blackRookTwoHasMoved; }


}

