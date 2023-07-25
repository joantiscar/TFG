using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.Audio;

public class UiFrente : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public Game game;
    bool soundPlayed = false;
    private AudioSource player = null;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (game.time <= 4 && game.time >= 1){
            if (game.betweenRounds && !soundPlayed){
                player.Play();
                soundPlayed = true;
            }
            Text.text = Math.Floor(game.time).ToString();
        }else if (Math.Floor(game.time) == 0){
             Text.text = game.betweenRounds ? "Survive" : "Done!";
        }else{
            soundPlayed = false;
            player.Stop();
            Text.text = ""; 
        }
            
    }
}
