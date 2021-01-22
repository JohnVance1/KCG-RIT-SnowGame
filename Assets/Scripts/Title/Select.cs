using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class Select : MonoBehaviour {
    public Image image1;
    public Image image2;
    public AudioClip start;
    public AudioClip exit;
    AudioSource audioSource;
    void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    public void SelectPos(int posY) {
        image1.rectTransform.localPosition = new Vector3(80, posY, 0);
        image2.rectTransform.localPosition = new Vector3(-80, posY, 0);
    }
    public void GameStart(string scene) {
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(start);
            StartCoroutine(Checking(audioSource, () => {
                SceneManager.LoadScene(scene); // Will change once merged
            }));
        }
    }
    public void GameExit() {
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(exit);
            StartCoroutine(Checking(audioSource, () => {

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }));
        }
    }
    private IEnumerator Checking(AudioSource audio, UnityAction callback) {
        while (true) {
            yield return new WaitForFixedUpdate();
            if (!audio.isPlaying) {
                callback();
                break;
            }
        }
    }
}
