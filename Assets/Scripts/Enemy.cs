using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{

    [SerializeField] private EnemyStats data;
    public EnemyStats Data => data;
    public static event Action<EnemyStats> onEnemyReachedEnd;
    public static event Action<Enemy> onEnemyDestroyed;
    private Path currentPath;
    private Vector3 targetPosition;
    private int currentWaypoint;
    private float _lives;
    private float _maxLives;

    [SerializeField] private Transform healthBar;
    private Vector3 healthBarOriginalScale;

    private void Awake()
    {
        currentPath = GameObject.Find("Waypoints").GetComponent<Path>();
        healthBarOriginalScale = healthBar.localScale;
    }

    private void OnEnable()
    {
        // Set the enemy on the right paht uppon spawn
        currentWaypoint = 0;
        targetPosition = currentPath.getPosition(currentWaypoint);
    }

    void Update()
    {
        // Follow the intended path
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, data.speed * Time.deltaTime);

        float relativeDistance = (transform.position - targetPosition).magnitude;
        // Destroying enemy and taking away lives
        if (relativeDistance < 0.1f)
        {
            if (currentWaypoint < currentPath.waypoints.Length - 1)
            {
            currentWaypoint++;
            targetPosition = currentPath.getPosition(currentWaypoint);
            }
            else
            {
                onEnemyReachedEnd?.Invoke(data);
                gameObject.SetActive(false);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        // What??? you took damamge??
        _lives -= damage;
        _lives = Math.Max(_lives, 0);
        UpdateHealthBar();
        if (_lives <= 0)
        {
            onEnemyDestroyed?.Invoke(this);
            gameObject.SetActive(false);
        }
    }

    private void UpdateHealthBar()
    {
        // Update the health bar scale based on the current lives
        float healthPercent = _lives / data.lives;
        Vector3 scale = healthBarOriginalScale;
        scale.x = healthBarOriginalScale.x * healthPercent;
        healthBar.localScale = scale;
    }

    public void Initialize(float healthMultiplier)
    {
        _maxLives = data.lives * healthMultiplier;
        UpdateHealthBar();
        _lives = _maxLives;
    }
}
