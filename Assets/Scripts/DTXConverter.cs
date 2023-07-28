using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using System.Text.RegularExpressions;
using System.Globalization;
using System;
using System.Threading.Tasks;
using SimpleFileBrowser;

public class DTXConverter : MonoBehaviour
{
    public string path;
    string[] fileContent;
    public float[] BPMs = new float[1295];
    public string[] chips = new string[1295];
    public int[] volumes = new int[1295];
    public float currentBPM = 0;
    public GameObject channels;
    public GameObject AudioSpawners;
    public GameObject notaPrefab;
    public GameObject quarterPrefab;
    public GameObject beginingPrefab;
    public GameObject[] chipSpawners;
    public int score = 0;

    public GameObject BGMChannel;
    public GameObject BPMChannel;
    public GameObject HiHatCloseChannel;
    public GameObject SnareChannel;
    public GameObject BassDrumChannel;
    public GameObject HighTomChannel;
    public GameObject LowTomChannel;
    public GameObject CymbalChannel;
    public GameObject FloorTomChannel;
    public GameObject HiHatOpenChannel;
    public GameObject RideCymbalChannel;
    public GameObject LeftCymbalChannel;

    public int eBGMChannel = 1;
    public int eBPMChannel = 8;
    public int HiHatClose = 11;
    public int Snare = 12;
    public int BassDrum = 13;
    public int HighTom = 14;
    public int LowTom = 15;
    public int Cymbal = 16;
    public int FloorTom = 17;
    public int HiHatOpen = 18;
    public int RideCymbal = 19;
    public int LeftCymbal = 0x1A;
    public int CompasChannel = 02;

    public int noteSeparationValue = 12;

    double timeSignature = 1;



    int lastMeasure = 0;
    public bool auto = false;

    SortedDictionary<string, List<string>> map = new SortedDictionary<string, List<string>>();
    private const string CharList = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    int base36ToDecimal(string input)
    {
        input = input.ToUpper();
        int result = 0;
        int pos = 0;
        for (int i = input.Length - 1; i >= 0; i--)
        {
            result += CharList.IndexOf(input[i]) * (int)Math.Pow(36, pos);
            pos++;
        }
        return result;

    }

    public void changeBPM(int a)
    {
        currentBPM = BPMs[a];
    }

