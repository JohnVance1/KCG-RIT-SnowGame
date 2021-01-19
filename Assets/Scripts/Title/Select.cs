using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Select : MonoBehaviour
{
    public Image image1; 
    public Image image2;
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
        SceneManager.LoadScene("Main");
    }
    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}
