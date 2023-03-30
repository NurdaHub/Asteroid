using UnityEngine;

public class BulletBase : MonoBehaviour
{
    private Vector3 prevPosition;
    private float bulletSpeed = 3f;
    private float currentDistance;

    public void InitBullet(Vector3 _position, Quaternion _rotation)
    {
        this.transform.position = _position;
        this.transform.rotation = _rotation;
        prevPosition = this.transform.position;
    }

    private void Update()
    {
        CheckDistance();
        MoveBullet();
    }

    private void MoveBullet()
    {
        var translate = Vector3.up * Time.deltaTime * bulletSpeed; 
        transform.Translate(translate);
    }

    private void CheckDistance()
    {
        var distance = Vector3.Distance(prevPosition, this.transform.position);
        prevPosition = this.transform.position;
        
        if (distance > 1f)
            return;
        
        currentDistance += distance;

        if (currentDistance > AsteroidSpawner.MaxDistance)
        {
            this.gameObject.SetActive(false);
            currentDistance = 0;
        }
    }
}
