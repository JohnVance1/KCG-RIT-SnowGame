using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck2D : MonoBehaviour {
    public bool isCollision = false;
    public bool isClear = false;
    public List<string> CollisionList;
    public List<string> ClearList;
    void OnTriggerStay2D(Collider2D col) {
        if (CollisionList.Contains(col.tag)) {
            isCollision = true;
        } else if (ClearList.Contains(col.tag)) {
            isClear = true;
        }
    }
}
