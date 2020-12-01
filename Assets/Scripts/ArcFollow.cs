using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcFollow : MonoBehaviour
{
    public bool Owner;
    public Vector3 target;
    public float speed;
    public int attackVal;

    void Start()
    {
        Vector3 targetDir = target - transform.position;
        float angle = Vector3.Angle(transform.right, targetDir);
        Vector3 cross = Vector3.Cross(transform.right, targetDir);
        transform.Rotate(new Vector3(0, 0, angle * (cross.z > 0 ? 1 : -1)));
    }

    private float nowSpeed = 0;
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, nowSpeed * Time.deltaTime);
        nowSpeed += speed * Time.deltaTime;
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var otherP = collision.gameObject.GetComponent<IPerson>();
        if (otherP == null) return;
        if (otherP.Owner == Owner) return;
        otherP.hit(attackVal);
        Destroy(gameObject);
    }
}
