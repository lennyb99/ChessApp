using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    [SerializeField] public Board board;

    public GameObject currentField;

    PieceInterface script;

    private void Start()
    {
        board = GameObject.Find("Board").GetComponent<Board>();

        if (GetComponent<Rook>() != null)
        {
            script = GetComponent<Rook>();
            return;
        }
        else if(GetComponent<Bishop>() != null)
        {
            script = GetComponent<Bishop>();
            return;
        }
        else if (GetComponent<Queen>() != null)
        {
            script = GetComponent<Queen>();
            return;
        }
        else if (GetComponent<King>() != null)
        {
            script = GetComponent<King>();
            return;
        }
        else if (GetComponent<Pawn>() != null)
        {
            script = GetComponent<Pawn>();
            return;
        }
        else if (GetComponent<Knight>() != null)
        {
            script = GetComponent<Knight>();
            return;
        }
        Debug.Log(gameObject.name + " is not properly initialized.");
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = -1; 
            transform.position = newPosition + offset;
        }
    }

    public void PlaceObject(float x, float y)
    {
        transform.position = new Vector3(x, y, -1);
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset.z = -1;
    }

    void OnMouseUp()
    {
        isDragging = false;
        script.HandleMovedPiece(board.FindTargetSquare());
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(currentField.transform.position.x, currentField.transform.position.y, -1);
    }

    public GameObject getCurrentField() { return currentField; }
    public void SetCurrentField(GameObject targetField) { 
        this.currentField = targetField;
        PlaceObject(targetField.transform.position.x, targetField.transform.position.y); // Move the piece physically to its new position on the board
    }

    public void RemovePiece()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log(gameObject.name + " was destroyed");
    }

    public PieceInterface GetScript()
    {
        return script;
    }

}
