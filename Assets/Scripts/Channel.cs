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
    public int instrumentChannel;
    public bool togleable = false;
    public bool togleValue = false;
    [ColorUsage(true, true)]
    public Color color;
    public float maxTimeLight = 0.1f;
    public Transform instrumentTransform;
    public GameObject notePrefab;
    public GameObject lightPrefab;
    public Transform LightsStart;


    DTXConverter c;
    // Start is called before the first frame update
    void Start()
    {
        c = converter.GetComponent<DTXConverter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving) transform.Translate(Vector3.Scale(Vector3.Scale(new Vector3(0, -1, 0), 
        (transform.parent.transform.localScale)), c.transform.localScale) * (float) Time.deltaTime * (float) BPM / (float) 240 * (float) c.noteSeparationValue);
        if (Input.GetKeyDown(button)) handleInput();

    }
    public IEnumerator TurnLightsOn(int c)
    {
        if (defaultChip != -1 && lightPrefab){
            Color col = color * 2;
            var a = Instantiate(lightPrefab, LightsStart);
            Material mat = lightPrefab.GetComponent<Renderer>().sharedMaterial;
            mat.EnableKeyword("_EMISSION");
            if (c == 1) col = a.GetComponent<TravelingLight>().Perfect;
            if (c == 2) col = a.GetComponent<TravelingLight>().Good;
            if (c == 3) col = a.GetComponent<TravelingLight>().Miss;
            mat.SetColor("_EmissionColor", col);
            yield return new WaitForSecondsRealtime(0.0125f);
        }   
    }
    public void handleInput()
    {
        int co = 0;
        if (notesContainer.transform.childCount > 0)
        {
            Nota a = notesContainer.transform.GetChild(0).GetComponent<Nota>();
            for (int i = 0; i < notesContainer.transform.childCount; i++)
            {
                Transform g = notesContainer.transform.GetChild(i);
                if (Vector3.Distance (g.transform.position, judgement.transform.position) 
                < (Vector3.Distance (a.transform.position, judgement.transform.position))) a = g.GetComponent<Nota>();
            }

                float distance = Math.Abs(Vector3.Distance (a.transform.position, judgement.transform.position));
                if (distance < 0.25)
                {
                    c.PerfectNote();
                    Destroy(a.gameObject);
                    co = 1;
                }
                else if (distance < 0.5)
                {
                    c.GoodNote();
                    Destroy(a.gameObject);
                    co = 2;
                }
                else if (distance < 0.75)
                {
                    c.MissedNote();
                    Destroy(a.gameObject);
                    co = 3;
                }
                defaultChip = a.objectNumber;
        }
        if (defaultChip != -1)
        {
            c.playChip(defaultChip);
            if (lightPrefab) StartCoroutine(TurnLightsOn(co));
        }
    }
}
