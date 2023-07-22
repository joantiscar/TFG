using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{

    [SerializeField] private TMP_Text text;
    [SerializeField] private string difficulty;
    

    // Start is called before the first frame update
    void Start()
    {
        string leaderBoardString = "";
        List<KeyValuePair<string, int>> leaderBoard;
        if (difficulty == "EASY") leaderBoard = Singleton.GetInstance().leaderBoardEasy;
        else if (difficulty == "NORMAL") leaderBoard = Singleton.GetInstance().leaderBoardNormal;
        else leaderBoard = Singleton.GetInstance().leaderBoardHard;
        foreach (KeyValuePair<string, int> value in leaderBoard)
            leaderBoardString += (value.Key + " " + value.Value + "\n");
        text.text = leaderBoardString;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
