using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public enum MoveDirection {
    N_S, NE_SW, NW_SE
}
public class TrianglePeace : MeshGenBase {
    Vector3 moveVec = Vector3.up;
    MeshCollider meshcollision = null;//メッシュ当たり判定(レイを飛ばしてクリックを検知するため。)
    [SerializeField] PolygonCollider2D Polygon = null;//ポリゴン当たり判定(三角同士の当たり判定検知)
    [SerializeField] MeshRenderer Renderer = null;
    CollisionCheck2D Col = null;
    List<Vector2> BetweenVec = new List<Vector2>();
    MeshFilter MFcache = null;
    Camera mainCam;//カメラ
    bool isClicked = false;//今クリックされているかを格納する。

    public AudioClip triangleSet;
    AudioSource audioSource;

    //動かす際に必要。
    Vector3 CalcPos = Vector3.zero;//位置の計算に使う。
    float screenSizeY = 0;
    Vector3 MouseInitPos = Vector3.zero;//マウスの初期位置を
    Vector3[] VertexInit { get; set; }
    Vector2[] PolygonVex { get; set; }
    void Awake() {
        //CreateMesh
        mesh = new Mesh { name = gameObject.name };
        Polygon.gameObject.name = gameObject.name;
        //MeshSettings
        MFcache = GetComponent<MeshFilter>();
        MFcache.mesh = mesh;

        audioSource = GetComponent<AudioSource>();

        meshcollision = GetComponent<MeshCollider>();
        meshcollision.sharedMesh = mesh;
        Col = Polygon.GetComponent<CollisionCheck2D>();
        mainCam = Camera.main;

        screenSizeY = Screen.width / 4;
    }
    void Update() {
        //クリック検知部分===========================
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                isClicked = hit.collider.gameObject.name.Equals(gameObject.name);
            }
            MouseInitPos = Input.mousePosition;
            VertexInit = mesh.vertices;
        }
        //クリック離したとき検知。
        else if ((Input.GetMouseButtonUp(0) || Col.isCollision) && isClicked) {
            audioSource.PlayOneShot(triangleSet);
            SetVertice();
            isClicked = false;
            Col.isCollision = false;
        }
        if (!isClicked) return;
        //===============================================
        //ずれた位置計算。
        CalcPos = Input.mousePosition - MouseInitPos;
        CalcPos.z = CalcPos.y * moveVec.y;
        CalcPos.x = CalcPos.z * moveVec.x;
        CalcPos.y = 0;
        CalcPos /= screenSizeY;
        SlideVertice();
    }


    //Correct the position.
    void SetVertice() {
        if (moveVec.x != 0) {
            var setPos = moveVec.x / 2f;
            CalcPos.x = Mathf.Round(CalcPos.x / setPos) * setPos;
        }
        if (moveVec.y != 0) {
            CalcPos.z = Mathf.Round(CalcPos.z / moveVec.y) * moveVec.y;
        }
        if (Col.isCollision) {
            CalcPos = Vector3.zero;
            Col.isCollision = false;
        }
        SlideVertice();
    }


    //Displace the vertices.
    void SlideVertice() {
        Vector3[] vertices = MFcache.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++) { vertices[i] = VertexInit[i] + CalcPos; }
        mesh.vertices = vertices;
        MFcache.mesh.vertices = vertices;
        meshcollision.sharedMesh = mesh;
        var Vec2D = BetweenVec.ToArray();
        for (int i = 0; i < vertices.Length; i++) {
            Vec2D[i].x += vertices[i].x;
            Vec2D[i].y += vertices[i].z;
        }
        Polygon.points = Vec2D;
    }

    /// <summary>
    /// ピース生成。
    /// </summary>
    /// <param name="HC">どの位置を基準とするか。</param>
    /// <param name="length">ピースの長さ。(三角形)</param>
    /// <param name="moveDir">移動方向</param>
    public void CreatePiece(HexCoordinates HC, uint length = 1, MoveDirection moveDir = MoveDirection.N_S) {
        var material = GetComponent<Renderer>().materials[0];
        var COLOR = material.GetColor("_Color");
        Debug.Log(COLOR);
        switch (moveDir) {
            case MoveDirection.NE_SW:
                COLOR.r = 0.5f;
                COLOR.g = 0.0f;
                COLOR.b = 0.0f;
                moveVec.x = Mathf.Sqrt(3.0f);
                moveVec.y = 0.5f;
                break;
            case MoveDirection.NW_SE:
                COLOR.r = 0.0f;
                COLOR.g = 0.5f;
                COLOR.b = 0.0f;
                moveVec.x = -Mathf.Sqrt(3.0f);
                moveVec.y = 0.5f;
                break;
        }
        Debug.Log(COLOR);
        material.SetColor("_Color", COLOR);
        var temp = HC;
        Vector2 between = Vector2.zero;
        //頂点座標を格納するVeter3;
        Vector3 v1 = Vector3.zero;
        Vector3 v2 = Vector3.zero;
        Vector3 v3 = Vector3.zero;
        //NE,SW方向に移動するか。(この移動方向に限って生成する方法少し変わるので・・・)
        bool isNESW = moveDir.Equals(MoveDirection.NE_SW);

        //足す方向設定。
        HexDirection addDir = HexDirection.S;
        switch (moveDir) {
            case MoveDirection.NW_SE:
                addDir = HexDirection.SE;
                break;
            case MoveDirection.NE_SW:
                addDir = HexDirection.SW;
                break;
        }

        //PieceGenerate
        for (int len = 0; len < length; len++) {
            v1 = temp.ToPosition();
            if (len % 2 == 0) {
                v2 = temp.Step(HexDirection.N).ToPosition();
                v3 = temp.Step(HexDirection.NE).ToPosition();

                //v1
                between.x = 0.05f;
                between.y = 0.05f;
                BetweenVec.Add(between);
                //v2
                between.x = 0.05f;
                between.y = -0.05f;
                BetweenVec.Add(between);
                //v3
                between.x = -0.05f;
                between.y = 0;
                BetweenVec.Add(between);

            } else {
                if (isNESW) {
                    v2 = temp.Step(HexDirection.NW).ToPosition();
                    v3 = temp.Step(HexDirection.N).ToPosition();
                    //v1
                    between.x = -0.05f;
                    between.y = +0.05f;
                    BetweenVec.Add(between);
                    //v2
                    between.x = +0.05f;
                    between.y = 0;
                    BetweenVec.Add(between);
                    //v3
                    between.x = -0.05f;
                    between.y = -0.05f;
                    BetweenVec.Add(between);

                } else {
                    v2 = temp.Step(HexDirection.NE).ToPosition();
                    v3 = temp.Step(HexDirection.SE).ToPosition();
                    //v1
                    between.x = 0.05f;
                    between.y = 0;
                    BetweenVec.Add(between);
                    //v2
                    between.x = -0.05f;
                    between.y = -0.05f;
                    BetweenVec.Add(between);
                    //v3
                    between.x = -0.05f;
                    between.y = +0.05f;
                    BetweenVec.Add(between);
                }
                temp += addDir;
            }
            AddTriangle(v1, v2, v3);
        }
        if (temp.ToPosition().Equals(Vector3.zero)) {
            Debug.LogError("CollisionError:This is the initial point.\n" + "CollisionObjName:" + gameObject.name);
        }
        Vector3[] Array = vertices.ToArray();

        mesh.vertices = Array;
        mesh.triangles = triangles.ToArray();
        meshcollision.sharedMesh = mesh;
        var Vec2D = BetweenVec.ToArray();
        for (int i = 0; i < Array.Length; i++) {
            Vec2D[i].x += Array[i].x;
            Vec2D[i].y += Array[i].z;
        }
        Polygon.points = Vec2D;
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
