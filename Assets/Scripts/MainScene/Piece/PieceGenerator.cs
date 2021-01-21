using UnityEngine;

public class PieceGenerator : MonoBehaviour {
    [SerializeField] HexBoard hexBoard = null;
    [SerializeField] TrianglePiece TPeace = null;
    [SerializeField] PieceData[] PD = null;
    void Start() {
        Transform BoardTrans = hexBoard.transform;
        for (int i = 0; i < PD.Length; i++) {
            var Piece = Instantiate(TPeace);
            Piece.transform.SetParent(BoardTrans);
            Piece.gameObject.name = "Piece" + i.ToString("N2");
            Piece.CreatePiece(PD[i].InitHex, PD[i].HexLength, PD[i].moveDir);
        }
    }

}
