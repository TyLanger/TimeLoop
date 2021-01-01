using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    float _moveSpeed;
    int _damage;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, _moveSpeed * Time.fixedDeltaTime);
    }

    public void Setup(float moveSpeed, int damage)
    {
        _moveSpeed = moveSpeed;
        _damage = damage;

        // destroy this after 10s as a precaution
        Invoke("Cleanup", 10f);
    }

    void Cleanup()
    {
        Destroy(gameObject);
    }
}
