using System.Collections;
using UnityEngine;

public class AlienSpawner : MonoBehaviour
{
    [SerializeField] private GameObject alienPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private BulletController bulletController;

    private int defaultBulletsCount = 3;
    private float borderWidth;
    private float borderHeight;
    private float minPos;
    private bool isLeft;
    
    private Pool<BulletController> bulletsPool;

    private void OnEnable()
    {
        CalculateBoreder();
        StartCoroutine(WaitTime());
        
        bulletsPool = new Pool<BulletController>(bulletController, defaultBulletsCount, this.transform);
    }

    private void AlienInit()
    {
        isLeft = Random.Range(0, 2) != 0;
        var alien = Instantiate(alienPrefab, CalculatePosition(), Quaternion.identity, this.transform);
        var alienController = alien.GetComponent<AlienController>();
        alienController.AlienInit(player, isLeft, bulletsPool);

        StartCoroutine(WaitTime());
    }

    private IEnumerator WaitTime()
    {
        var randomDelay = Random.Range(20, 40);
        Debug.Log("randomDelay  " + randomDelay);
        yield return new WaitForSeconds(randomDelay);
        
        AlienInit();
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
}
