using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAfterSomeTime : MonoBehaviour
{
    [SerializeField] private float time;
    [SerializeField] private AudioSource audioSource;
    private void Start()
    {
        TimerUtils.AddTimer(time, () => audioSource.Play());
    }
}
