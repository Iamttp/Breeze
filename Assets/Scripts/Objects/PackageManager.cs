using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 背包物品
/// TODO 背包界面
/// </summary>
[System.Serializable]
public class PackageObject
{
    public string typeName;
    public Sprite sprite;

    //[Header("背包拥有的物品数目")]
    public int num;
    //[Header("为空表示无法种植")]
    public string mapName;
}

public class PackageManager : MonoBehaviour
{
    [Header("所有背包物品初始配置")]
    public List<PackageObject> pObjs; // 所有物品初始配置 unity界面配置
    public Dictionary<string, PackageObject> objTable = new Dictionary<string, PackageObject>(); // 所有物品名称 -> 物品

    [Header("底部栏设置")]
    public List<GameObject> downCells;

    public static PackageManager instance;
    [HideInInspector]
    private int cellCount = 5;

    void Awake()
    {
        instance = this;
    }

    private int nowPage = 0;
    public void AddPage()
    {
        nowPage++;
        if (nowPage > 2) nowPage = 2;
        updateDownCell();
    }

    public void decPage()
    {
        nowPage--;
        if (nowPage < 0) nowPage = 0;
        updateDownCell();
    }

    public void setPage(int page)
    {
        if (page < 0 || page > 2) return;
        nowPage = page;
        updateDownCell();
    }

    private void updateDownCell()
    {
        for (int i = 0; i < cellCount; i++)
        {
            var index = i + nowPage * cellCount;
            if (index < pObjs.Count) downCells[i].name = pObjs[index].typeName;
            else downCells[i].name = "";
        }
    }

    void Start()
    {
        foreach (var obj in pObjs) objTable[obj.typeName] = obj;
        updateDownCell();
    }

    void Update()
    {

    }
}
