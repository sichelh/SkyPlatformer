using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpShooter : MonoBehaviour
{
    [SerializeField] private float shootingJumpPower = 500f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            other.GetComponent<Rigidbody>().AddForce(Vector2.up * shootingJumpPower, ForceMode.Impulse);
        }
    }
}
