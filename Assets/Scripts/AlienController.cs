using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienController : MonoBehaviour
{
    private GameObject player;
    private Pool<BulletAlien> bulletsPool;
    private Vector3 direction;
    private int points = 200;
    private float alienSpeed;
    private string asteroidTag = "Asteroid";
    private string playerTag = "Player";
    private string bulletTag = "Bullet";

    public Action OnAlienDestroyed;

    public void AlienInit(GameObject _player, bool _isLeft, Pool<BulletAlien> _bulletsPool)
    {
        alienSpeed = Random.Range(1f, 2f);
        direction = _isLeft ? transform.right : -transform.right;
        player = _player;
        bulletsPool = _bulletsPool;

        StartCoroutine(WaitTime());
    }
    
    private void Update()
    {
        MoveAlien();
    }

    private void MoveAlien()
    {
        var translate = direction * Time.deltaTime * alienSpeed; 
        transform.Translate(translate);
    }

    private IEnumerator WaitTime()
    {
        var randomDelay = Random.Range(2, 5);
        yield return new WaitForSeconds(randomDelay);
        
        Shoot();
    }

    private void Shoot()
    {
        var bulletRotation = CalculateRotation();
        var bullet = bulletsPool.GetFreeElement();
        
        bullet.InitBullet(this.transform.position, bulletRotation);

        StartCoroutine(WaitTime());
    }
    
    private Quaternion CalculateRotation()
    {
        var direction = player.transform.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var currentRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        return currentRotation;
    }

    private void OnBroke()
    {
        OnAlienDestroyed?.Invoke();
        Destroy(this.gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var isDestroyer = collider.CompareTag(asteroidTag) || collider.CompareTag(playerTag);
        
        if (isDestroyer)
            OnBroke();

        if (collider.CompareTag(bulletTag))
        {
            UIController.currentScore += points;
            OnBroke();
        }
    }
}
