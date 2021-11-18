using UnityEngine;

public class BulletBase : MonoBehaviour
{
    private Rigidbody2D bulletRB;
    private float bulletSpeed = 150f;

    public void InitBullet(Vector3 _position, Quaternion _rotation)
    {
        this.transform.position = _position;
        this.transform.rotation = _rotation;
        bulletRB = GetComponent<Rigidbody2D>();
        bulletRB.AddForce(transform.up * bulletSpeed);
    }
    
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Border"))
            this.gameObject.SetActive(false);
    }
}
