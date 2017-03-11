﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float speed;
    public Player player;

    private Sprite defaultSprite;
    public Sprite muzzleFlash;
    public float framesToFlash;

    private SpriteRenderer spriteRend;
    CameraController camCtrl;

    public GameObject deathParticle;
    public GameObject bulletParticle;

    public int damageToGive;
    

    void Start ()
    {
        camCtrl = FindObjectOfType<CameraController>();
        spriteRend = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRend.sprite;
        player = FindObjectOfType<Player>();
        GetComponent<Rigidbody2D>().rotation = Random.Range(-5, 5);    

        if (player.transform.localScale.x < 0) // Flips the bullets relative to player.
        {
            speed = -speed;
            transform.localScale = new Vector3(-1, 1, 1);
            GetComponent<Rigidbody2D>().rotation = Random.Range(-5, 5);
        }

        StartCoroutine("MuzzleFlashCo");
	}
	

	void Update ()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * speed;
        Destroy(gameObject, 3.0f);
    }


    IEnumerator MuzzleFlashCo() // Sets first few frames of the bullet object to muzzle flash sprite.
    {
        spriteRend.sprite = muzzleFlash;
        for (int i = 0; i < framesToFlash; i++) // Countdown timer.
        {
            yield return 0;
        }
        spriteRend.sprite = defaultSprite;
    }


    void OnTriggerEnter2D(Collider2D other) // Handles the bullet's collsions.
    {
        if (other.tag != "Bullet" || other.tag != "Player" || other.tag != "Checkpoint")
        {
            if (other.tag == "Enemy")
            {
                other.GetComponent<EnemyHealthManager>().GiveDamage(damageToGive);
                //camCtrl.ShakeCamera(0.3f,0.4f);
                camCtrl.StopCoroutine("ShakeCamera");
                //camCtrl.StartCoroutine(camCtrl.ShakeCamera(0.1f,0.7f)); // Screen shake.
                //Instantiate(deathParticle, other.transform.position, other.transform.rotation);
                //Destroy(other.transform.root.gameObject);             
            }
            else
            {
                if (other.tag != "Checkpoint")
                {
                    Instantiate(bulletParticle, gameObject.transform.position, gameObject.transform.rotation);
                    Destroy(gameObject);
                }                
            }
        }
    }
}