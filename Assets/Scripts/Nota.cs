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
    public Channel channel;
    public MeshRenderer m;
    public bool visible = true;

    // Start is called before the first frame update
    void Start()
    {
        channel = gameObject.transform.parent.transform.parent.GetComponent<Channel>();
        m.enabled = visible;
    }

    // Update is called once per frame
    void Update()
    {
        if (visible) m.enabled = (transform.parent.transform.parent.localPosition.y + transform.localPosition.y) < 40;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == channel.judgement){
            if (objectChannel == DTXConverter.eBPMChannel){
                DTXConverter.changeBPM(objectNumber);
                Destroy(gameObject);
            }else if (objectChannel == DTXConverter.eBGMChannel){
                DTXConverter.playChip(objectNumber);
                Destroy(gameObject);
            }else if (DTXConverter.auto) {
                DTXConverter.playChip(objectNumber);
                DTXConverter.PerfectNote();
                Destroy(gameObject);
                channel.StartCoroutine(channel.TurnLightsOn());
            }
            
        }else if (other.gameObject == channel.death){
            DTXConverter.MissedNote();
            Debug.Log("Missed note: objectNumber " + objectNumber + "   objectChannel " + objectChannel);
            Destroy(gameObject);
        }
    }
}
