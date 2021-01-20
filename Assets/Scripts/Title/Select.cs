using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class Select : MonoBehaviour
{
    public Image image1;
    public Image image2;
    public AudioClip start;
    public AudioClip exit;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void SelectStart()
    {
        image1.rectTransform.localPosition = new Vector3(80, -140, 0);
        image2.rectTransform.localPosition = new Vector3(-80, -140, 0);
    }
    public void SelectExit()
    {
        image1.rectTransform.localPosition = new Vector3(80, -180, 0);
        image2.rectTransform.localPosition = new Vector3(-80, -180, 0);
    }
    public void GameStart()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(start);
            StartCoroutine(Checking(audioSource, () =>
            {
                SceneManager.LoadScene("Main");
            }));
        }
    }
    public void GameExit()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(exit);
            StartCoroutine(Checking(audioSource, () =>
            {

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }));
        }
    }
    private IEnumerator Checking(AudioSource audio, UnityAction callback)
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!audio.isPlaying)
            {
                callback();
                break;
            }
        }
    }
}
