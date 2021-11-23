using System;
using System.Collections;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public Action OnGameOver;
    public Action<int> OnShipDestroyed;

    [SerializeField] private float rotateSpeed;
    [SerializeField] private float impulseSpeed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private BulletController bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Transform bulletsParent;
    [SerializeField] private AudioSource shootAudio;
    [SerializeField] private AudioSource explodeAudio;

    private AudioSource moveAudio;
    private Rigidbody2D shipRB;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider2D;
    private Pool<BulletController> bulletsPool;
    private int defaultBulletsCount = 10;
    private int shipLife = 3;
    private float blinkSpeed = 5f;
    private bool isMouseControl;
    private bool canBlink;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        moveAudio = GetComponent<AudioSource>();
        shipRB = GetComponent<Rigidbody2D>();
        bulletsPool = new Pool<BulletController>(bulletPrefab, defaultBulletsCount, bulletsParent);
    }

    public void Init()
    {
        RespawnShip();
    }

    private void FixedUpdate()
    {
        ShipControl();
        Blink();
    }

    private void ShipControl()
    {
        if (isMouseControl)
        {
            RotateWithMouse();
            
            if (Input.GetMouseButtonDown(0))
                Shoot();
            
            if (Input.GetKey(KeyCode.Mouse1))
                SetImpulse();
        }
        else
            RotateWithKeyboard();
        
        var vertical = Input.GetAxis("Vertical");
        
        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
        
        if (vertical > 0)
            SetImpulse();
    }

    private void RotateWithMouse()
    {
        var direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, angularSpeed * Time.deltaTime);
    }

    private void RotateWithKeyboard()
    {
        var horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.forward * -horizontal * rotateSpeed);
    }

    private void SetImpulse()
    {
        shipRB.AddForce(transform.up * impulseSpeed, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        shootAudio.Play();
        var bullet = bulletsPool.GetFreeElement();
        bullet.InitBullet(bulletSpawn.position, this.transform.rotation);
    }

    public void SwitchControl(bool value)
    {
        isMouseControl = !value;
    }

    private void ShipDestroyed()
    {
        shipLife--;
        explodeAudio.Play();

        if (shipLife <= 0)
        {
            GameOver();
            OnGameOver?.Invoke();
            return;
        }
        
        OnShipDestroyed?.Invoke(shipLife);
        RespawnShip();
    }

    private void RespawnShip()
    {
        shipRB.Sleep();
        transform.position = Vector3.zero;
        StartCoroutine(Unbreakable());
    }
    
    private void Blink()
    {
        if (!canBlink)
            return;
        
        spriteRenderer.color = Color.Lerp(Color.white, Color.black, Mathf.PingPong(Time.time * blinkSpeed, 1));
    }

    private void GameOver()
    {
        spriteRenderer.enabled = false;
        polygonCollider2D.enabled = false;
    }

    private IEnumerator Unbreakable()
    {
        polygonCollider2D.enabled = false;
        canBlink = true;
        
        yield return new WaitForSeconds(3);

        canBlink = false;
        polygonCollider2D.enabled = true;
        spriteRenderer.color = Color.white;
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var collisionGO = collider.gameObject;
        var isDestroyer = collisionGO.CompareTag("Asteroid") || collisionGO.CompareTag("Bullet") || collisionGO.CompareTag("Alien");
        
        if (isDestroyer)
            ShipDestroyed();
    }
}