    void parseFile()
    {
        if (path.Length != 0)
        {
            fileContent = File.ReadAllLines(path);
            foreach (string line in fileContent)
            {
                string lineClean = line;
                int index = lineClean.LastIndexOf(";");
                if (index >= 0) lineClean = lineClean.Substring(0, index);
                if (lineClean.Length == 0) continue;
                if (lineClean[0] == '#')
                {
                    if (Regex.IsMatch(lineClean, "^#\\d{5}"))
                    {

                        string key = lineClean.Substring(1, 5);
                        string objects = lineClean.Substring(lineClean.LastIndexOf(':') + 1).TrimEnd().TrimStart();
                        if (!map.ContainsKey(key))
                        {
                            map.Add(key, new List<string>());
                        }
                        map[key].Add(objects);
                        if (int.Parse(key.Substring(0, 3)) > lastMeasure) lastMeasure = int.Parse(key.Substring(0, 3));

                    }
                    else if (lineClean.StartsWith("#TITLE:"))
                    {

                    }
                    else if (lineClean.StartsWith("#ARTIST"))
                    {

                    }
                    else if (lineClean.StartsWith("#PREVIEW"))
                    {

                    }
                    else if (lineClean.StartsWith("#PREIMAGE"))
                    {

                    }
                    else if (lineClean.StartsWith("#BPM"))
                    {

                        if (!lineClean.Contains(':')) continue;
                        int pos;
                        if (lineClean[4] == ':')
                        {
                            // BPM
                            pos = 0;
                        }
                        else
                        {
                            // BPMzz
                            pos = base36ToDecimal(lineClean.Substring(4, 2));
                        }
                        string t = lineClean.Substring(lineClean.LastIndexOf(':') + 1).TrimEnd().TrimStart();

                        if (t.Contains('.'))
                        {
                            BPMs[pos] = float.Parse(t, CultureInfo.InvariantCulture);
                        }
                        else
                        {

                            BPMs[pos] = (float)int.Parse(t, CultureInfo.InvariantCulture);
                        }

                    }
                    else if (lineClean.StartsWith("#WAV"))
                    {
                        int zz = base36ToDecimal(lineClean.Substring(4, 2));
                        string t = lineClean.Substring(lineClean.LastIndexOf(':') + 1).TrimEnd().TrimStart();
                        chips[zz] = t;
                    }
                    else if (lineClean.StartsWith("#VOLUME"))
                    {
                        int zz = base36ToDecimal(lineClean.Substring(7, 2));
                        string t = lineClean.Substring(lineClean.LastIndexOf(':') + 1).TrimEnd().TrimStart();
                        volumes[zz] = int.Parse(t);
                    }
                    else if (lineClean.StartsWith("#COMMENT"))
                    {

                    }
                    else if (lineClean.StartsWith("#PANEL"))
                    {

                    }
                    else if (lineClean.StartsWith("#DLEVEL"))
                    {

                    }
                    else if (lineClean.StartsWith("#GLEVEL"))
                    {

                    }
                    else if (lineClean.StartsWith("#BLEVEL"))
                    {

                    }
                    else if (lineClean.StartsWith("#HIDDENLEVEL"))
                    {

                    }
                    else if (lineClean.StartsWith("#STAGEFILE"))
                    {

                    }
                    else if (lineClean.StartsWith("#PREMOVIE"))
                    {

                    }
                    else if (lineClean.StartsWith("#BACKGROUND"))
                    {

                    }
                    else if (lineClean.StartsWith("#BACKGROUND_GR"))
                    {

                    }
                    else if (lineClean.StartsWith("#ARTIST"))
                    {

                    }
                    else if (lineClean.StartsWith("#ARTIST"))
                    {

                    }
                    else if (lineClean.StartsWith("#ARTIST"))
                    {

                    }
                    else if (lineClean.StartsWith("#ARTIST"))
                    {

                    }
                    else if (lineClean.StartsWith("#ARTIST"))
                    {

                    }
                    else if (lineClean.StartsWith("#ARTIST"))
                    {

                    }
                }
            }
        }
    }

    public void playChip(int chip)
    {
        var a = chipSpawners[chip];
        a.GetComponent<AudioSource>().Play();
    }


    void createBarForChannelAtHeight(int pos, GameObject channel)
    {
        var newObj = Instantiate(beginingPrefab, new Vector3(channel.transform.position.x, (pos * noteSeparationValue), channel.transform.position.z), Quaternion.Euler(0, 0, 0));
        newObj.transform.parent = channel.transform;
        newObj = Instantiate(quarterPrefab, new Vector3(channel.transform.position.x, ((pos + (float)0.25) * noteSeparationValue), channel.transform.position.z), Quaternion.Euler(0, 0, 0));
        newObj.transform.parent = channel.transform;
        newObj = Instantiate(quarterPrefab, new Vector3(channel.transform.position.x, ((pos + (float)0.50) * noteSeparationValue), channel.transform.position.z), Quaternion.Euler(0, 0, 0));
        newObj.transform.parent = channel.transform;
        newObj = Instantiate(quarterPrefab, new Vector3(channel.transform.position.x, ((pos + (float)0.75) * noteSeparationValue), channel.transform.position.z), Quaternion.Euler(0, 0, 0));
        newObj.transform.parent = channel.transform;
    }


