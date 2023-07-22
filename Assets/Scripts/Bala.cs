using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VolumetricLines;

public class Bala : MonoBehaviour
{

    public GameObject prefabAudioExplosion;
    public AudioSource audioSourceTravelling;
    public Game game;

    private bool hasInstantiateExplosion;
    float speed = 20f;
    public Vector3 target;
    public Color[] possibleColors;


    // Start is called before the first frame update
    void Start()
    {
        Color color = possibleColors[Random.Range(0, possibleColors.Length)];
        gameObject.GetComponent<VolumetricLineBehavior>().LineColor = color;
        StartCoroutine(StartAudio());
        hasInstantiateExplosion = false;
    }

    private IEnumerator StartAudio()
    {
        yield return new WaitForSeconds(2);
        audioSourceTravelling.Play();
    }


    // Update is called once per frame
    void Update()
    {
        var step =  speed * Time.deltaTime; 
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.name == "EspadaIzquierda" || other.name == "EspadaDerecha")
        {
            if(!hasInstantiateExplosion)
            {
                hasInstantiateExplosion = true;
                Instantiate(prefabAudioExplosion);
            }
            GameObject.Find("GameHandler").GetComponent<Game>().AddScore();
            Debug.Log("Boom");
            Destroy(transform.gameObject);
        }
        else if (other.name == "PlanoDeLaMuerte")
        {
            if (!hasInstantiateExplosion)
            {
                hasInstantiateExplosion = true;
                Instantiate(prefabAudioExplosion);
            }
            Destroy(transform.gameObject);
            game.GameOver();
        }
    }

}
