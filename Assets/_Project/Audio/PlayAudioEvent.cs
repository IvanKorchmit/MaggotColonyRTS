using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioEvent : MonoBehaviour
{
    public AudioEvent audioEvent;
    public AudioSource source;

    private void Start()
    {
        source = Camera.main.GetComponent<AudioSource>();
    }

    public void Play()
    {
        audioEvent.Play(source);
    }
}
