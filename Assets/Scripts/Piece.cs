using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    
    private GameObject board;
    public GameObject currentField;

    [SerializeField] private bool isWhite;

    /*
     * white pawn: 10       black pawn: 21
     * white bishop: 11     black bishop: 21
     * white knight: 12     black knight: 22
     * white rook: 13       black rook: 23
     * white queen: 14      black queen: 24
     * white king: 15       black king: 25
     */
    [SerializeField] private int pieceIdentifier;

    public bool IsWhite { get => isWhite; set => isWhite = value; }

    private void Start()
    {
        board = GameObject.Find("Board");
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
    public int GetPieceIdentifier()
    {
        return pieceIdentifier;
    }

    public void PlacePiece(float x, float y)
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
        board.GetComponent<Board>().FindPieceTarget(this);
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(currentField.transform.position.x, currentField.transform.position.y, -1);
    }

    public GameObject getCurrentField() { return currentField; }
    public void SetCurrentField(GameObject currentField) { this.currentField = currentField; }

    private void OnDestroy()
    {
        Debug.Log("destroyed");
    }

}
