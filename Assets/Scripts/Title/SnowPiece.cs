using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowPiece : MonoBehaviour {
    public RectTransform snow;
    public Text[] text;
    public Image[] image;
    float alfa = 0f;
    float ScreenScaleY = 1f;
    public GameObject Snoefall;
    void Start() {
        ScreenScaleY = 1080f / Screen.height;
        StartCoroutine(Snowstart());
    }
    void Update() {

    }
    IEnumerator Snowstart() {
        for (float i = 0.0f; i < 2.7f; i += Time.deltaTime) {
            snow.position += new Vector3(0, -850.0f * ScreenScaleY / (2.7f / Time.deltaTime) * (2.7f - i), 0);
            snow.Rotate(0, 0, -200.0f / (2.7f / Time.deltaTime) * (2.7f - i));
            yield return null;
        }

        Snoefall.SetActive(true);
        Color color = new Color(241f / 255f, 241f / 255f, 241f / 255f, alfa);
        for (float n = 0.0f; n < 1.5f; n += Time.deltaTime) {
            alfa += 1.0f / (1.5f / Time.deltaTime);
            color.a = alfa;
            for (int m = 0; m < text.Length; m++) {
                text[m].color = color;
            }
            for (int m = 0; m < image.Length; m++) {
                image[m].color = color;
            }
            yield return null;
        }
    }
}
