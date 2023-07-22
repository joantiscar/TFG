using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channel : MonoBehaviour
{
    public float BPM = 0;
    public bool moving = false;
    public GameObject converter;
    DTXConverter c;
    // Start is called before the first frame update
    void Start()
    {
        c = converter.GetComponent<DTXConverter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving){
            transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * BPM/240 * c.noteSeparationValue);
        }
    }
}
