using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private List<Field> fields = new List<Field>();

    [SerializeField] private GameObject playerControlManagerObj;
    private PlayerControlManager playerControlManager;

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
            Debug.Log("initialized");
        }
        else
        {
            Debug.Log("not init");
        }
        InitializeChessboardStandard();
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
                Field tempField = caller.gameObject.GetComponent<Piece>().getCurrentField().GetComponent<Field>(); // Gets the field that the to-be-moved piece was previously standing on
                
                if(tempField != null)   // Check if move is valid. Calling static methods in moveCalc for that. If not valid, quit method.
                {
                    Dictionary<(int, int), int> boardStructure = GetBoardStructure(); // Create snapshots of the board to give context for moveCalc Class. 
                    if (!MoveCalc.ValidMove(field.getFile(), field.getRank(), tempField.getFile(), tempField.getRank(),  
                        tempField.getCurrentPiece().GetComponent<Piece>().IsWhite, boardStructure))
                    {
                        caller.GetComponent<Piece>().ResetPosition();
                        return;
                    }


                    tempField.SetCurrentPiece(null);
                    if (field.getCurrentPiece() != null)
                    {      // checks to see if field is occupied by piece. if yes, then destroy piece
                        Destroy(field.getCurrentPiece());
                    }
                    field.SetCurrentPiece(caller.gameObject);

                    caller.gameObject.GetComponent<Piece>().SetCurrentField(field.gameObject);
                    caller.GetComponent<Piece>().PlacePiece(field.transform.position.x, field.transform.position.y); //Sends the new position for the piece to stand on
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

    private Dictionary<(int, int), int> GetBoardStructure()
    {
        Dictionary<(int, int), int> boardStructure = new Dictionary<(int, int), int>();

        foreach(var field in fields)
        {
            Field fieldScript = field.GetComponent<Field>();
            if (fieldScript.getCurrentPiece() != null) {
                boardStructure.Add((fieldScript.getFile(), fieldScript.getRank()), fieldScript.getCurrentPiece().GetComponent<Piece>().GetPieceIdentifier());
                //Debug.Log(field.gameObject.name +" : "+fieldScript.getCurrentPiece().GetComponent<Piece>().GetPieceIdentifier());
            }else
            {
                boardStructure.Add((fieldScript.getFile(), fieldScript.getRank()), 0); // Sets Piece Identifier to Value: 0
                //Debug.Log(field.gameObject.name + " : 0");
            }   
        }
        return boardStructure;
    }

    private void InitializeChessboardStandard()
    {
        
        foreach (var field in fields) {
            GameObject piece = null;
            if (field.getRank() == 2) // Fills with white pawns
            {
                piece = Instantiate(whitePawn, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z-1), Quaternion.identity);
                
                
            }
            if (field.getRank() == 7) // Fills with black pawns
            {
                piece = Instantiate(blackPawn, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z - 1), Quaternion.identity);
            }
            if(field.getRank() == 1)  // Filling with supposed white pieces
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
    }
}


