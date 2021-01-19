using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Snowfall : MonoBehaviour
{
    Image image;
    RectTransform trans;
    public float LifeTime = 2.5f;//������܂ł̎���
    public float MaxSpeed = 300.0f;//��ԑ����X�s�[�h
    public float MinSpeed = 100.0f;//��Ԓx���X�s�[�h
    float minAngle = 90.0f - 20.0f;//�ŏ��p�x
    float maxAngle = 90.0f + 20.0f;//�ő�p�x
    float MaxScale = 2.0f;//�ő�̑傫��
    float LightSize = 30f;
    Color SnowColor = Color.white;
    Vector2 MoveVec;
    void Awake()
    {
        image = GetComponent<Image>();
        trans = GetComponent<RectTransform>();

    }
    private void Update()
    {
        image.color = SnowColor;
        SnowColor.a -= 1f / (LifeTime / Time.deltaTime);
        Vector3 pos = trans.position;
        trans.position = new Vector3(pos.x - MoveVec.x / (LifeTime / Time.deltaTime), pos.y - MoveVec.y / (LifeTime / Time.deltaTime), pos.z);
        if (SnowColor.a <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    public void InitObj(float X, float Y, float END_X, float END_Y)
    {
        float angle = Random.Range(minAngle, maxAngle) * Mathf.Deg2Rad;
        float speed = Random.Range(MinSpeed, MaxSpeed);
        float scale = Random.Range(1.0f, MaxScale);
        trans.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        trans.sizeDelta = new Vector2(
            LightSize * scale,
            LightSize * scale
            );
        trans.position = new Vector3(
    Random.Range(X, END_X),
    Random.Range(Y, END_Y),
    0);
        MoveVec = new Vector2(
            speed * Mathf.Cos(angle),
            speed * Mathf.Sin(angle)
            );
        SnowColor.a = 1f;
    }
}
