using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    public static MenuUIHandler Instance;

    [SerializeField, Tooltip("Drag here the input text name")]
    TMP_InputField inputName;
    [SerializeField, Tooltip("Drag here the maxSores text")]
    TextMeshProUGUI maxScoresList;

    private string maxScoresString;

    const char separator = '\n';

    private void Awake()
    {
        Instance = this;        ReadDataManager();
        
    }

    public void UpdateDataManager()
    {        
        DataManager.Instance.playerName = inputName.text;
        DataManager.Instance.MaxScores.Clear();
        DataManager.Instance.MaxScores.AddRange(ConvertMaxScoreToList());
    }

    public void ReadDataManager()
    {
        maxScoresList.text = ConvertMaxScoreToString();
    }

    public void PlayArkanoid()
    {
        DataManager.Instance.UpdateData();        
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    private List<string> ConvertMaxScoreToList()
    {
        // Get a string from MaxScoreList string
        maxScoresString = maxScoresList.text;

        // Check if MaxScoreList is null or empty.
        if (!string.IsNullOrEmpty(maxScoresString))
        {
            return maxScoresString.Split(separator).ToList();
        }
        else
        {
            // Return a list form the string
            return new List<string>();
        }
    }

    private string ConvertMaxScoreToString()
    {
        string maxScores;
        if (DataManager.Instance != null)
        {
            if (DataManager.Instance.MaxScores.Count > 0)
            {
                maxScores = string.Join(separator.ToString(), DataManager.Instance.MaxScores);
            }
            else
            {
                maxScores = "";
            }
        }
        else 
        { 
            maxScores = ""; 
        }

        return maxScores;
    }
}