    void generateBars()
    {
        var newObj = channels;
        for (int p = 0; p < lastMeasure + 2; p++)
        {
            createBarForChannelAtHeight(p, HiHatCloseChannel);
            createBarForChannelAtHeight(p, SnareChannel);
            createBarForChannelAtHeight(p, BassDrumChannel);
            createBarForChannelAtHeight(p, HighTomChannel);
            createBarForChannelAtHeight(p, LowTomChannel);
            createBarForChannelAtHeight(p, CymbalChannel);
            createBarForChannelAtHeight(p, FloorTomChannel);
            createBarForChannelAtHeight(p, RideCymbalChannel);
            createBarForChannelAtHeight(p, LeftCymbalChannel);
        }
    }

    void generateMap()
    {
        int cont = 0;
        generateBars();
        foreach (String s in map.Keys)
        {
            // ESTA MAL ANAR DE 0 A 3599 I FER PRIMER LA TIME SIGNATURE I DESPUES LA RESTA EN FUNCIO DEL NOU ESPAI
            int i = int.Parse(s.Substring(3));
            int j = int.Parse(s.Substring(0, 3));
            List<string> objectsArray;
            map.TryGetValue(s, out objectsArray);
            foreach (string objects in objectsArray)
            {
                if (i == 2)
                {
                    timeSignature = double.Parse(objects, CultureInfo.InvariantCulture);
                    continue;
                }
                List<string> notes = GetChunks(objects, 2);
                double distance = (double)1 / notes.Count;
                for (int m = 0; m < notes.Count; m++)
                {
                    if (!notes[m].Equals("00"))
                    {
                        Color c = Color.black;
                        GameObject newParent = this.gameObject;
                        switch (i)
                        {

                            case 01:
                                newParent = BGMChannel;
                                break;
                            case 08:
                                newParent = BPMChannel;
                                break;
                            case 11:
                                if (HiHatCloseChannel.GetComponent<Channel>().defaultChip == -1)
                                {
                                    HiHatCloseChannel.GetComponent<Channel>().defaultChip = base36ToDecimal(notes[m]);
                                }
                                newParent = HiHatCloseChannel;
                                c = Color.blue;
                                break;
                            case 12:
                                if (SnareChannel.GetComponent<Channel>().defaultChip == -1)
                                {
                                    SnareChannel.GetComponent<Channel>().defaultChip = base36ToDecimal(notes[m]);
                                }
                                newParent = SnareChannel;
                                c = Color.yellow;
                                break;
                            case 13:
                                if (BassDrumChannel.GetComponent<Channel>().defaultChip == -1)
                                {
                                    BassDrumChannel.GetComponent<Channel>().defaultChip = base36ToDecimal(notes[m]);
                                }
                                newParent = BassDrumChannel;
                                c = new Color((float)180 / 255, (float)180 / 255, (float)180 / 255);

                                break;
                            case 14:
                                if (HighTomChannel.GetComponent<Channel>().defaultChip == -1)
                                {
                                    HighTomChannel.GetComponent<Channel>().defaultChip = base36ToDecimal(notes[m]);
                                }
                                newParent = HighTomChannel;
                                c = Color.green;
                                break;
                            case 15:
                                if (LowTomChannel.GetComponent<Channel>().defaultChip == -1)
                                {
                                    LowTomChannel.GetComponent<Channel>().defaultChip = base36ToDecimal(notes[m]);
                                }
                                newParent = LowTomChannel;
                                c = Color.red;
                                break;
                            case 16:
                                if (CymbalChannel.GetComponent<Channel>().defaultChip == -1)
                                {
                                    CymbalChannel.GetComponent<Channel>().defaultChip = base36ToDecimal(notes[m]);
                                }
                                newParent = CymbalChannel;
                                c = Color.blue;
                                break;
                            case 17:
                                if (FloorTomChannel.GetComponent<Channel>().defaultChip == -1)
                                {
                                    FloorTomChannel.GetComponent<Channel>().defaultChip = base36ToDecimal(notes[m]);
                                }
                                newParent = FloorTomChannel;
                                c = new Color((float)178 / 255, (float)133 / 235, (float)67 / 255);
                                break;
                            case 18:
                                if (HiHatOpenChannel.GetComponent<Channel>().defaultChip == -1)
                                {
                                    HiHatOpenChannel.GetComponent<Channel>().defaultChip = base36ToDecimal(notes[m]);
                                }
                                newParent = HiHatOpenChannel;
                                c = Color.blue;
                                break;
                            case 19:
                                if (RideCymbalChannel.GetComponent<Channel>().defaultChip == -1)
                                {
                                    RideCymbalChannel.GetComponent<Channel>().defaultChip = base36ToDecimal(notes[m]);
                                }
                                newParent = RideCymbalChannel;
                                c = Color.black;
                                break;
                            case 20:
                                if (LeftCymbalChannel.GetComponent<Channel>().defaultChip == -1)
                                {
                                    LeftCymbalChannel.GetComponent<Channel>().defaultChip = base36ToDecimal(notes[m]);
                                }
                                newParent = LeftCymbalChannel;
                                c = Color.magenta;
                                break;
                        }
                        if (newParent != this.gameObject)
                        {
                            var newObj = Instantiate(notaPrefab, 
                            new Vector3(newParent.transform.position.x, ((j + (float)(m * distance)) * noteSeparationValue) + 4, newParent.transform.position.z), Quaternion.Euler(0, 0, 90), 
                            this.transform);
                            newObj.GetComponent<Nota>().objectNumber = base36ToDecimal(notes[m]);
                            newObj.GetComponent<Nota>().objectChannel = i;
                            newObj.GetComponent<Nota>().DTXConverter = this;
                            if (i == HiHatOpen)
                            {
                                newObj.GetComponent<Nota>().FootIcon.SetActive(true);
                            }
                            if (i == 1 || i == 8) newObj.GetComponent<MeshRenderer>().enabled = false;
                            newObj.GetComponent<Nota>().GetComponent<Renderer>().material.SetColor("_BaseColor", c);
                            newObj.name = "Nota " + cont;
                            cont++;
                            newObj.transform.parent = newParent.GetComponent<Channel>().notesContainer.transform;

                        }

                    }
                }

            }


        }


    }

