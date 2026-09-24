using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }
    public static event Action<int> OnLivesChanged;
    public static event Action<int> OnRecourceChange;
    public static event Action<int> OnScoreChange;
    private int lives;
    private int _recources;
    private int score = 0;
    public int Score => score;
    public int Recources => _recources;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
            instance = this;
    }

    private void OnEnable()
    {
        Enemy.onEnemyReachedEnd += HandleEnemyReachEnd;
        Enemy.onEnemyDestroyed += HandleEnemyDestroyed;
    }

    private void OnDisable()
    {
        Enemy.onEnemyReachedEnd -= HandleEnemyReachEnd;
        Enemy.onEnemyDestroyed -= HandleEnemyDestroyed;
    }

    private void Start()
    {
        lives = 20;
        _recources = 50;
        OnLivesChanged?.Invoke(lives);
        OnRecourceChange?.Invoke(_recources);
        OnScoreChange?.Invoke(score);
    }

    private void HandleEnemyReachEnd(EnemyStats data)
    {
        //changing the players lives
        lives = Mathf.Max(0, lives - data.damage);
        OnLivesChanged?.Invoke(lives);
    }
    private void HandleEnemyDestroyed(Enemy enemy)
    {
        AddRecources(enemy.Data.recourceReward);
        AddScore(enemy.Data.scoreReward);
    }
    private void AddRecources(int amount)
    {
        _recources += amount;
        OnRecourceChange?.Invoke(_recources);
    }
    private void AddScore(int amount)
    {
        score += amount;
        OnScoreChange?.Invoke(score);
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }
    public void SpendRecources(int amount)
    {
        if (_recources >= amount)
        {
            _recources -= amount;
            OnRecourceChange?.Invoke(_recources);
        }
    }
}
