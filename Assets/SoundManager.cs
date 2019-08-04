using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource JumpAudio;
    public AudioSource LandAudio;
    public AudioSource ShootAudio;
    public AudioSource SpeedupAudio;
    public AudioSource SpeedDownAudio;
    public AudioSource EndLevelAudio;

    public void PlayAudio(AudioSource audioSource)
    {
        audioSource.Play();
    }
}
