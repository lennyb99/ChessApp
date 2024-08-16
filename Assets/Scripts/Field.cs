using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Field : MonoBehaviour {

    [SerializeField] private int rank;
    [SerializeField] private int file;

    [SerializeField] private string color;

    public GameObject currentPiece;


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

    public void SetCurrentPiece(GameObject piece)
    {
        this.currentPiece = piece;
    }

    public GameObject getCurrentPiece() { return currentPiece; }
}
