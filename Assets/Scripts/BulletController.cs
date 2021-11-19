using System;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D bulletRB;
    private float bulletSpeed = 150f;
    private float maxDistance;
    private float currentDistance;
    private Vector3 prevPosition;

    public void InitBullet(Vector3 _position, Quaternion _rotation)
    {
        this.transform.position = _position;
        this.transform.rotation = _rotation;
        bulletRB = GetComponent<Rigidbody2D>();
        bulletRB.AddForce(transform.up * bulletSpeed);
        prevPosition = this.transform.position;
    }

    private void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        var distance = Vector3.Distance(prevPosition, this.transform.position);
        prevPosition = this.transform.position;
        
        if (distance > 1f)
            return;
        
        currentDistance += distance;

        if (currentDistance > AsteroidSpawner.maxDistance)
        {
            this.gameObject.SetActive(false);
            currentDistance = 0;
        }
    }
}
