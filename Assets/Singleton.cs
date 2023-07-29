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
    public TMP_Text TotalScoreLabel;
    public TMP_Text AccLabel;
    public TMP_Text PerfectNotesLabel;
    public TMP_Text GoodNotesLabel;
    public TMP_Text MissedNotesLabel;
    public GameObject LeftRay;
    public GameObject RightRay;
    public GameObject Baqueta1;
    public GameObject Baqueta2;
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
        
    }


    public static Singleton GetInstance()
    {
        return inst;
    }
}