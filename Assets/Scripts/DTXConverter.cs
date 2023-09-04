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
using TMPro;

public class DTXConverter : MonoBehaviour
{
    public string path;
    string[] fileContent;
    float[] BPMs = new float[1297];
    public string[] chips = new string[1297];
    int[] volumes = new int[1297];
    public float currentBPM = 0;
    public GameObject channels;
    public GameObject AudioSpawners;
    public GameObject notaPrefab;
    public GameObject quarterPrefab;
    public GameObject beginingPrefab;
    public GameObject[] chipSpawners;
    
    bool over = false;
    public bool test = false;


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

    public TMP_Text AccLabel;
    public TMP_Text ScoreLabel;
    public TMP_Text ComboLabel;



    public int lastMeasure = 0;
    public int totalNotes = 0;
    public int notesPassed = 0;
    public int perfectNotes = 0;
    public int goodNotes = 0;
    public int missedNotes = 0;
    public int score = 0;
    public int combo = 0;
    public int comboMultiplier = 1;
    public int comboCounter = 0;
    public int comboThreshold = 5;
    public int ogComboThreshold = 5;

    string artist;
    string songName;

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


    public void playChip(int chip)
    {
        var a = chipSpawners[chip];
        a.GetComponent<AudioSource>().Play();
    }


