using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CludeCreator : MonoBehaviour
{
    public GameObject prefabClude;
    public List<Sprite> cludeSprites;

    public Vector2 dirSpeed;
    public float addTime;
    public float probably;
    private LinkedList<GameObject> cludes = new LinkedList<GameObject>();
    const int size = 30;  // TODO 云消失位置

    void Start()
    {
        StartCoroutine(Creator());
        // 先随机在场景中生成云
        int numStart = 10;
        while (numStart-- > 0)
        {
            cludes.AddLast(Instantiate(prefabClude, new Vector3(Random.Range(-size, size), Random.Range(-size, size), 0)
                , Quaternion.identity, transform));
        }
    }

    void Update()
    {
        var obj = cludes.First;
        while (obj != null)
        {
            obj.Value.transform.position += (Vector3)((dirSpeed + Random.insideUnitCircle) * Time.deltaTime);
            if (obj.Value.transform.position.y > size)
            {
                Destroy(obj.Value);

                var temp = obj;
                obj = obj.Next;
                cludes.Remove(temp);
            }
            else
            {
                obj = obj.Next;
            }
        }
    }

    IEnumerator Creator()
    {
        yield return new WaitForSeconds(Random.Range(addTime / 2, addTime));

        foreach (var obj in cludes)
            obj.GetComponent<SpriteRenderer>().sprite = cludeSprites[Random.Range(0, cludeSprites.Count)];
        for (int i = -size; i <= size; i++)
        {
            if (Random.value > probably)
                cludes.AddLast(Instantiate(prefabClude, new Vector3(i, -size, 0) + (Vector3)Random.insideUnitCircle, Quaternion.identity, transform));
        }
        StartCoroutine(Creator());
    }
}
