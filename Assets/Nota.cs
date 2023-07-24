using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nota : MonoBehaviour
{
    public int objectNumber = 0;
    public int objectChannel = 0;
    public DTXConverter DTXConverter;
    public GameObject FootIcon;
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
            }else if (objectChannel == DTXConverter.eBGMChannel){
                DTXConverter.playChip(objectNumber);
            }else if (DTXConverter.auto) DTXConverter.playChip(objectNumber);
            
        }else if (other.name == "DeathLine"){
            DTXConverter.IncreaseScore(-300);
            Destroy(gameObject);
        }
    }
}
