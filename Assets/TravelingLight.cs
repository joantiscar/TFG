using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelingLight : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color Perfect;
    [ColorUsage(true, true)]
    public Color Good;
    [ColorUsage(true, true)]
    public Color Miss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            transform.Translate(Vector3.Scale(new Vector3(0, 0, -40), (transform.parent.transform.localScale) ) * Time.deltaTime);
            if (transform.localPosition.y < -80) Destroy(gameObject);
    }
}
