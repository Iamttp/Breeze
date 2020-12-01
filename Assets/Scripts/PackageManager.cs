﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackageManager : MonoBehaviour
{
    public GameObject plant;
    public Dictionary<Vector2, GameObject> plants = new Dictionary<Vector2, GameObject>(); // pos -> plant 防止重复放置

    public List<GameObject> downCells;

    public struct Obj // 物品属性
    {

    }
    public Dictionary<string, Obj> objTable = new Dictionary<string, Obj>();
    public Dictionary<string, int> objNum = new Dictionary<string, int>();


    public static PackageManager instance;
    void Awake()
    {
        instance = this;
        objNum["Pumpkin"] = 2;
        objNum["Carrot"] = 1;
    }

    void Start()
    {
    }

    void Update()
    {
        foreach (var cell in downCells)
        {
            if (objNum[cell.name] <= 0) cell.SetActive(false);
            else cell.SetActive(true);
            cell.GetComponentInChildren<Text>().text = objNum[cell.name].ToString();
        }
    }
}
