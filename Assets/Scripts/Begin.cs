using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Begin : MonoBehaviour
{
    public Dropdown dropdown;
    public static string _ip = "127.0.0.1";
    void Start()
    {
    }

    void Update()
    {

    }

    public void openScene()
    {
        if (dropdown.value == 0)
            _ip = "127.0.0.1";
        else
            _ip = "39.97.171.148";
        SceneManager.LoadScene(1);
    }
}
