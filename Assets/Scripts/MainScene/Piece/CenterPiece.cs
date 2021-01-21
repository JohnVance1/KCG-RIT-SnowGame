using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class CenterPiece : MeshGenBase {

    MeshCollider meshcollision = null;//メッシュ当たり判定(レイを飛ばしてクリックを検知するため。)
    [SerializeField] PolygonCollider2D Polygon = null;//ポリゴン当たり判定(三角同士の当たり判定検知)
    [SerializeField] PieceLine PieceLine;//ピースの線を引くクラス。
    [SerializeField] int DirAdd = 0;
    [SerializeField] bool flipX = false;
    CollisionCheck2D Col = null;
    MeshFilter MFcache = null;
    Camera mainCam;//カメラ
    bool isClicked = false;//今クリックされているかを格納する。
    List<Vector3> LineVertex = new List<Vector3>();//線の

    public bool centerSet = false;

    //動かす際に必要。
    Vector3 CalcPos = Vector3.zero;//位置の計算に使う。
    float screenSizeY = 0;
    Vector3 MouseInitPos = Vector3.zero;//マウスの初期位置を
    Vector3[] VertexInit { get; set; }
    Vector2[] PolygonVex { get; set; }
    bool isFadeOut = false;
    void Awake() {
        LineVertex.Add(Vector3.zero);
        //CreateMesh
        mesh = new Mesh { name = gameObject.name };
        Polygon.gameObject.name = gameObject.name;
        //MeshSettings
        MFcache = GetComponent<MeshFilter>();
        MFcache.mesh = mesh;

       
        meshcollision = GetComponent<MeshCollider>();
        meshcollision.sharedMesh = mesh;
        Col = Polygon.GetComponent<CollisionCheck2D>();
        mainCam = Camera.main;

        screenSizeY = Screen.width / 4;
        CreateHex();
    }
    void Update() {
        if (isFadeOut) return;
        float dir = GetDir(MouseInitPos, Input.mousePosition) + 180 + DirAdd;
        int CalcDir = (int)Mathf.Round(dir / 60f);
        dir = (CalcDir * 60) % 360;
        //クリック検知部分===========================
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                isClicked = hit.collider.gameObject.name.Equals(gameObject.name);
            }
            MouseInitPos = Input.mousePosition;
            VertexInit = mesh.vertices;
            PolygonVex = Polygon.points;
        } else if ((Input.GetMouseButtonUp(0)) && isClicked) {
            SetVertex(CalcDir);
        }
        if (!isClicked) return;
        //=============================

        //動作部分====================
        var distance = Vector3.Distance(MouseInitPos, Input.mousePosition);
        CalcPos.x = distance * Mathf.Sin(dir * Mathf.Deg2Rad) / screenSizeY;
        CalcPos.z = distance * Mathf.Cos(dir * Mathf.Deg2Rad) / screenSizeY;
        if (flipX) { CalcPos.x *= -1; }

        SlideVertex();

    }
    public void SetVertex(int CalcDir, bool isCollide = false) {
        Vector3 moveVec = Vector3.down;
        Debug.Log(CalcDir % 6);
        switch (CalcDir % 6) {
            case 1:
            case 5:
                moveVec.x = -Mathf.Sqrt(3.0f);
                moveVec.y = -0.5f;
                break;
            case 2:
            case 4:
                moveVec.x = Mathf.Sqrt(3.0f);
                moveVec.y = -0.5f;
                break;
        }
        if (flipX) { moveVec.x *= -1; }

        //Correct the position.
        if (!isCollide) {
            if (moveVec.x != 0) {
                var setPos = moveVec.x / 2f;
                CalcPos.x = Mathf.Round(CalcPos.x / setPos) * setPos;
            }
            if (moveVec.y != 0) {
                CalcPos.z = Mathf.Round(CalcPos.z / moveVec.y) * moveVec.y;
            }
            LineVertex.Add(CalcPos);
            PieceLine.SetVertex(LineVertex.ToArray());
        } else {
            CalcPos = Vector3.zero;
        }
        centerSet = true;
        isClicked = false;
        Col.isCollision = false;
        SlideVertex();
    }

    //When cleard stage,Use this.
    public void FadeOut() {
        StartCoroutine(FadeCorutine());
        isFadeOut = true;
    }
    IEnumerator FadeCorutine() {
        var material = GetComponent<Renderer>().materials[0];
        var COLOR = material.GetColor("_Color");
        float init = COLOR.a;
        for (float TIME = 0; TIME < 1f; TIME += Time.deltaTime) {
            COLOR.a -= init / (1f / Time.deltaTime);
            material.SetColor("_Color", COLOR);
            yield return null;
        }
    }
    void SlideVertex() {
        Vector3[] vertices = MFcache.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++) { vertices[i] = VertexInit[i] + CalcPos; }
        mesh.vertices = vertices;
        MFcache.mesh.vertices = vertices;
        meshcollision.sharedMesh = mesh;
        var Vec2D = Polygon.points;
        for (int i = 0; i < Vec2D.Length; i++) {
            Vec2D[i].x = PolygonVex[i].x + CalcPos.x;
            Vec2D[i].y = PolygonVex[i].y + CalcPos.z;
        }
        Polygon.points = Vec2D;
    }

    /// <summary>
    /// ピース生成。
    /// </summary>
    void CreateHex() {
        //頂点座標を格納するVeter3;
        var temp = new HexCoordinates(HexDirection.N, 0, 0);
        Vector3 v1 = Vector3.zero;
        Vector3 v2 = Vector3.zero;
        Vector3 v3 = Vector3.zero;
        v1 = temp.ToPosition();
        for (byte i = 0; i < 6; i++) {
            v2 = temp.Step((HexDirection)(i % 6)).ToPosition();
            v3 = temp.Step((HexDirection)((i + 1) % 6)).ToPosition();
            AddTriangle(v1, v2, v3);
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        meshcollision.sharedMesh = mesh;

        Vector2[] Collide = new Vector2[6];
        for (byte i = 0; i < Collide.Length; i++) {
            var angle01 = (30 + 60 * i) * Mathf.Deg2Rad;
            Collide[i].x = Mathf.Cos(angle01) * 0.75f;
            Collide[i].y = Mathf.Sin(angle01) * 0.75f;
        }
        Polygon.points = Collide;
    }
    /// <summary>
    /// 二点間の角度を求める。
    /// </summary>
    /// <param name="p1">点１</param>
    /// <param name="p2">点２</param>
    /// <returns>二点間の角度</returns>
    float GetDir(Vector3 p1, Vector3 p2) {
        float rad = Mathf.Atan2(p2.x - p1.x, p2.y - p1.y);
        return (rad * Mathf.Rad2Deg);
    }
}
