using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{

    [SerializeField] private TowerData data;
    private CircleCollider2D circleCollider;
    private ObjectPooling projectilePool;

    private List<Enemy> enemiesInRange;
    private float shootTimer;


    private void OnEnable()
    {
        Enemy.onEnemyDestroyed += HandeEnemyDestroyed;
    }
    private void OnDisable()
    {
        Enemy.onEnemyDestroyed -= HandeEnemyDestroyed;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, data.range);
    }

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = data.range;
        enemiesInRange = new List<Enemy>();
        projectilePool = GetComponent<ObjectPooling>();
        shootTimer = data.shootInterval;
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0 )
        {
            shootTimer = data.shootInterval;
            Shoot();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemiesInRange.Add(enemy);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }

    private void Shoot()
    {
        if (enemiesInRange.Count > 0)
        {
            GameObject projectile = projectilePool.GetPooledObject();
            projectile.transform.position = transform.position;
            projectile.SetActive(true);
            Vector2 shootDirection = (enemiesInRange[0].transform.position - transform.position).normalized;
            projectile.GetComponent<Projectile>().Shoot(data, shootDirection);
        }
    }
    
    private void HandeEnemyDestroyed(Enemy enemy)
    {
        enemiesInRange.Remove(enemy);
    }
}
