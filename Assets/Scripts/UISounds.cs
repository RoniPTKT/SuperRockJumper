using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISounds : MonoBehaviour
{
    private AudioSource uiAudio;
    [SerializeField] private AudioClip confirmSound;

    public void Start()
    {
        uiAudio = GetComponent<AudioSource>();
    }

    public void playSound()
    {
        Debug.Log("test");
        uiAudio.PlayOneShot(confirmSound, 1.0f);
    }
}
