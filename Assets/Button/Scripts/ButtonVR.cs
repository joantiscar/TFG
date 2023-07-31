/**************************************************
Copyright : Copyright (c) RealaryVR. All rights reserved.
Description: Script for VR Button functionality.
***************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonVR : MonoBehaviour
{
    public GameObject button;
    public UnityEvent onPress;
    public UnityEvent onRelease;
    AudioSource sound;
    public GameObject cage;
    bool isPressed1;
    bool isPressed2;

    void Start()
    {
        sound = GetComponent<AudioSource>();
        isPressed1 = false;
        isPressed2 = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPressed1 && other.name.Equals("Baqueta1") && other.gameObject.GetComponent<Rigidbody>().velocity.y < -0.1)
        {
            onPress.Invoke();
            if (sound != null) sound.Play();
            isPressed1 = true;
        }
        if (!isPressed2 && other.name.Equals("Baqueta2") && other.gameObject.GetComponent<Rigidbody>().velocity.y < -0.1)
        {
            onPress.Invoke();
            if (sound != null) sound.Play();
            isPressed2 = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Equals("Baqueta1"))
        {
            onRelease.Invoke();
            isPressed1 = false;
        }
        if (other.name.Equals("Baqueta2"))
        {
            onRelease.Invoke();
            isPressed2 = false;
        }
    }
}
