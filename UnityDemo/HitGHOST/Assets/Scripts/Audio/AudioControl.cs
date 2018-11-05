using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    private AudioControl() { }
    private void Awake()
    {
        if (SingletonControl.Instance.audioControl != null)
        {
            if (SingletonControl.Instance.audioControl != this)
                DestroyImmediate(this.gameObject);
        }
        else
        {
            SingletonControl.Instance.audioControl = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    [SerializeField]
    private AudioClip back_audioclip;
    [SerializeField]
    private AudioClip fire_audioclip;
    [SerializeField]
    private AudioClip kick_audioclip;

    /// <summary>
    /// 播放主背景音乐，其他音效使用PlayOneShot播放。可以并存
    /// </summary>
    [SerializeField]
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource.clip = back_audioclip;
        audioSource.mute = !SingletonControl.Instance.gameManager.musicon;

        audioSource.Play();
    }

    private void Update()
    {
        if (audioSource.mute == SingletonControl.Instance.gameManager.musicon)
            audioSource.mute = !SingletonControl.Instance.gameManager.musicon;
    }

    public void PlayFire()
    {
        //指马上播放一个音乐且只播放一次，同时Pause和Stop对其无效
        audioSource.PlayOneShot(fire_audioclip);
    }

    public void PlayKick()
    {
        //指马上播放一个音乐且只播放一次，同时Pause和Stop对其无效
        audioSource.PlayOneShot(kick_audioclip);
    }
}
