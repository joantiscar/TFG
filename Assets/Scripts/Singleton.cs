using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using SimpleFileBrowser;

public class Singleton : MonoBehaviour
{

    private static Singleton inst;

    public string path;
    public GameObject UI;
    public GameObject UIMainMenu;
    public GameObject UIPickSong;
    public GameObject UISettings;
    public GameObject UIScoreBoard;
    public GameObject MapPrefab;
    public DTXConverter converter;
    public int lastScore = 0;
    public int lastTotalNotes = 0;
    public int lastPerfectNotes = 0;
    public int lastGoodNotes = 0;
    public int lastMissedNotes = 0;
    public string songTitle = "";
    public TMP_Text TotalScoreLabel;
    public TMP_Text AccLabel;
    public TMP_Text PerfectNotesLabel;
    public TMP_Text GoodNotesLabel;
    public TMP_Text MissedNotesLabel;
    public TMP_Text SongTitleLabel;
    public GameObject LeftRay;
    public GameObject LeftHand;
    public GameObject RightRay;
    public GameObject RightHand;
    public GameObject BaquetaPrefab;
    public GameObject Baqueta1;
    public Transform Baqueta1Pos;
    public bool Baqueta1Grabbed;
    public GameObject Baqueta2;
    public Transform Baqueta2Pos;
    public bool Baqueta2Grabbed;
    private void Awake()
    {
        if(inst == null)
        {
            inst = this;

            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        FileBrowser.SetFilters(true, new FileBrowser.Filter("DTX", ".dtx", ".DTX"));
        FileBrowser.SetDefaultFilter(".dtx");
        
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickSong(){
        UIMainMenu.active = false;
        UIPickSong.active = true;
        StartCoroutine(ShowLoadDialogCoroutine());
    }

    public void CloseGameAndShowScore(){
        Destroy(converter.gameObject);
        UIScoreBoard.SetActive(true);
        TotalScoreLabel.text = lastScore.ToString();
        AccLabel.text = ((lastScore / (lastTotalNotes * 300)) * 100).ToString() + "%";
        PerfectNotesLabel.text = lastPerfectNotes.ToString();
        GoodNotesLabel.text = lastGoodNotes.ToString();
        MissedNotesLabel.text = lastMissedNotes.ToString();
        SongTitleLabel.text = songTitle;
        Destroy(Baqueta1);
        Destroy(Baqueta2);
        LeftHand.GetComponent<UnityEngine.XR.Interaction.Toolkit.ActionBasedController>().enableInputActions = true;
        RightHand.GetComponent<UnityEngine.XR.Interaction.Toolkit.ActionBasedController>().enableInputActions = true;
        
    }

    public void BackToMenu(){
        UIScoreBoard.SetActive(false);
        UIMainMenu.SetActive(true);
    }

    public IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        if (FileBrowser.Success)
        {
            path = FileBrowser.Result[0];
            Baqueta1 = Instantiate(BaquetaPrefab, Baqueta1Pos);
            Baqueta1.name = "Baqueta1";
            Baqueta2 = Instantiate(BaquetaPrefab, Baqueta2Pos);
            Baqueta2.name = "Baqueta2";
            StartGame();
        }
        
    }
    
    public void StartGame(){
        var newObj = Instantiate(MapPrefab, transform);
        converter = newObj.transform.Find("Map").GetComponent<DTXConverter>();
        converter.path = path;
        lastScore = 0;
        lastPerfectNotes = 0;
        lastGoodNotes = 0;
        lastMissedNotes = 0;
        LeftRay.SetActive(false);
        RightRay.SetActive(false);
        Baqueta1.SetActive(true);
        Baqueta2.SetActive(true);
    }

    public void checkStart(){
        if (Baqueta1Grabbed && Baqueta2Grabbed){
            converter.startGame();
        }
    }

    public static Singleton GetInstance()
    {
        return inst;
    }
}