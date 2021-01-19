using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeGenerator : MonoBehaviour {
    [SerializeField] HexBoard board;
    [SerializeField] PolygonCollider2D FieldEdge;
    [SerializeField] PolygonCollider2D ClearEdge;

    void Start() {
        Vector2[] fieldEdge = new Vector2[14];
        Vector2[] clearEdge = new Vector2[14];
        Vector2 vec = Vector2.zero;
        byte index = 0;
        for (byte edgeNum = 0; edgeNum < 7; edgeNum++) {
            vec.x = Mathf.Cos(60 * index * Mathf.Deg2Rad);
            vec.y = Mathf.Sin(60 * index * Mathf.Deg2Rad);
            fieldEdge[edgeNum] = vec * (board.size + 0.01f);
            clearEdge[edgeNum] = vec * (board.size + 1.01f);
            index++;
        }
        index--;
        for (byte edgeNum = 7; edgeNum < 14; edgeNum++) {
            vec.x = Mathf.Cos(60 * index * Mathf.Deg2Rad);
            vec.y = Mathf.Sin(60 * index * Mathf.Deg2Rad);
            fieldEdge[edgeNum] = vec * (board.size + 1f);
            clearEdge[edgeNum] = vec * (board.size + 2f);
            index--;
        }
        FieldEdge.points = fieldEdge;
        ClearEdge.points = clearEdge;
    }
}
