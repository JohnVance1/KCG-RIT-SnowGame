using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("Tools/CreatePieceData"))]
public class PieceData : ScriptableObject {
    public HexCoordinates InitHex = new HexCoordinates(HexDirection.N, 0, 0);

    public uint HexLength = 1;

    public MoveDirection moveDir = MoveDirection.N_S;

}
