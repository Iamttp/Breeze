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

    void Start()
    {
        StartCoroutine(Creator());
    }

    void Update()
    {
        var obj = cludes.First;
        while (obj != null)
        {
            obj.Value.transform.position += (Vector3)((dirSpeed + Random.insideUnitCircle) * Time.deltaTime);
            if (obj.Value.transform.position.y > 20) // TODO 云消失位置
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
        for (int i = -20; i <= 20; i++)
        {
            if (Random.value > probably)
                cludes.AddLast(Instantiate(prefabClude, new Vector3(i, -20, 0) + (Vector3)Random.insideUnitCircle, Quaternion.identity));
        }
        StartCoroutine(Creator());
    }
}
