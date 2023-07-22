using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Singleton : MonoBehaviour
{

    private static Singleton inst;

    public List<KeyValuePair<string, int>> leaderBoardEasy;
    public List<KeyValuePair<string, int>> leaderBoardNormal;
    public List<KeyValuePair<string, int>> leaderBoardHard;

    public int difficulty;
    public string currentName;
    private int currentScore;


    private void Awake()
    {
        if(inst == null)
        {
            inst = this;
            inst.leaderBoardEasy = new List<KeyValuePair<string, int>>(5);
            inst.leaderBoardNormal = new List<KeyValuePair<string, int>>(5);
            inst.leaderBoardHard = new List<KeyValuePair<string, int>>(5);
            DontDestroyOnLoad(gameObject);

            /*
            // Dades de prova
            AddLeaderBoardEntry("Easy", 99, 0);
            AddLeaderBoardEntry("Easy", 5, 0);
            AddLeaderBoardEntry("asdsadas", 23, 0);
            AddLeaderBoardEntry("Easy", 2, 0);
            AddLeaderBoardEntry("Normal", 9999, 1);
            AddLeaderBoardEntry("asdsadsa", 9999, 1);
            AddLeaderBoardEntry("Normal", 9999, 1);
            AddLeaderBoardEntry("Normal", 9999, 1);
            AddLeaderBoardEntry("Hard", 12345565, 2);
            AddLeaderBoardEntry("Hard", 123, 2);
            AddLeaderBoardEntry("dasdsadsadas", 2, 2);
            AddLeaderBoardEntry("Hard", 444, 2);
            */
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        difficulty = Game.EASY_MODE;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static Singleton GetInstance()
    {
        return inst;
    }


    public int GetDifficulty()
    {
        return difficulty;
    }

    public void SetDifficult(TMP_Text diff)
    {
        SetDifficult(diff.text);
    }

    public void SetDifficult(string diff)
    {
        string text = diff.ToUpper();
        if (text.Equals("EASY")) difficulty = Game.EASY_MODE;
        else if (text.Equals("NORMAL")) difficulty = Game.NORMAL_MODE;
        else if (text.Equals("HARD")) difficulty = Game.HARD_MODE;
    }

    public string GetCurrentName()
    {
        return currentName;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }


    private class LeaderBoardComparator : IComparer<KeyValuePair<string, int>>
    {
        public int Compare(KeyValuePair<string, int> x, KeyValuePair<string, int> y)
        {
            return (y.Value - x.Value);
        }
    }


    public void AddLeaderBoardEntry(string name, int score, int difficulty)
    {
        List<KeyValuePair<string, int>> leaderBoard;
        if (difficulty == 0) leaderBoard = leaderBoardEasy;
        else if (difficulty == 1) leaderBoard = leaderBoardNormal;
        else leaderBoard = leaderBoardHard;
        currentName = name;
        currentScore = score;
        KeyValuePair<string, int> newEntry = new KeyValuePair<string, int>(name, score);
        leaderBoard.Add(newEntry);
        leaderBoard.Sort(new LeaderBoardComparator());
        if (leaderBoard.Count > 5)
            leaderBoard.RemoveAt(leaderBoard.Count - 1);
    }
}
