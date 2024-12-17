using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource myAudioSource;

    private void Awake()
    {
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }

        myAudioSource = GetComponent<AudioSource>();

    }

}
