using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private List<Field> fields = new List<Field>();
    private List<PieceInterface> pieces = new List<PieceInterface>();

    [SerializeField] private GameObject playerControlManagerObj;
    private PlayerControlManager playerControlManager;
    private bool whiteTurn;

    [SerializeField] private bool enPassantPossible;
    [SerializeField] private Piece enPassantTakeablePiece;

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
        if (playerControlManager == null)
        {
            Debug.Log("PlayerCtrlManager not initialized");
        }
    }
    public List<PieceInterface> GetAllPiecesOnBoard()
    {
        return pieces; 
    }
    public void RegisterPiece(PieceInterface newPiece)
    {
        pieces.Add(newPiece);
        Debug.Log(pieces.Count);
    }

    public void UnregisterPiece(PieceInterface delPiece)
    {
        pieces.Remove(delPiece);
        Debug.Log(pieces.Count);
    }

    public void AddMove()
    {
        
    }

    public Field FindSquareByCoordinates(int file, int rank)
    {
        foreach(var field in fields)
        {
            if(field.getFile() == file && field.getRank() == rank)
            {
                return field;
            }
        }

        return null;
    }

    public Field FindTargetSquare()
    {
        foreach (var field in fields) // Queue through all fields of the board 
        {
            if (playerControlManager.IsMouseHovering(field.gameObject)) // Check if mouse is hovering over field. Method triggers if mouse is lifted
            {
                return field;
            }
        }
        return null;
    }

    public void ShareKingInformation(King king, bool white)
    {
        foreach(Piece piece in pieces)
        {
            if (white)
            {
                piece.whiteKing = king;
            }
            else
            {
                piece.blackKing = king;
            }
        }
    }

    private Dictionary<(int, int), int> GetBoardStructure()
    {
        Dictionary<(int, int), int> boardStructure = new Dictionary<(int, int), int>();

        foreach (var field in fields)
        {
            Field fieldScript = field.GetComponent<Field>();
            if (fieldScript.GetCurrentGameObject() != null)
            {
                //boardStructure.Add((fieldScript.getFile(), fieldScript.getRank()), fieldScript.getCurrentPiece().GetComponent<Moveable>().GetPieceIdentifier());
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

    public void InitializeRookTesting() {
        GameObject piece = null;
        var field = fields[30];
        piece = Instantiate(blackPawn, new Vector3(field.transform.position.x, field.transform.position.y, field.transform.position.z - 1), Quaternion.identity);

        field.SetCurrentGameobject(piece);
        piece.GetComponent<Moveable>().SetCurrentField(field.gameObject); 
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
                field.SetCurrentGameobject(piece);
                piece.GetComponent<Moveable>().SetCurrentField(field.gameObject);
            }

        }
        whiteTurn = true;
        enPassantPossible = false;
    }

    public void SetEnPassantTakeablePiece(Piece piece)
    {
        enPassantTakeablePiece = piece;
        enPassantPossible = true;
    }

    public void SetEnPassantPossible(bool val)
    {
        enPassantPossible=val;
    }

    public bool GetEnPassantPossible()
    {
        return enPassantPossible;
    }

    public Piece GetEnPassantTakeablePiece()
    {
        return enPassantTakeablePiece;
    }



}

