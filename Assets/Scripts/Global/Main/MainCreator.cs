using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCreator : MonoBehaviour
{
    public static MainCreator instance;
    public GameObject mainScenePerfab;
    void Awake()
    {
        instance = this;

        Application.targetFrameRate = 60;
        //Screen.SetResolution(1920, 1080, true); // TODO 分辨率可变
    }

    void Start()
    {
        Instantiate(mainScenePerfab);
    }

    void Update()
    {

    }
}
