using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadScore : MonoBehaviour
{

    private TMP_Text text;


    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetComponent<TMP_Text>();
        int diff = Singleton.GetInstance().GetDifficulty();
        string difficulty = "EASY";
        if (diff == 1) difficulty = "NORMAL";
        else if (diff == 2) difficulty = "HARD";
        string name = Singleton.GetInstance().GetCurrentName();
        int score = Singleton.GetInstance().GetCurrentScore();
        text.text = "Difficulty = " + difficulty + "\n" + 
            "Name = " + name + "\n" + 
            "Score = " + score;
    }

}
