using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienSpawner : MonoBehaviour
{
    [SerializeField] private GameObject alienPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private BulletAlien bulletAlien;
    [SerializeField] private AudioSource explodeAudio;

    private int defaultBulletsCount = 3;
    private float borderWidth;
    private float borderHeight;
    private float minPos;
    private bool isLeft;
    private Coroutine currentCoroutine;
    private GameObject currentAlien;
    
    private Pool<BulletAlien> bulletsPool;

    private void Awake()
    {
        CalculateBoreder();
        bulletsPool = new Pool<BulletAlien>(bulletAlien, defaultBulletsCount, this.transform);
    }

    public void Init()
    {
        DeleteOldAlien();
        bulletsPool.DeactivateAllElements();
        currentCoroutine = StartCoroutine(WaitTime());
    }

    private void AlienSpawn()
    {
        isLeft = Random.Range(0, 2) != 0;
        currentAlien = Instantiate(alienPrefab, CalculatePosition(), Quaternion.identity, this.transform);
        var alienController = currentAlien.GetComponent<AlienController>();
        alienController.AlienInit(player, isLeft, bulletsPool);
        alienController.OnAlienDestroyed += AlienDestroyed;

        currentCoroutine = StartCoroutine(WaitTime());
    }

    private IEnumerator WaitTime()
    {
        var randomDelay = Random.Range(20, 40);
        Debug.Log("randomDelay  " + randomDelay);
        yield return new WaitForSeconds(randomDelay);
        
        AlienSpawn();
    }

    private void CalculateBoreder()
    {
        var height = 2f * Camera.main.orthographicSize;
        var width = height * Camera.main.aspect;
        var y = height * 0.2f;
        
        borderWidth = width / 2f + 1f;
        minPos = height / 2f - y;
    }
    
    private Vector3 CalculatePosition()
    {
        var yPos = Random.Range(-minPos, minPos);
        var xPos = isLeft ? -borderWidth : borderWidth;
        var currentSpawnPosition = new Vector3(xPos, yPos, 0);

        return currentSpawnPosition;
    }

    private void AlienDestroyed()
    {
        explodeAudio.Play();
    }

    private void DeleteOldAlien()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        
        if (currentAlien != null)
            Destroy(currentAlien);
    }
}
