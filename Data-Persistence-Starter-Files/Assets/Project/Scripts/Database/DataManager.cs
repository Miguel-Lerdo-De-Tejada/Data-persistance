using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DataManager: MonoBehaviour
{
    public static DataManager Instance;    

    [HideInInspector]
    public string playerName;
    [HideInInspector]
    public List<string> MaxScores;
    
    private void Awake()
    {
        MaxScores = new List<string>();
        UploadDataManager();        
    }

    protected void UploadDataManager()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }            
    }

    // Read Arkanoid data
    public void ReadData()
    {

    }

    // Update Arkanoid data
    public void UpdateData()
    {
        MenuUIHandler.Instance.UpdateDataManager();
    }
}
