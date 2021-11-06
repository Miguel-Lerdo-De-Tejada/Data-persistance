using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class DataManager: MonoBehaviour
{
    public static DataManager Instance;    

    [HideInInspector]
    public string playerName;
    [HideInInspector]
    public List<string> MaxScores;

    // Data base table ArkanoidGame:
    [System.Serializable]
    private class ArkanoidGame
    {
        public string playerName;
        public string maxScores;
    }
    
    private void Awake()
    {
        MaxScores = new List<string>();
        UploadDataManager();        
    }

    private void Start()
    {
        ReadData();
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

    public void SaveData()
    {
        ArkanoidGame arkanoid = new ArkanoidGame();

        arkanoid.playerName = MenuUIHandler.Instance.inputName.text;
        arkanoid.maxScores = MenuUIHandler.Instance.ConvertMaxScoreToString();

        string jsonFile = JsonUtility.ToJson(arkanoid);

        File.WriteAllText($"{Application.persistentDataPath}/ArkanoidFile.json", jsonFile);
    }

    // Read Arkanoid data
    public void ReadData()
    {
        string path= $"{Application.persistentDataPath}/ArkanoidFile.json";
        
        if (File.Exists(path))
        {
            string jsonFile = File.ReadAllText(path);

            ArkanoidGame arkanoid = JsonUtility.FromJson<ArkanoidGame>(jsonFile);

            MenuUIHandler.Instance.inputName.text = arkanoid.playerName;
            MenuUIHandler.Instance.inputErrorName.text = arkanoid.playerName;
            MenuUIHandler.Instance.maxScoresList.text = arkanoid.maxScores;
        }
    }

    // Update Arkanoid data
    public void UpdateData()
    {
        MenuUIHandler.Instance.UpdateDataManager();
    }

    // Se envía a todos los objetos de juego antes de que se cierre la aplicación
    private void OnApplicationQuit()
    {
        SaveData();        
    }
}
