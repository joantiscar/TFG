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

    public void Play(int force){
        // TODO: multiples sonidos
        Debug.Log(gameObject.transform.parent.name);
        channel.GetComponent<Channel>().handleInput();
    }
}
