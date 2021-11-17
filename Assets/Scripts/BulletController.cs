using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D bulletRB;
    private float bulletSpeed = 150f;

    private void OnEnable()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        bulletRB.AddForce(transform.up * bulletSpeed);
    }
}
