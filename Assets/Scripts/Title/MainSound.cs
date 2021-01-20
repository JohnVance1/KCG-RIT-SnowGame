using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSound : MonoBehaviour
{
    private static MainSound sound;
    public static MainSound Sound { get { return sound; } }

    private void Awake()
    {
        if (sound != null && sound != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            sound = this;
            DontDestroyOnLoad(this);

        }


    }

   
}
