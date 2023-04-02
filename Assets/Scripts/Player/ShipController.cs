using System;
using System.Collections;
using UI;
using UnityEngine;

namespace Player
{
    public class ShipController : MonoBehaviour
    {
        [SerializeField] private BulletController bulletPrefab;
        [SerializeField] private Transform bulletSpawn;
        [SerializeField] private Transform bulletsParent;
        [SerializeField] private AudioSource shootAudio;
        [SerializeField] private AudioSource explodeAudio;
        [SerializeField] private PlayerLife playerLife;

        private AudioSource moveAudio;
        private SpriteRenderer spriteRenderer;
        private PolygonCollider2D polygonCollider2D;
        private Pool<BulletController> bulletsPool;
        private Vector3 currentTranslate;
        private string asteroidTag = "Asteroid";
        private string bulletTag = "EnemyBullet";
        private string alienTag = "Alien";
        private int defaultBulletsCount = 10;
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
            isShipActive = true;
        }

        private void Update()
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
                currentTranslate /= 1.001f;   
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

        private void Destroy()
        {
            currentTranslate = Vector3.zero;
            explodeAudio.Play();
            playerLife.MinusLife();

            if (!playerLife.IsLifeEnded())
                return;

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

        public void Deactivate()
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
            var isDestroyer = collisionGO.CompareTag(asteroidTag) || collisionGO.CompareTag(bulletTag) || collisionGO.CompareTag(alienTag);
        
            if (isDestroyer)
                Destroy();
        }
    }
}
