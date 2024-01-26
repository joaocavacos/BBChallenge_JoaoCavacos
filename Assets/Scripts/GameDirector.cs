using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }

    public Transform player;
    private float initialPlayerPosX;
    
    public TMP_Text countdownText;
    public TMP_Text distanceText;
    public TMP_Text scoreText;
    public TMP_Text scoreTextGameOver;
    
    public float initialCountdown = 60f;
    public float scoreCountdown = 60f;
    private float distanceCovered;
    private float score = 0f;
    [SerializeField] private float distanceScore;
    [SerializeField] private float platformScore;

    public bool isPlaying = true;

    public UnityEvent OnGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        initialPlayerPosX = player.position.x;
    }

    void Start()
    {
        scoreCountdown = initialCountdown;
    }

    void Update()
    {
        if (!isPlaying) return;
        
        UpdateCooldown();
        UpdateDistance();
    }

    private void UpdateCooldown()
    {
        scoreCountdown -= Time.deltaTime;
        countdownText.text = Mathf.Max(0, Mathf.Round(scoreCountdown)).ToString("F0") + "s";

        if (scoreCountdown <= 0)
        {
            HandleGameOver();
        }
    }

    private void UpdateDistance()
    {
        distanceCovered = Mathf.Abs(player.position.x - initialPlayerPosX);
        distanceText.text = distanceCovered.ToString("F0") + "m";

        distanceScore = Mathf.RoundToInt(distanceCovered);
        UpdateScore();
    }

    private void UpdateScore()
    {
        score = distanceScore + platformScore;
        scoreText.text = score.ToString();
    }

    public void IncreasePlatformScore(int num)
    {
        platformScore += num;
    }

    public void HandleGameOver()
    {
        isPlaying = false;
        OnGameOver.Invoke();
        Time.timeScale = 0f;
        
        scoreTextGameOver.text = "SCORE: " + score;
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartScene");
    }
}
