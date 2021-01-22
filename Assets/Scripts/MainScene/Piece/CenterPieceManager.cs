using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CenterPieceManager : MonoBehaviour {
    [SerializeField] CollisionCheck2D[] CenterCollisions;
    [SerializeField] CenterPiece[] CenterPieces;
    [SerializeField] Transform BoardTrans;
    [SerializeField] RectTransform[] ImgTrans;
    [SerializeField] RectTransform BackCrystalTrans;
    bool isClear = false;

    public AudioClip pieceSet;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {

        if(CenterPieces[0].centerSet)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(pieceSet);
            CenterPieces[0].centerSet = false;
        }        

        if (isClear) return;//クリアしていたら処理をとばす。

        //中心から伸びているオブジェクトのうち一つでも何か邪魔なものに当たったら元に戻るようにする。
        for (byte i = 0; i < CenterCollisions.Length; i++) {
            if (CenterCollisions[i].isCollision) {
                for (byte j = 0; j < CenterPieces.Length; j++) {
                    CenterPieces[j].SetVertex(0, true);
                }
                return;
            }
        }

        //中心から伸びているオブジェクトがゴール(ステージの外に出たときクリア判定を送る。)
        if (CenterCollisions[0].isClear) {
            isClear = true;
            //Clear Flag On Color Change
            RenderLineManager.RenderLineManagerInstance.ClearFlag = true;

            RenderLineManager.RenderLineManagerInstance.RotationSpeed = 0.2f;

            for (byte j = 0; j < CenterPieces.Length; j++) {
                CenterPieces[j].FadeOut();
            }
            StartCoroutine(EndEffect());
        }
    }
    IEnumerator EndEffect() {
        Vector3 Scale = Vector3.zero;
        float Angle = 2f;
        for (float TIME = 0; TIME < 2f; TIME += Time.deltaTime) {
            BoardTrans.localScale = Vector3.one - Scale;
            for (int i = 0; i < ImgTrans.Length; i++) {
                ImgTrans[i].localScale = Scale;
            }
            BackCrystalTrans.localScale = Scale;
            BackCrystalTrans.Rotate(0, 0, Angle);
            Scale += Vector3.one / (2f / Time.deltaTime);
            Angle -= 1.8f / (2f / Time.deltaTime);
            yield return null;
        }
        BoardTrans.localScale = Vector3.one;
        BoardTrans.position = Vector3.one * 1000;
        for (int i = 0; i < ImgTrans.Length; i++) {
            ImgTrans[i].localScale = Vector3.one;
        }
        BackCrystalTrans.localScale = Vector3.one;
        while (true) {
            BackCrystalTrans.Rotate(0, 0, Angle);
            yield return null;
        }
    }
}
