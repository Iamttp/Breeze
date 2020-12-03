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
    public Dictionary<string, int> objToCell = new Dictionary<string, int>(); // 物品名称 -> cell索引

    public static PackageManager instance;

    void Awake()
    {
        instance = this;
    }

    public void updateDownCell()
    {
        int cellIndex = 0;
        foreach (var obj in pObjs)
        {
            //if (obj.num == 0) continue;
            downCells[cellIndex++].name = obj.typeName;
            objToCell[obj.typeName] = cellIndex - 1;
            if (cellIndex >= downCells.Count) break;
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
