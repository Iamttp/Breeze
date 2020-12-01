using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [Header("")]
    public GameObject leftObj;
    public GameObject rightObj;
    public GameObject upObj;
    public GameObject downObj;
    
    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject upDoor;
    public GameObject downDoor;

    [HideInInspector]
    public bool left;
    [HideInInspector]
    public bool right;
    [HideInInspector]
    public bool up;
    [HideInInspector]
    public bool down;

    void Start()
    {
        leftDoor.SetActive(!left);
        rightDoor.SetActive(!right);
        upDoor.SetActive(!up);
        downDoor.SetActive(!down);
        
        leftObj.SetActive(left);
        rightObj.SetActive(right);
        upObj.SetActive(up);
        downObj.SetActive(down);
    }

    void Update()
    {
        
    }
}
