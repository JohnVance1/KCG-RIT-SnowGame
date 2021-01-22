using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceGenerator : MonoBehaviour {
    [SerializeField] HexBoard hexBoard = null;
    [SerializeField] TrianglePiece TPeace = null;
    [SerializeField] StageData PD = null;
    void Start() {
        Transform BoardTrans = hexBoard.transform;

        for (int i = 0; i < PD.PD.Length; i++) {
            var Piece = Instantiate(TPeace);
            Piece.transform.SetParent(BoardTrans);
            Piece.gameObject.name = "Piece" + i.ToString("N2");
            Piece.CreatePiece(PD.PD[i].InitHex, PD.PD[i].HexLength, PD.PD[i].moveDir);
        }
    }

}

