using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    public float speed;
    Rigidbody2D rg;
    Animator anim;

    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    Vector2 move;

    void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        // 动画翻转
        if (move.x != 0)
        {
            var vec = transform.localScale;
            transform.localScale = new Vector3(move.x * Mathf.Abs(vec.x), vec.y, vec.z); // -1 翻转
        }
        anim.SetFloat("speed", move.magnitude);

        if (move.x != 0 || move.y != 0)
        {
            int x = Mathf.RoundToInt(transform.position.x / RoomCreator.instance.xOffset);
            int y = Mathf.RoundToInt(transform.position.y / RoomCreator.instance.yOffset);
            CameraFollow.instance.target = new Vector2(x * RoomCreator.instance.xOffset, y * RoomCreator.instance.yOffset);
        }
    }

    private void FixedUpdate()
    {
        rg.MovePosition(rg.position + move * speed * Time.fixedDeltaTime);
    }
}
