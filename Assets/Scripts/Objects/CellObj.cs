using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CellObj : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector2 orgPos;
    private Dictionary<string, PackageObject> objTable;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        orgPos = rectTransform.position;
        objTable = PackageManager.instance.objTable;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.red;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, getRectPos(eventData.position), eventData.enterEventCamera, out pos);
        rectTransform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<Image>().color = Color.white;
        createMapObj(Camera.main.ScreenToWorldPoint(rectTransform.position));
        rectTransform.position = orgPos;
    }

    private static Vector2 getRectPos(Vector2 pos)
    {
        Vector2 wpos = Camera.main.ScreenToWorldPoint(pos);
        wpos.x = Mathf.Round(wpos.x - 0.5f) + 0.5f;
        wpos.y = Mathf.Round(wpos.y - 0.5f) + 0.5f;
        return Camera.main.WorldToScreenPoint(wpos);
    }

    void createMapObj(Vector2 worldPos)
    {
        if (MapManager.instance.plants.ContainsKey(worldPos)) return;
        var mapName = objTable[name].mapName;
        if (mapName == null || mapName == "") return;

        var creator = Instantiate(MapManager.instance.mapObj, worldPos, Quaternion.identity);
        MapManager.instance.plants[worldPos] = creator;
        creator.GetComponent<MapObj>().typeName = mapName;
        creator.GetComponent<MapObj>().isDrag = true;
        objTable[name].num--;
    }

    void Update()
    {
        if (!objTable.ContainsKey(name) || objTable[name].num == 0)
        {
            GetComponent<Image>().color = Color.clear; // 透明
            GetComponentInChildren<Text>().enabled = false;
            return;
        }

        GetComponent<Image>().color = Color.white; // 透明
        if (objTable[name].sprite != null)
            GetComponent<Image>().sprite = objTable[name].sprite;
        GetComponentInChildren<Text>().enabled = true;
        GetComponentInChildren<Text>().text = objTable[name].num.ToString();
    }
}