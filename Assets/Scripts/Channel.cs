using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Channel : MonoBehaviour
{
    public float BPM = 0;
    public bool moving = false;
    public GameObject converter;
    public GameObject judgement;
    public GameObject death;
    public GameObject notesContainer;
    public int defaultChip = -1;
    public KeyCode button;
    public KeyCode button2;
    public int instrumentChannel;
    public bool togleable = false;
    public bool togleValue = false;
    [ColorUsage(true, true)]
    public Color color;
    public GameObject middle;
    public float maxTimeLight = 0.1f;
    public Transform instrumentTransform;
    public GameObject notePrefab;
    DTXConverter c;
    // Start is called before the first frame update
    void Start()
    {
        c = converter.GetComponent<DTXConverter>();

    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            transform.Translate(Vector3.Scale(Vector3.Scale(new Vector3(0, -1, 0), (transform.parent.transform.localScale)), c.transform.localScale) * Time.deltaTime * BPM / 240 * c.noteSeparationValue );
        }
        if (Input.GetKeyDown(button))
        {
            handleInput();

        }
    }


    public IEnumerator TurnLightsOn()
    {
            Material mat = middle.GetComponent<Renderer>().material;
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", color / 4);
            yield return new WaitForSecondsRealtime(0.0125f);
            mat.SetColor("_EmissionColor", color / 2);
            yield return new WaitForSecondsRealtime(0.0125f);
            mat.SetColor("_EmissionColor", color);
            yield return new WaitForSecondsRealtime(0.0125f);
            mat.SetColor("_EmissionColor", color / 2);
            yield return new WaitForSecondsRealtime(0.0125f);
            mat.SetColor("_EmissionColor", color / 4);
            yield return new WaitForSecondsRealtime(0.0125f);
            mat.DisableKeyword("_EMISSION");
    }

    public void handleInput(){
        if (!togleable || (togleValue == false && !Input.GetKey(button2)) || togleValue == true && Input.GetKey(button2))
            {
                if (notesContainer.transform.childCount > 0)
                {
                    Nota a = notesContainer.transform.GetChild(0).GetComponent<Nota>();
                    for (int i = 0; i < notesContainer.transform.childCount; i++)
                    {
                        Transform g = notesContainer.transform.GetChild(i);
                        if (g.position.y < a.transform.position.y) a = g.GetComponent<Nota>();
                    }


                    if (a.DTXConverter)
                    {
                        double distance = Math.Abs(a.transform.position.y - (judgement.transform.position.y + 0.5));
                        if (distance < 0.5)
                        {
                            c.PerfectNote();
                            Destroy(a.gameObject);
                        }
                        else if (distance < 1)
                        {
                            c.GoodNote();
                            Destroy(a.gameObject);
                        }
                        else if (distance < 1.5)
                        {
                            c.MissedNote();
                            Destroy(a.gameObject);
                        }
                        defaultChip = a.objectNumber;
                    }
                    else
                    {
                        Destroy(a.gameObject);
                    }
                }

                if (defaultChip != -1) {
                    c.playChip(defaultChip);
                    StartCoroutine(TurnLightsOn());
                }
                
            }
    }
}
