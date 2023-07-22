using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedalsController : MonoBehaviour
{

    public Instrument z;
    public Instrument x;
    public Instrument c;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)){
            z.Play(1);
        }if (Input.GetKeyDown(KeyCode.X)){
            x.Play(1);
        }if (Input.GetKeyDown(KeyCode.C)){
            c.Play(1);
        }   
    }
}
