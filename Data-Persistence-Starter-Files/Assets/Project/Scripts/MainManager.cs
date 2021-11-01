using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        BuildBrickWall();
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

    // In Update MainManager event check game states, if it do not have started and if it have been finished:

    void CheckGameStates()
    {
        if (!m_Started)
        {
            InputStartGame();
        }
        else if (m_GameOver)
        {
            InputRestartGame();
        }
    }

    // If the game is not started Input for start gema, and if the game is over, input for restart game.

    void InputStartGame()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Started = true;
            float randomDirection = Random.Range(-1.0f, 1.0f);
            Vector3 forceDir = new Vector3(randomDirection, 1, 0);
            forceDir.Normalize();

            Ball.transform.SetParent(null);
            Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
        }
    }

    void InputRestartGame()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Add points to score when the brick is bounced by the ball and destroyed, it is called in BuildBrickWall for the 
    // Brick,OnDestroy event.
    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    // Sets the game over flag and shows the game over user interface screen. It is called in DeathZone class.
    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
