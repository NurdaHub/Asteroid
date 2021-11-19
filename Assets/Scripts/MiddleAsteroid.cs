using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleAsteroid : AsteroidBase
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
            var asteroid = smallAsteroidsPool.GetFreeElement();
            asteroid.Initialize(this.transform.position, middleAsteroidsPool, smallAsteroidsPool);
        }
    }
}
