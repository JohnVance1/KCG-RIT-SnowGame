using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceLine : MonoBehaviour {
    LineRenderer render;
    void Start() {
        render = GetComponent<LineRenderer>();
        render.startWidth = 0.05f;
        render.endWidth = 0.05f;
        render.positionCount = 1;
        render.SetPosition(0, Vector3.zero);
    }
    public void SetVertex(Vector3[] pos) {
        render.positionCount = pos.Length;
        Vector3 vec = Vector3.zero;
        for (int i = 0; i < pos.Length; i++) {
            render.SetPosition(i, vec + pos[i]);
            vec += pos[i];
        }
    }

}
