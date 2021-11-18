using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidBase : MonoBehaviour
{
    public static Action<GameObject> OnAsteroidOverBorder;
    private Rigidbody2D asteroidRB;
    private float asteroidSpeed;

    public void Initialize()
    {
        asteroidSpeed = Random.Range(70f, 100f);
        asteroidRB = GetComponent<Rigidbody2D>();
        asteroidRB.AddForce(transform.up * asteroidSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision enter");
        
        if (collision.gameObject.CompareTag("Bullet"))
        {
            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
            Debug.Log("is bullet");
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Border"))
            OnAsteroidOverBorder?.Invoke(gameObject);
    }
}