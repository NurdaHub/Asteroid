using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float impulseSpeed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletParent;
    [SerializeField] private Transform bulletSpawnPos;
    
    private Rigidbody2D shipRB;
    public bool isMouseControl;

    private void OnEnable()
    {
        shipRB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        ShipControl();
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
        Debug.Log("Shoot");
        Instantiate(bulletPrefab, bulletSpawnPos.position, transform.rotation, bulletParent);
    }
}
