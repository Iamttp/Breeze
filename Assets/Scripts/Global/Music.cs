using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    //音源AudioSource相当于播放器，而音效AudioClip相当于磁带
    [HideInInspector]
    public AudioSource music;
    private AudioClip back;
    private AudioClip destory;
    private AudioClip attack;
    public static Music instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //给对象添加一个AudioSource组件
            music = gameObject.AddComponent<AudioSource>();
            music.volume = Begin.musicVol;
            back = Resources.Load<AudioClip>("Music/back");
            destory = Resources.Load<AudioClip>("Music/destory");
            attack = Resources.Load<AudioClip>("Music/attack");
            playBack();
        }
    }

    void Update()
    {
    }

    public void playBack()
    {
        if (music.clip == back) return;
        music.loop = true;
        music.clip = back;
        music.Play();
    }

    public void playDestory()
    {
        music.PlayOneShot(destory);
    }

    public void playAttack()
    {
        music.PlayOneShot(attack);
    }
}
