using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject swordPrefab;

    void Start()
    {
        // 创建Player
        var tempPlayer = Instantiate(swordPrefab);
        tempPlayer.name = "Player";
        tempPlayer.AddComponent<PlayerControll>();
        foreach (Transform child in tempPlayer.transform)
        {
            if (child.gameObject.name == "Circle")
            {
                child.GetComponent<SpriteRenderer>().color = Color.blue;
                break;
            }
        }

        // 创建Enemy
        var tempEnemy = Instantiate(swordPrefab);
        tempEnemy.AddComponent<ComputerControll>().followDis = 2;
        foreach (Transform child in tempEnemy.transform)
        {
            if (child.gameObject.name == "Circle")
            {
                child.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            }
        }
    }

    void Update()
    {

    }
}
