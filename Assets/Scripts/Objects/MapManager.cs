﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapObject
{
    public string typeName;
    public Sprite sprite;

    //[Header("为空表示无法收获")]
    public string packageName;
    //[Header("收获数目")]
    public int packageNum;

    //[Header("为空表示无法成长")]
    public string upMapName;
    //[Header("成长用时")]
    public int upSeconds;

    [HideInInspector]
    public System.DateTime plantTime;
}

public class MapManager : MonoBehaviour
{
    [Header("所有地图物品初始配置")]
    public List<MapObject> mObjs; // 所有物品初始配置 unity界面配置
    public Dictionary<string, MapObject> objTable = new Dictionary<string, MapObject>(); // 所有物品名称 -> 物品

    public static MapManager instance;
    public GameObject mapObj;
    public Dictionary<Vector2, GameObject> plants = new Dictionary<Vector2, GameObject>(); // 地图pos -> plant 防止重复放置

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        foreach (var obj in mObjs) objTable[obj.typeName] = obj;
    }

    void Update()
    {

    }
}
