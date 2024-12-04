using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Field : MonoBehaviour {

    [SerializeField] private int rank;
    [SerializeField] private int file;

    [SerializeField] private string color;

    public Board board;

    public GameObject currentGameObject;
    public PieceInterface currentPiece;

    [Header("Adjacent Fields")]
    public Field topMid;
    public Field topLeft;
    public Field topRight;
    public Field midLeft;
    public Field midRight;
    public Field bottomMid;
    public Field bottomLeft;
    public Field bottomRight;

    


    void Start() {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (color.Equals("white")) {
            spriteRenderer.material.color = new Color(252f / 255f, 188f / 255f, 141f / 255f, 1.0f);
        }
        else {
            spriteRenderer.material.color = new Color(102f/255f,47f/255f,7f/255f,1.0f);
        }
    }
   

    public int getRank() { return rank; }
    public int getFile() { return file; }

    public void SetCurrentGameobject(GameObject piece)
    {
        this.currentGameObject = piece;
    }

    public bool IsFieldGuarded(bool whiteGuarding)
    {
        if (board == null)
        {
            return false;
        }
        foreach (PieceInterface piece in board.GetAllPiecesOnBoard())
        {
            if (piece.AmIGuardingField(this, whiteGuarding))
            {
                return true;
            }
        }
        return false;
    }

    public GameObject GetCurrentGameObject() { return currentGameObject; }
}
