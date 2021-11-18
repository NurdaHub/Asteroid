using System.Collections;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    private Rigidbody2D alienRB;
    private Vector3 playerPosition;

    public void AlienInit(Transform _playerTransform)
    {
        var alienSpeed = Random.Range(70f, 100f);
        playerPosition = _playerTransform.position;
        alienRB = GetComponent<Rigidbody2D>();
        alienRB.AddForce(transform.right * alienSpeed);

        StartCoroutine(ShootWithDelay());
    }

    private IEnumerator ShootWithDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2, 5));
            Shoot();
        }
    }

    private void Shoot()
    {
        var bulletRotation = CalculateRotation();
        var bullet = Instantiate(bulletPrefab);
        var bulletController = bullet.GetComponent<BulletController>();
        bulletController.InitBullet(this.transform.position, bulletRotation);
    }
    
    private Quaternion CalculateRotation()
    {
        var angle = Mathf.Atan2(playerPosition.x, playerPosition.y) * Mathf.Rad2Deg;
        var currentRotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        return currentRotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
            this.gameObject.SetActive(false);
        
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}
