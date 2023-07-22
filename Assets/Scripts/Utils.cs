using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Utils : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void WriteInName(string letter){
        Singleton.GetInstance().currentName += letter;
    }
    public void DeleteInName(){
        if (Singleton.GetInstance().currentName.Length > 0) 
            Singleton.GetInstance().currentName = Singleton.GetInstance().currentName.Remove(Singleton.GetInstance().currentName.Length - 1);
    }
}
