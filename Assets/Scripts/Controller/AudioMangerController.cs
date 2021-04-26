using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMangerController : MonoBehaviour
{
    public static AudioMangerController instance;
    public AudioSource HurtAudio;
    public AudioSource JumpAudio;
    public AudioSource ItemAudio;
    public AudioSource DeathAudio;
    public AudioSource BackGroundAudio;

    private void Awake()
    {
        instance = this;
    }

    public void Hurt()
    {
        HurtAudio.Play();
    }

    public void Jump()
    {
        JumpAudio.Play();
    }

    public void Item()
    {
        ItemAudio.Play();
    }
    public void Death()
    {
        DeathAudio.Play();
    }

    public void Disable()
    {
        foreach (AudioSource audioSource in GetComponents<AudioSource>())
        {
            audioSource.enabled = false;
        }
        DeathAudio.enabled = true;
    }
}