    void createBarForChannelAtHeight(int pos, GameObject channel)
    {
        bool transf = channel.GetComponent<Channel>().instrumentTransform;
        var newObj = Instantiate(beginingPrefab, channel.transform);
        newObj.transform.localPosition = new Vector3(0, (pos * noteSeparationValue), 0);
        if (transf) newObj.transform.rotation = channel.GetComponent<Channel>().instrumentTransform.localRotation;
        newObj = Instantiate(quarterPrefab, channel.transform);
        newObj.transform.localPosition = new Vector3(0, ((pos + (float)0.25) * noteSeparationValue), 0);
        if (transf) newObj.transform.rotation = channel.GetComponent<Channel>().instrumentTransform.localRotation;        
        newObj = Instantiate(quarterPrefab, channel.transform);
        newObj.transform.parent = channel.transform;
        newObj.transform.localPosition = new Vector3(0, ((pos + (float)0.50) * noteSeparationValue), 0);
        if (transf) newObj.transform.rotation = channel.GetComponent<Channel>().instrumentTransform.localRotation;
        newObj = Instantiate(quarterPrefab, channel.transform);
        newObj.transform.localPosition = new Vector3(0, ((pos + (float)0.75) * noteSeparationValue), 0);
        if (transf) newObj.transform.rotation = channel.GetComponent<Channel>().instrumentTransform.localRotation;

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

    Channel GetChannelByNumber(int n)
    {
        Channel c = BGMChannel.GetComponent<Channel>();
        switch (n)
        {
            case 01:
                c = BGMChannel.GetComponent<Channel>();
                break;
            case 08:
                c = BPMChannel.GetComponent<Channel>();
                break;
            case 11:
                c = HiHatCloseChannel.GetComponent<Channel>();
                break;
            case 12:
                c = SnareChannel.GetComponent<Channel>();
                break;
            case 13:
                c = BassDrumChannel.GetComponent<Channel>();
                break;
            case 14:
                c = HighTomChannel.GetComponent<Channel>();
                break;
            case 15:
                c = LowTomChannel.GetComponent<Channel>();
                break;
            case 16:
                c = CymbalChannel.GetComponent<Channel>();
                break;
            case 17:
                c = FloorTomChannel.GetComponent<Channel>();
                break;
            case 18:
                c = HiHatOpenChannel.GetComponent<Channel>();
                break;
            case 19:
                c = RideCymbalChannel.GetComponent<Channel>();
                break;
            case 20:
                c = LeftCymbalChannel.GetComponent<Channel>();
                break;
        }
        return c;
    }

    void generateMap()
    {
        foreach (String s in map.Keys)
        {
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
                        Channel newParent = GetChannelByNumber(i);
                        Color c = newParent.color;
                        if (newParent.GetComponent<Channel>().defaultChip == -1)
                        {
                            newParent.GetComponent<Channel>().defaultChip = base36ToDecimal(notes[m]);
                        }
                        var newObj = Instantiate(newParent.notePrefab, newParent.gameObject.transform);
                        newObj.transform.localPosition = new Vector3(0, ((j + (float)(m * distance)) * noteSeparationValue) + 4, 0);
                        if (newParent.instrumentTransform){
                            var thing = newObj.transform.Find("Model").transform;
                            thing.rotation = newParent.instrumentTransform.rotation;
                            thing.parent = newParent.instrumentTransform.parent;                     // Detach
                            thing.localScale = newParent.instrumentTransform.localScale;
                            thing.SetParent(newObj.transform, true);
                        }                        
                        newObj.GetComponent<Nota>().objectNumber = base36ToDecimal(notes[m]);
                        newObj.GetComponent<Nota>().objectChannel = i;
                        newObj.GetComponent<Nota>().DTXConverter = this;
                        if (i == HiHatOpen)
                        {
                            newObj.GetComponent<Nota>().FootIcon.SetActive(true);
                        }
                        if (i == 1 || i == 8 || i == 3)
                        {
                            newObj.GetComponent<Nota>().visible = false;
                        }
                        else
                        {
                            newObj.name = "Nota " + totalNotes;
                            totalNotes++;
                        }
                        //newObj.transform.Find("Model").GetComponent<Renderer>().material.SetColor("_BaseColor", c);
                        newObj.transform.parent = newParent.notesContainer.transform;

                    }
                }

            }


        }


    }



    void Start()
    {

        chipSpawners = new GameObject[1295];
        parseFile();
        StartCoroutine(LoadThings());

    }


    IEnumerator LoadThings()
    {
        generateMap();

        createSoundSpawners();

        yield return new WaitForSeconds(3);

        if (auto || test) startGame();


    }

    async Task<bool> createSoundSpawners()
    {   
        for (int i = 1; i < 1295; i++) // Per cada possible chip
        {
            if (chips[i].Length > 0) // Si la chip existeix
            {
                if (chips[i].StartsWith("#WAV")) chips[i] = chips[i].Substring(7); // Si no hem borrat el #WAV, el borrem
                AudioClip c = await LoadClip(Path.GetFullPath(Path.Combine(path, @"..\")) + chips[i]);
                chipSpawners[i] = new GameObject();
                chipSpawners[i].transform.parent = AudioSpawners.transform;
                chipSpawners[i].AddComponent<AudioSource>();
                chipSpawners[i].name = chips[i] + "(" + i + ")";
                chipSpawners[i].GetComponent<AudioSource>().clip = c;
                if (volumes[i] != 0) // Si hi ha indicat el volum per aquest chipA
                {
                    chipSpawners[i].GetComponent<AudioSource>().volume = (float)volumes[i] / 100;
                }

            }
        }
        return true;
    }

    async Task<AudioClip> LoadClip(string clipPath)
    {
        AudioClip clip = null; AudioType x;
        if (clipPath.ToUpper().EndsWith("WAV")) x = AudioType.WAV;
        else if (clipPath.ToUpper().EndsWith("MP3")) x = AudioType.MPEG;
        else if (clipPath.ToUpper().EndsWith("OGG")) x = AudioType.OGGVORBIS;
        else if (clipPath.ToUpper().EndsWith("XA"))
        {
            // Si el fitxer es .XA, intentem buscar una alternativa .WAV
            clipPath = clipPath.Replace(".xa", ".WAV");
            clipPath = clipPath.Replace(".XA", ".WAV");
            clipPath = clipPath.ToUpper();
            x = AudioType.WAV;
        }
        else x = AudioType.UNKNOWN;
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(clipPath, x))
        {
            uwr.SendWebRequest();
            // wrap tasks in try/catch, otherwise it'll fail silently
            try
            {
                while (!uwr.isDone) await Task.Delay(5);
                if (uwr.isNetworkError || uwr.isHttpError) Debug.Log($"{uwr.error}" + " File name: " + clipPath);
                else clip = DownloadHandlerAudioClip.GetContent(uwr);    
            }
            catch (Exception err){ Debug.Log($"{err.Message}, {err.StackTrace} File name: {clipPath}");}
        }
        return clip;
    }

    public void startGame()
    {
        currentBPM = BPMs[0];
        if (currentBPM == 0) currentBPM = BPMs[1];
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
        ComboLabel.text = "x" + comboMultiplier.ToString();
        ScoreLabel.text = score.ToString();
        if (notesPassed > 0) AccLabel.text = ((float) (((perfectNotes + (goodNotes / 2)) / notesPassed) * 100)).ToString() + "%";
        else AccLabel.text = "100%";

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
        comboCounter += 1;
        combo += 1;
        if (comboCounter >= comboThreshold){
            comboMultiplier *= 2;
            comboCounter = 0;
            comboThreshold *= 2;
        }
        score += ammount * comboMultiplier;
    }

    public void PerfectNote()
    {
        IncreaseScore(300);
        notesPassed++;
        perfectNotes++;
        checkForGameEnd();
    }

    public void GoodNote()
    {
        IncreaseScore(150);
        notesPassed++;
        goodNotes++;
        checkForGameEnd();
    }

    public void MissedNote()
    {
        notesPassed++;
        missedNotes++;
        combo = 0;
        comboCounter = 0;
        comboMultiplier = 1;
        comboThreshold = ogComboThreshold;
        checkForGameEnd();
    }

    void checkForGameEnd()
    {
        if (notesPassed >= totalNotes && !over)
        {
            StartCoroutine(FinishGame());
        }
    }

    IEnumerator FinishGame()
    {
        over = true;
        Singleton.GetInstance().lastScore = score;
        Singleton.GetInstance().lastTotalNotes = totalNotes;
        Singleton.GetInstance().lastPerfectNotes = perfectNotes;
        Singleton.GetInstance().lastGoodNotes = goodNotes;
        Singleton.GetInstance().lastMissedNotes = missedNotes;
        Singleton.GetInstance().songTitle = artist + " - " + songName;
        Singleton.GetInstance().CloseGameAndShowScore();
        yield return new WaitForSeconds(2);
    }

    void parseFile()
    {
        if (path.Length != 0)
        {
            fileContent = File.ReadAllLines(path);
            foreach (string line in fileContent)
            {
                string lineClean = line;
                // Si conté un comentari, l'eliminem
                int index = lineClean.LastIndexOf(";");
                if (index >= 0) lineClean = lineClean.Substring(0, index);
                if (lineClean.Length == 0) continue;
                // Si no comença amb un #, es una linea incorrecta
                if (lineClean[0] == '#')
                {  
                    // Si comença amb 5 digits, es un objecte
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
                    else if (lineClean.StartsWith("#TITLE")) songName = lineClean.Substring(lineClean.LastIndexOf(':') + 1).TrimEnd().TrimStart();
                    else if (lineClean.StartsWith("#ARTIST")) artist = lineClean.Substring(lineClean.LastIndexOf(':') + 1).TrimEnd().TrimStart();      
                    else if (lineClean.StartsWith("#BPM"))
                    {
                        // Ignorar format antic 
                        if (!lineClean.Contains(':')) continue;
                        int pos;
                        // Si no especifica un numero despres de BPM és el primer
                        if (lineClean[4] == ':') pos = 0;
                        else pos = base36ToDecimal(lineClean.Substring(4, 2));
                        string t = lineClean.Substring(lineClean.LastIndexOf(':') + 1).TrimEnd().TrimStart();
                        if (t.Contains('.')) BPMs[pos] = float.Parse(t, CultureInfo.InvariantCulture);
                        else BPMs[pos] = (float)int.Parse(t, CultureInfo.InvariantCulture);
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
                }
            }
        }
    }

}