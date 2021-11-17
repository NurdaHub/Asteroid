using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject bigAsteroid;
    [SerializeField] private GameObject middleAsteroid;
    [SerializeField] private GameObject smallAsteroid;
    [SerializeField] private Camera m_OrthographicCamera;

    private int currentAsteroidsCount = 5;
    private float borderWidth;
    private float borderHeight;
    private Dictionary<GameObject, AsteroidBase> asteroidsDict;
    private Queue<GameObject> currentAsteroids;

    public GameObject BorderLeft;
    public GameObject BorderRight;
    public GameObject BorderTop;
    public GameObject BorderBottom;

    private void OnEnable()
    {
        CalculateBoreder();
        Vector3 currentSpawnPosition = BorderLeft.transform.position;
        
        asteroidsDict = new Dictionary<GameObject, AsteroidBase>();
        currentAsteroids = new Queue<GameObject>();
        
        
        for (int i = 0; i < currentAsteroidsCount; i++)
        {
            var asteroid = Instantiate(bigAsteroid, currentSpawnPosition, Quaternion.identity, transform);
            var asteroidScript = asteroid.GetComponent<AsteroidBase>();
            
            asteroid.SetActive(false);
            
            asteroidsDict.Add(asteroid, asteroidScript);
            currentAsteroids.Enqueue(asteroid);
        }

        AsteroidBase.OnAsteroidOverBorder += ReturnAsteroid;
        
        for (int i = 0; i < currentAsteroidsCount; i++)
        {
            AsteroidSpawn();
        }
    }

    private void AsteroidSpawn()
    {
        var asteroid = currentAsteroids.Dequeue();
        var asteroidScript = asteroidsDict[asteroid];

        asteroid.transform.position = CalculatePosition();
        asteroid.transform.rotation = CalculateRotation();
        asteroid.SetActive(true);
        asteroidScript.Initialize();
    }

    private void ReturnAsteroid(GameObject currentAsteroid)
    {
        currentAsteroid.SetActive(false);
        currentAsteroids.Enqueue(currentAsteroid);
    }

    private Vector3 CalculatePosition()
    {
        var xPos = Random.Range(-borderWidth, borderWidth);
        var yPos = Random.Range(-borderHeight, borderHeight);
        var currentSpawnPosition = new Vector3(borderWidth, yPos, 0);

        return currentSpawnPosition;
    }

    private Quaternion CalculateRotation()
    {
        var angle = Mathf.Atan2(-10f, -borderWidth) * Mathf.Rad2Deg;
        var currentRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        return currentRotation;
    }

    private void CalculateBoreder()
    {
        // calculate height
        
        float height = 2f * m_OrthographicCamera.orthographicSize;
        // calculate width
        float width = height * m_OrthographicCamera.aspect;

        borderWidth = width / 2f + 1f;
        borderHeight = height / 2f + 1f;

        // my asset is 1px wide, so from the center of the asset it's 0.5f to either edge of the asset
        // which means you want to position it at the edge of the width - 0.5f
        float leftBorderPosition = -1f * (width / 2) - 1f; 
        // same with the right border
        float rightBorderPosition = width / 2 + 1f;

        // position your assets with a new vector3
        BorderLeft.transform.position = new Vector3(leftBorderPosition, 0, 0);
        BorderRight.transform.position = new Vector3(rightBorderPosition, 0, 0);

        // we want to also scale our border asset to the entire width
        // so we should calculate the ratio for the x aspect
        // grab the size of the asset 
        float spriteWidth = BorderTop.GetComponent<SpriteRenderer>().bounds.size.x;
        // grab the ratio for scale
        float ratio = width / spriteWidth;

        // and apply it
        BorderTop.transform.localScale = new Vector3(ratio, 1, 1);
        BorderBottom.transform.localScale = new Vector3(ratio, 1, 1);
        
        Debug.Log($"height  {height} ||| width  {width}");
        Debug.Log($"spriteWidth  {spriteWidth} ||| ratio  {ratio}");
    }
}
