using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameChessGameManager : MonoBehaviour
{
    [SerializeField] Board board;

    private List<Dictionary<(int, int), int>> allMoves;
    bool x;

    // Start is called before the first frame update
    void Start()
    {
        allMoves = new List<Dictionary<(int, int), int>>();
        
        Debug.Log(x);
    }

    public void InitializeChessBoard()
    {
        board.InitializeChessboardStandard();
    }

    void addMove(Dictionary<(int,int), int> newPosition)
    {
        allMoves.Add(newPosition);
    }
}
