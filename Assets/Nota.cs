using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nota : MonoBehaviour
{
    public int objectNumber;
    public int objectChannel;
    public DTXConverter DTXConverter;
    public float speed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "JudgementLine"){
            if (objectChannel == DTXConverter.eBPMChannel){
                DTXConverter.changeBPM(objectNumber);
            }else{
                DTXConverter.playChip(objectNumber);
            }
            
        }else if (other.name == "DeathLine"){
            Destroy(gameObject);
        }
    }
}
