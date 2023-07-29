using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedalsController : MonoBehaviour
{

    public DTXConverter converter;
    public Channel bassDrum;
    public GameObject HiHatClosed;
    public GameObject HiHatOpen;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            bassDrum.handleInput();

        }
        HiHatClosed.SetActive(!Input.GetKey(KeyCode.LeftShift));
        HiHatOpen.SetActive(Input.GetKey(KeyCode.LeftShift));
    }
}
