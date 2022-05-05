using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public int destroyValue;

    public float forceToDestroy;
    public static float destroyCountdown = 1.5f;
    public GameObject particles;

    private Rigidbody rb;
    private Vector3 dir;

    public float force = 300f;

    ScoreHandler scoreHandler;

    private void Awake()
    {
        scoreHandler = FindObjectOfType<ScoreHandler>();
        rb = GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            dir = collision.contacts[0].point - transform.position;
            dir = -dir.normalized;
            rb.AddForce(dir*force);
            Invoke("DestroyMe", destroyCountdown);
        }
    }


    void DestroyMe()
    {
        Instantiate(particles, transform.position, transform.rotation);
        Destroy(this.gameObject);
        scoreHandler.UpdateScore(destroyValue);
    }
}
