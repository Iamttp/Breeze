using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Begin : MonoBehaviour
{
    public Dropdown ModeDrop;
    public Dropdown MusicDrop;
    public static string _ip = "127.0.0.1";
    public static float musicVol = 0;
    void Start()
    {
    }

    void Update()
    {

    }

    public void openScene()
    {
        if (ModeDrop.value == 0)
            _ip = "127.0.0.1";
        else
            _ip = "39.97.171.148";
        if (MusicDrop.value == 0)
            musicVol = 0;
        else
            musicVol = 1;
        SceneManager.LoadScene(1);
    }
}
