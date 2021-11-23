using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class AlienController : MonoBehaviour
{
    private Rigidbody2D alienRB;
    private GameObject player;
    private Pool<BulletAlien> bulletsPool;
    private int points = 200;

    public Action OnAlienDestroyed;

    public void AlienInit(GameObject _player, bool _isLeft, Pool<BulletAlien> _bulletsPool)
    {
        var alienSpeed = Random.Range(40f, 60f);
        var direction = _isLeft ? transform.right : -transform.right;
        
        player = _player;
        alienRB = GetComponent<Rigidbody2D>();
        alienRB.AddForce(direction * alienSpeed);
        bulletsPool = _bulletsPool;

        StartCoroutine(WaitTime());
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
        var isDestroyer = collider.CompareTag("Asteroid") || collider.CompareTag("Player");
        
        if (isDestroyer)
            OnBroke();

        if (collider.CompareTag("Bullet"))
        {
            UIController.currentScore += points;
            OnBroke();
        }
    }
}
