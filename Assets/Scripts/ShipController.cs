using System;
using System.Collections;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public Action OnGameOver;
    public Action<int> OnShipDestroyed;
    
    [SerializeField] private BulletController bulletPrefab;
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private Transform bulletsParent;
    [SerializeField] private AudioSource shootAudio;
    [SerializeField] private AudioSource explodeAudio;

    private AudioSource moveAudio;
    private SpriteRenderer spriteRenderer;
    private PolygonCollider2D polygonCollider2D;
    private Pool<BulletController> bulletsPool;
    private Vector3 currentTranslate;
    private int defaultBulletsCount = 10;
    private int shipLife = 3;
    private float blinkSpeed = 5f;
    private float rotateSpeed = 2f;
    private float impulseSpeed = 1.5f;
    private float angularSpeed = 4f;
    private bool isMouseControl;
    private bool canBlink;
    private bool canShoot = true;
    private bool isShipActive;

    private void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        moveAudio = GetComponent<AudioSource>();
        bulletsPool = new Pool<BulletController>(bulletPrefab, defaultBulletsCount, bulletsParent);
    }

    public void Init()
    {
        RespawnShip();
        spriteRenderer.enabled = true;
        polygonCollider2D.enabled = true;
        shipLife = 3;
        isShipActive = true;
    }

    private void FixedUpdate()
    {
        ShipControl();
        Blink();
    }

    private void ShipControl()
    {
        if (isShipActive)
        {
            if (isMouseControl)
            {
                RotateWithMouse();
            
                if (Input.GetMouseButtonDown(0))
                    Shoot();
            
                if (Input.GetMouseButtonDown(1))
                    SetImpulse();
            }
            else
                RotateWithKeyboard();

            if (Input.GetKeyDown(KeyCode.Space))
                Shoot();
        
            if (Input.GetKeyDown(KeyCode.W))
                SetImpulse();
        
            transform.Translate(currentTranslate, Space.World);
            currentTranslate /= 1.005f;   
        }
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
        currentTranslate = transform.up * Time.deltaTime * impulseSpeed; 
        moveAudio.Play();
    }

    private void Shoot()
    {
        if (canShoot)
        {
            shootAudio.Play();
            var bullet = bulletsPool.GetFreeElement();
            bullet.InitBullet(bulletSpawn.position, this.transform.rotation);
            StartCoroutine(WaitForShoot());
        }
    }

    public void SwitchControl(bool value)
    {
        isMouseControl = !value;
    }

    private void ShipDestroyed()
    {
        shipLife--;
        currentTranslate = Vector3.zero;
        explodeAudio.Play();
        OnShipDestroyed?.Invoke(shipLife);

        if (shipLife <= 0)
        {
            GameOver();
            OnGameOver?.Invoke();
            return;
        }
        
        RespawnShip();
    }

    private void RespawnShip()
    {
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
        isShipActive = false;
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

    private IEnumerator WaitForShoot()
    {
        canShoot = false;
        
        yield return new WaitForSeconds(0.33f);

        canShoot = true;
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        var collisionGO = collider.gameObject;
        var isDestroyer = collisionGO.CompareTag("Asteroid") || collisionGO.CompareTag("EnemyBullet") || collisionGO.CompareTag("Alien");
        
        if (isDestroyer)
            ShipDestroyed();
    }
}
