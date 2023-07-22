using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : MonoBehaviour
{

    private const string GAME_SCENE_NAME = "GameScene";
    public string difficulty = "EASY";

    private float wait = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (wait > 0)
            wait -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (wait <= 0)
        {
            if (other.name == "EspadaIzquierda" || other.name == "EspadaDerecha")
            {
                if (difficulty.ToUpper().Equals("MENU"))
                {
                    GameObject.Find("Utils").GetComponent<Utils>().LoadScene("MainMenu");
                }
                else if (difficulty.ToUpper().Equals("EXIT"))
                {
                    Application.Quit();
                }
                else
                {
                    Singleton.GetInstance().SetDifficult(difficulty);
                    GameObject.Find("Utils").GetComponent<Utils>().LoadScene(GAME_SCENE_NAME);
                }
            }
        }
    }

}