    IEnumerator ShowLoadDialogCoroutine()
    {
        // Show a load file dialog and wait for a response from user
        // Load file/folder: both, Allow multiple selection: true
        // Initial path: default (Documents), Initial filename: empty
        // Title: "Load File", Submit button text: "Load"
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Load");

        // Dialog is closed
        // Print whether the user has selected some files/folders or cancelled the operation (FileBrowser.Success)
        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            // Print paths of the selected files (FileBrowser.Result) (null, if FileBrowser.Success is false)
            for (int i = 0; i < FileBrowser.Result.Length; i++)
                Debug.Log(i + ": " + FileBrowser.Result[i]);
            path = FileBrowser.Result[0];

        }
        parseFile();
        StartCoroutine(LoadThings());
    }

    void Start()
    {

        chipSpawners = new GameObject[1295];

        
        FileBrowser.SetFilters(true, new FileBrowser.Filter("DTX", ".dtx", ".DTX"));
        FileBrowser.SetDefaultFilter(".dtx");
        
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);
        //StartCoroutine(ShowLoadDialogCoroutine());
        

        path = EditorUtility.OpenFilePanel("Select a map", "", "dtx");
        parseFile();
        StartCoroutine(LoadThings());

    }


    IEnumerator LoadThings()
    {


        generateMap();

        createSoundSpawners();

        yield return new WaitForSeconds(1);

        startGame();


    }

    async Task<bool> createSoundSpawners()
    {
        for (int i = 1; i < 1295; i++)
        {
            if (chips[i].Length > 0)
            {
                AudioClip c = await LoadClip(Path.GetFullPath(Path.Combine(path, @"..\")) + chips[i]);
                chipSpawners[i] = new GameObject();
                chipSpawners[i].transform.parent = AudioSpawners.transform;
                chipSpawners[i].AddComponent<AudioSource>();
                chipSpawners[i].name = chips[i] + "(" + i + ")";
                chipSpawners[i].GetComponent<AudioSource>().clip = c;
                if (volumes[i] != 0)
                {
                    chipSpawners[i].GetComponent<AudioSource>().volume = (float)volumes[i] / 100;
                }

            }
        }
        return true;
    }

    async Task<AudioClip> LoadClip(string path)
    {
        AudioClip clip = null;
        AudioType x;
        if (path.ToUpper().EndsWith("WAV")) x = AudioType.WAV;
        else if (path.ToUpper().EndsWith("MP3")) x = AudioType.MPEG;
        else if (path.ToUpper().EndsWith("OGG")) x = AudioType.OGGVORBIS;
        else if (path.ToUpper().EndsWith("XA"))
        {
            path = path.Replace(".xa", ".WAV");
            path = path.Replace(".XA", ".WAV");
            x = AudioType.WAV;
        }
        else x = AudioType.UNKNOWN;
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, x))
        {
            uwr.SendWebRequest();

            // wrap tasks in try/catch, otherwise it'll fail silently
            try
            {
                while (!uwr.isDone) await Task.Delay(5);

                if (uwr.isNetworkError || uwr.isHttpError) Debug.Log($"{uwr.error}");
                else
                {
                    clip = DownloadHandlerAudioClip.GetContent(uwr);
                }
            }
            catch (Exception err)
            {
                Debug.Log($"{err.Message}, {err.StackTrace} File name: {path}");
            }
        }
        // CREATE CHIP
        return clip;
    }

    void startGame()
    {
        currentBPM = BPMs[0];
        BGMChannel.GetComponent<Channel>().moving = true;
        BPMChannel.GetComponent<Channel>().moving = true;
        HiHatCloseChannel.GetComponent<Channel>().moving = true;
        SnareChannel.GetComponent<Channel>().moving = true;
        BassDrumChannel.GetComponent<Channel>().moving = true;
        HighTomChannel.GetComponent<Channel>().moving = true;
        LowTomChannel.GetComponent<Channel>().moving = true;
        CymbalChannel.GetComponent<Channel>().moving = true;
        FloorTomChannel.GetComponent<Channel>().moving = true;
        HiHatOpenChannel.GetComponent<Channel>().moving = true;
        RideCymbalChannel.GetComponent<Channel>().moving = true;
        LeftCymbalChannel.GetComponent<Channel>().moving = true;
    }

    // Update is called once per frame
    void Update()
    {
        BGMChannel.GetComponent<Channel>().BPM = currentBPM;
        BPMChannel.GetComponent<Channel>().BPM = currentBPM;
        HiHatCloseChannel.GetComponent<Channel>().BPM = currentBPM;
        SnareChannel.GetComponent<Channel>().BPM = currentBPM;
        BassDrumChannel.GetComponent<Channel>().BPM = currentBPM;
        HighTomChannel.GetComponent<Channel>().BPM = currentBPM;
        LowTomChannel.GetComponent<Channel>().BPM = currentBPM;
        CymbalChannel.GetComponent<Channel>().BPM = currentBPM;
        FloorTomChannel.GetComponent<Channel>().BPM = currentBPM;
        HiHatOpenChannel.GetComponent<Channel>().BPM = currentBPM;
        RideCymbalChannel.GetComponent<Channel>().BPM = currentBPM;
        LeftCymbalChannel.GetComponent<Channel>().BPM = currentBPM;
    }

    public static List<string> GetChunks(string value, int chunkSize)
    {
        List<string> triplets = new List<string>();
        while (value.Length > chunkSize)
        {
            triplets.Add(value.Substring(0, chunkSize));
            value = value.Substring(chunkSize);
        }
        if (value != "")
            triplets.Add(value);
        return triplets;
    }

    public void channelPressed(int channel)
    {

    }

    public void IncreaseScore(int ammount)
    {
        score += ammount;
    }
}