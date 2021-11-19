﻿using System;
using UnityEngine;

public class BigAsteroid : AsteroidBase
{
    private int brokenPieceCount = 2;

    private void OnEnable()
    {
        OnAsteroidBroke += AsteroidBroke;
    }

    private void AsteroidBroke()
    {
        for (int i = 0; i < brokenPieceCount; i++)
        {
            var asteroid = middleAsteroidsPool.GetFreeElement();
            asteroid.Initialize(this.transform.position, middleAsteroidsPool, smallAsteroidsPool);
        }
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     Debug.Log("collision enter");
    //     
    //     if (collision.gameObject.CompareTag("Bullet"))
    //     {
    //         
    //     }
    // }
}
