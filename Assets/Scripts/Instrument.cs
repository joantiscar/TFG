using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instrument : MonoBehaviour
{
    public GameObject channel;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Play(){
        Debug.Log("hit " + channel.GetComponent<Channel>().name);
        channel.GetComponent<Channel>().handleInput();
    }
}
