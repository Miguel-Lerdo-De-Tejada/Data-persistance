using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody BallRB;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI BestScoreText;
    public GameObject GameOverPanel;

    private bool m_Started = false;
    private int m_Points;
    
    // private bool m_GameOver = false;

    private string m_PlayerName;
    private string m_BestPlayerName;
    private int m_BestScore;

    private const float c_ballAccelerationIncrement=0.1f;
    private const float c_ballMaxVelocityIncrement = 1.0f;

    private struct DBLabels
    {
        public static string bestScore = "Best score :";
        public static string Score = "Score :";
    }

    private struct ButtonCode
    {
        public static string fire1 = "Fire1";
    }

    
    // Start is called before the first frame update
    void Start()
    {
        BuildBrickWall();
        ReadDB();
        InitializeScore();
    }

    private void Update()
    {
        CheckGameStates();
    }

    // In start MainManager event build a brick wall

    void BuildBrickWall()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    void InitializeScore()
    {
        ScoreText.text = $"{DBLabels.Score} {m_PlayerName} : {m_Points}";
    }

    // In Update MainManager event check game states, if it do not have started and if it have been finished:

    void CheckGameStates()
    {
        if (!m_Started) {InputStartGame();}

        if (Ball.Instance != null)
        {
            if (Ball.Instance.isOutOfBricks)
            {
                Ball.Instance.isOutOfBricks = false;
                Ball.Instance.acceleration += c_ballAccelerationIncrement;
                Ball.Instance.maxVelocity += c_ballMaxVelocityIncrement;
                                
                BuildBrickWall();                
            }
        }
    }

    // If the game is not started Input for start gema, and if the game is over, input for restart game.

    void InputStartGame()
    {
        if (Input.GetButtonDown(ButtonCode.fire1) || Input.GetKeyDown(KeyCode.Space))
        {
            Ball.Instance.sound.PlayOneShot(Ball.Instance.shoot);
            m_Started = true;
            float randomDirection = Random.Range(-1.0f, 1.0f);
            Vector3 forceDir = new Vector3(randomDirection, 1, 0);
            forceDir.Normalize();

            BallRB.transform.SetParent(null);
            BallRB.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);        
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Add points to score when the brick is bounced by the ball and destroyed, it is called in BuildBrickWall for the 
    // Brick,OnDestroy event.
    void AddPoint(int point)
    {
        m_Points += point;
        if (m_Points > m_BestScore) 
        { 
            m_BestScore = m_Points;
            m_BestPlayerName = m_PlayerName;
        }
        BestScoreText.text = $"{DBLabels.bestScore} {m_BestPlayerName} : {m_BestScore}";
        ScoreText.text = $"{DBLabels.Score} {m_PlayerName} : {m_Points}";        
    }

    // Sets the game over flag and shows the game over user interface screen. It is called in DeathZone class.
    public void GameOver()
    {        
        GameOverPanel.SetActive(true);

        // Update best scores list        
        UpdateBestScoresList();
    }

    private void ReadDB()
    {
        if (DataManager.Instance != null)
        {
            string bestScore="";

            m_PlayerName = DataManager.Instance.playerName;
            if (DataManager.Instance.MaxScores.Count > 0)
            {
                m_BestScore = ObtainBestScore();
                m_BestPlayerName = ObtainBestPlayer();
                bestScore = DataManager.Instance.MaxScores[0];
            }
            else
            {               
                bestScore = $"{m_PlayerName} : {m_BestScore}";
            }

            BestScoreText.text = $"{DBLabels.bestScore} {bestScore}";
        }            
    }

    void UpdateBestScoresList()
    {
        if (DataManager.Instance != null)
        {
            if (DataManager.Instance.MaxScores.Count > 0)
            {
                bool isBestScore = false;
                List<string> score = new List<string>();
                for (int i = 0; i < DataManager.Instance.MaxScores.Count; i++)
                {
                    if (i > 5)
                    {
                        break;
                    }
                    else
                    {
                        string scoreData = DataManager.Instance.MaxScores[i];
                        score.Clear();
                        score.AddRange(scoreData.Split(':').ToList());
                        
                        if (m_Points > int.Parse(score[1]))
                        {
                            DataManager.Instance.MaxScores.Insert(i, $"{m_PlayerName} : {m_Points}");
                            isBestScore = true;
                            break;
                        }
                    }
                }

                if (DataManager.Instance.MaxScores.Count > 5)
                {
                    DataManager.Instance.MaxScores.RemoveAt(DataManager.Instance.MaxScores.Count - 1);
                }
                else if (DataManager.Instance.MaxScores.Count < 5)
                {
                    if (!isBestScore)
                    {
                        DataManager.Instance.MaxScores.Add($"{m_PlayerName} : {m_Points}");
                    }
                    else 
                    { 
                        isBestScore = false;
                    }
                }
            }
            else
            {
                string bestScore = $"{m_PlayerName} : {m_Points}";
                DataManager.Instance.MaxScores.Add(bestScore);
            }
        }
        else
        {
            Debug.Log("Game loaded outside Menu scene");
        }
    }

    int ObtainBestScore()
    {
        if (DataManager.Instance.MaxScores.Count > 0)
        {
            int returnScore;
            List<string> score = new List<string>();
            string scoreData = DataManager.Instance.MaxScores[0];
            score.Clear();
            score.AddRange(scoreData.Split(':').ToList());
            returnScore = int.Parse(score[1]);            
            return returnScore;
        }
        else
        {
            return 0;
        }
    }

    string ObtainBestPlayer()
    {
        string returnPlayer;
        List<string> score = new List<string>();
        string scoreData = DataManager.Instance.MaxScores[0];
        score.Clear();
        score.AddRange(scoreData.Split(':').ToList());
        returnPlayer = score[0];
        return returnPlayer;
    }
}
