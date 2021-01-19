using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackToTitle : MonoBehaviour {
    [SerializeField] Image Panel;
    public void OnClick() {
        StartCoroutine(BackTitle());
    }
    IEnumerator BackTitle() {
        var COL = Color.black;
        COL.a = 0f;
        for (float i = 0; i < 1f; i += Time.deltaTime) {
            COL.a = i;
            Panel.color = COL;
            yield return null;
        }
        Panel.color = Color.black;
        SceneManager.LoadScene("Title");
    }
}
