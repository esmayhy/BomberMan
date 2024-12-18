using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager1 : MonoBehaviour
{
    private int currentScore = 0;
    private int lives = 3;
    private int maxBombs = 1;
    private int explodeRange = 1;
    private float moveSpeed = 4f;
    private int speedCounter = 1;

    [SerializeField] private int bombLimit = 6;
    [SerializeField] private int explodeLimit = 5;
    [SerializeField] private int speedLimit = 5;
    private float speedIncrease = .4f;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerParentTransform;

    private PlayerController currentPlayer;

    [SerializeField] private float delayToSpawnPlayer = 1f;

    [SerializeField] private CameraController1 myCamera;

    [SerializeField] private Text ScoreText;
    [SerializeField] private Text LivesText;
    [SerializeField] private Text maxBombsText;
    [SerializeField] private Text explodeRangeText;
    [SerializeField] private Text speedText;
    [SerializeField] private Text levelNameText;

    private bool isPaused = false;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject WinGamePanel;

    private int enemiesThisLevel = 0;

    [SerializeField] private string nextLevelToLoad;

    [SerializeField] private bool isLastLevel = false;

    const string CurrentScoreKey = "CurrentScore";
    const string LivesKey = "Lives";
    const string MaxBombKey = "MaxBombs";
    const string ExplodeRangeKey = "ExplodeRange";
    const string MoveSpeedKey = "MoveSpeed";


    private void Awake()
    {
        LoadPlayerPrefs();
    }
    
    void Start()
    {
        UpdateScore(0);
        UpdateLivesText();
        UpdateMaxBombsText();
        UpdateExplodeRangeText();
        UpdateSpeedText();
        UpdateLevelText();
        SpawnPlayer();

        enemiesThisLevel = GetEnemyCount();


    }

    // Update is called once per frame
    public void PlayerDied()
    {
        if (lives > 1)
        {
            lives--;

            Invoke("SpawnPlayer", currentPlayer.GetDestroyDelayTime() + delayToSpawnPlayer);
        }
        else
        {
            Invoke("GameOver", 2f);
        }

    }
    private void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, new Vector3(1, 16, 1), Quaternion.identity, playerParentTransform);
        currentPlayer = player.GetComponent<PlayerController>();
        currentPlayer.InitializePlayer(maxBombs, moveSpeed);
        myCamera.SetPlayer(player);
        UpdateLivesText();
    }
    public void UpdateScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        ScoreText.text = "Score:" + currentScore.ToString("D6");
    }

    private void UpdateLivesText()
    {
        LivesText.text = lives.ToString("D2");

    }

    private void UpdateMaxBombsText()
    {
        maxBombsText.text =  maxBombs.ToString("D2");

    }

    private void UpdateExplodeRangeText()
    {
        explodeRangeText.text = explodeRange.ToString("D2");

    }

    private void UpdateSpeedText()
    {
        speedText.text =  speedCounter.ToString("D2");

    }

    private void UpdateLevelText()
    {
        levelNameText.text = SceneManager.GetActiveScene().name;
    }

    public void PauseButton()
    {
        if(isPaused)
        {
            pausePanel.SetActive(false);
            currentPlayer.SetPaused(false);
            isPaused = false;
            Time.timeScale = 1f;
        }
        else
        {
            pausePanel.SetActive(true);
            currentPlayer.SetPaused(true);
            isPaused = true;
            Time.timeScale = 0f;

        }
    }

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private int GetEnemyCount()
    {
        int count = GameObject.FindGameObjectsWithTag("Enemy").Length;
        return count;
    }
    public void EnemyHasDied()
    {
        enemiesThisLevel--;

        if(enemiesThisLevel <= 0)
        {
            if (isLastLevel)
            {
                currentPlayer.PlayVictory();
                Invoke("DisplayWinPanel", 3f);
            }
            else
            {
                currentPlayer.PlayVictory();
                SavePlayerData();
                Invoke("LoadNextLevel", 3f);
            }
            
        }
    }

    private void DisplayWinPanel()
    {
        WinGamePanel.SetActive(true);
    }
    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);

    }
    public void IncreaseMaxBombs()
    {
        maxBombs++;
        maxBombs = Mathf.Clamp(maxBombs, 1, bombLimit);
        UpdateMaxBombsText();
        currentPlayer.InitializePlayer(maxBombs, moveSpeed);
    }

    public void IncreaseSpeed()
    {
        if(speedCounter < speedLimit)
        {
            moveSpeed += speedIncrease;
            //moveSpeed = Mathf.Clamp(moveSpeed, 4f, speedLimit);
            speedCounter++;
            UpdateSpeedText();
            currentPlayer.InitializePlayer(maxBombs, moveSpeed);
        }
        
    }

    public int GetExplodeRange()
    {
        return explodeRange;
    }

    public void IncreaseExplodeRange()
    {
        explodeRange++;
        explodeRange = Mathf.Clamp(explodeRange, 1, explodeLimit);
        UpdateExplodeRangeText();
    }

    private void SavePlayerData()
    { 
        PlayerPrefs.SetInt(CurrentScoreKey, currentScore);
        PlayerPrefs.SetInt(LivesKey, lives);
        PlayerPrefs.SetInt(MaxBombKey, maxBombs);
        PlayerPrefs.SetInt(ExplodeRangeKey, explodeRange);
        PlayerPrefs.SetFloat(MoveSpeedKey, moveSpeed);

    }

    private void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(CurrentScoreKey))
        {
            currentScore = PlayerPrefs.GetInt(CurrentScoreKey);
        }
        if (PlayerPrefs.HasKey(LivesKey))
        {
            lives = PlayerPrefs.GetInt(LivesKey);
        }
        if (PlayerPrefs.HasKey(MaxBombKey))
        {
            maxBombs = PlayerPrefs.GetInt(MaxBombKey);
        }
        if (PlayerPrefs.HasKey(ExplodeRangeKey))
        {
            explodeRange = PlayerPrefs.GetInt(ExplodeRangeKey);
        }
        if (PlayerPrefs.HasKey(MoveSpeedKey))
        {
            moveSpeed = PlayerPrefs.GetFloat(MoveSpeedKey);
        }
    }


}

