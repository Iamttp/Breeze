using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public GameObject showF;
    public string typeName;
    void Start()
    {

    }

    void Update()
    {
        if (!showF.activeSelf) return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            Destroy(gameObject);
            PackageManager.instance.objNum[typeName] += 2;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        var otherP = collision.gameObject;
        if (otherP.name == "Player")
        {
            showF.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        showF.SetActive(false);
    }
}