using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidBase : MonoBehaviour
{
    protected Pool<MiddleAsteroid> middleAsteroidsPool;
    protected Pool<SmallAsteroid> smallAsteroidsPool;
    private Rigidbody2D asteroidRB;
    private float asteroidSpeed;

    protected Action OnAsteroidBroke;

    public void Initialize(Vector3 _position, Pool<MiddleAsteroid> _middleAsteroidsPool, Pool<SmallAsteroid> _smallAsteroidsPool)
    {
        middleAsteroidsPool = _middleAsteroidsPool;
        smallAsteroidsPool = _smallAsteroidsPool;
        this.transform.position = _position;
        this.transform.rotation = CalculateRotation();
        asteroidSpeed = Random.Range(70f, 100f);
        asteroidRB = GetComponent<Rigidbody2D>();
        asteroidRB.AddForce(transform.up * asteroidSpeed);
    }
    
    private Quaternion CalculateRotation()
    {
        var random = Random.Range(80, 100);
        var direction = Vector3.zero - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var currentRotation = Quaternion.AngleAxis(angle - random, Vector3.forward);

        return currentRotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision enter");
        
        if (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Alien"))
        {
            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
            OnAsteroidBroke?.Invoke();
            Debug.Log("broke");
        }
    }
}