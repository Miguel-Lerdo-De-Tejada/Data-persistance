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
    
    [SerializeField,Tooltip("Drag the player name input UI field here")]
    private TMPro.TMP_InputField playerNameField;

    private void Awake()
    {
        ReadData();
    }

    private void Update()
    {
        readPlayerName();
    }

    private void ReadData()
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

    private void readPlayerName()
    {
        playerName = playerNameField.text;
    }
}
