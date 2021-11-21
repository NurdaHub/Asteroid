using System.Collections;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    private Rigidbody2D alienRB;
    private GameObject player;
    
    private Pool<BulletAlien> bulletsPool;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var collisionGO = collision.gameObject;
        var isDestroyer = collisionGO.CompareTag("Asteroid") || collisionGO.CompareTag("Bullet") || collisionGO.CompareTag("Player");
        
        if (isDestroyer)
            Destroy(this.gameObject);
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Border"))
            Destroy(this.gameObject);
    }
}
