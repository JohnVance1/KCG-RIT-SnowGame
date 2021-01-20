using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackToTitle : MonoBehaviour {
    [SerializeField] Image Panel;
    public AudioClip title;
    AudioSource audioSource;
    

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void OnClick() {
        StartCoroutine(BackTitle());
        audioSource.PlayOneShot(title);

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
