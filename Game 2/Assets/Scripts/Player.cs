﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    public float moveSpeed;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    public Transform firePoint;
    public GameObject bullet;
    private float bulletStart = 0f;
    public float bulletFireRate;
    public float firingShake;
    public float firingShakeLength;
    private float bulletKick;
    private float cameraKick;
    public bool isFiring;
    public bool canMove;

    CameraController camCtrl;
    Controller2D controller;
    PauseMenu pauseMen;
    Rigidbody2D rb2d;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        controller = GetComponent<Controller2D>();
        camCtrl = FindObjectOfType<CameraController>();
        pauseMen = FindObjectOfType<PauseMenu>();
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        cameraKick = -0.1f;
        bulletKick = -0.5f;
    }


    void Update()
    {

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            Debug.Log("space pressed");
            velocity.y = jumpVelocity;
        }

        if (Input.GetKey(KeyCode.Return) && Time.time > bulletStart + bulletFireRate && pauseMen.isPaused == false) // Shoots bullets
        {
            isFiring = true;
            Instantiate(bullet, firePoint.position, firePoint.rotation);
            bulletStart = Time.time;
            rb2d.AddForce(new Vector2(bulletKick, 0)); // causes issues with jumping when rigidbody is dynamic.
            Camera.main.transform.Translate(new Vector3(cameraKick, 0, 0));
            camCtrl.StartCoroutine(camCtrl.ShakeCamera(firingShake, firingShakeLength));
        }

        if (Input.GetKeyUp(KeyCode.Return))
        {
            isFiring = false;
        }

        if (Input.GetAxisRaw("Horizontal") == 1 && isFiring == false) // Rotates player unless they're firing.
        {
            transform.localScale = new Vector3(1, 1, 1);
            bulletKick = -0.5f;
            cameraKick = -0.1f;
        }
        else if (Input.GetAxisRaw("Horizontal") == -1 && isFiring == false)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            bulletKick = 0.5f;
            cameraKick = 0.1f;
        }

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}