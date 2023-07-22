using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UiPeus : MonoBehaviour
{
    public TextMeshProUGUI Puntuacio;
    public TextMeshProUGUI Ronda;
    public TextMeshProUGUI Rellotge;
    public TextMeshProUGUI HighScore;
    public Game game;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Puntuacio.text = game.score.ToString();
        double floor = Math.Floor(game.time);
        Rellotge.text = "0:" + (floor < 10 ? "0" + floor.ToString() : floor.ToString());
        Ronda.text = "Round " + game.round.ToString();
    }
}
