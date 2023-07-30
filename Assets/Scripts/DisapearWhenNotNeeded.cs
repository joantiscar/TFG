using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisapearWhenNotNeeded : MonoBehaviour
{
    MeshRenderer m;
    public Transform judgementLine;
    // Start is called before the first frame update
    void Start()
    {
        m = this.GetComponent<MeshRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //if (transform.position.y < transform.parent.parent.position.y) Destroy(gameObject);
        //m.enabled = (transform.parent.localPosition.y + transform.localPosition.y) < 40;
    }
}
