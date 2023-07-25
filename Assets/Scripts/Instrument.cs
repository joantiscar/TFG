using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instrument : MonoBehaviour
{
    AudioSource sound;
    // Start is called before the first frame update
    void Start()
    {

        sound = GetComponent<AudioSource>();
    }

    public void Play(int force){
        // TODO: multiples sonidos
        sound.Play();
    }
}
