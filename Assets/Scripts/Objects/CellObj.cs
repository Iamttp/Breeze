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
    private bool isOnDrag;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        orgPos = rectTransform.position;
        objTable = PackageManager.instance.objTable;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isOnDrag = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, getRectPos(eventData.position), eventData.enterEventCamera, out pos);
        rectTransform.position = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        createMapObj(Camera.main.ScreenToWorldPoint(getRectPos(rectTransform.position)), transform.rotation);
        rectTransform.position = orgPos;
        transform.rotation = Quaternion.identity;
        isOnDrag = false;
    }

    private static Vector2 getRectPos(Vector2 pos)
    {
        Vector2 wpos = Camera.main.ScreenToWorldPoint(pos);
        wpos.x = Mathf.Round(wpos.x - 0.5f) + 0.5f;
        wpos.y = Mathf.Round(wpos.y - 0.5f) + 0.5f;
        return Camera.main.WorldToScreenPoint(wpos);
    }

    void createMapObj(Vector2 worldPos, Quaternion quaternion)
    {
        var ic = MapManager.instance;
        if (ic.plants.ContainsKey(worldPos)) return;
        var mapName = objTable[name].mapName;
        if (mapName == null || mapName == "") return;

        GameObject creator;
        if (ic.mapNameToPrefab.ContainsKey(mapName))
        {
            creator = Instantiate(ic.mapNameToPrefab[mapName], worldPos, quaternion);
        }
        else
        {
            creator = Instantiate(ic.mapObjPrefab, worldPos, quaternion);
        }
        ic.plants[worldPos] = creator;
        creator.GetComponent<MapObj>().typeName = mapName;
        creator.GetComponent<MapObj>().isDrag = true;
        objTable[name].num--;
    }
    
    void Update()
    {
        if (isOnDrag)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                transform.Rotate(new Vector3(0, 0, 1), 90);
            }
            GetComponentInChildren<Text>().enabled = false;
            GetComponent<Image>().color = Color.white / 2;
        }
        else
        {
            UpdateObj();
        }
    }

    public void UpdateObj()
    {
        if (!objTable.ContainsKey(name) || objTable[name].num == 0)
        {
            GetComponent<Image>().color = Color.clear; // 透明
            GetComponentInChildren<Text>().enabled = false;
            return;
        }

        GetComponent<Image>().color = Color.white;
        if (objTable[name].sprite != null)
            GetComponent<Image>().sprite = objTable[name].sprite;
        GetComponentInChildren<Text>().enabled = true;
        GetComponentInChildren<Text>().text = objTable[name].num.ToString();
    }
}