using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameChessGameManager : MonoBehaviour
{
    [SerializeField] Board board;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitializeChessBoard()
    {
        board.InitializeChessboardStandard();
    }

}
