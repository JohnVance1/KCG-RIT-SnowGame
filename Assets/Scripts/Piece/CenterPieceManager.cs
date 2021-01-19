using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterPieceManager : MonoBehaviour {
    [SerializeField] CollisionCheck2D[] CenterCollisions;
    [SerializeField] CenterPiece[] CenterPieces;

    // Update is called once per frame
    void Update() {
        for (byte i = 0; i < CenterCollisions.Length; i++) {
            if (CenterCollisions[i].isCollision) {
                for (byte j = 0; j < CenterPieces.Length; j++) {
                    CenterPieces[j].SetVertex(0, true);
                }
            }
        }
    }
}
