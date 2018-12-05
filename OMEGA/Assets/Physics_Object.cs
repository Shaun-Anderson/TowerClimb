using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics_Object : MonoBehaviour {
    public Controller2D controller;
    public Vector3 velocity;
    public float gravity;
    public int moveSpeed;
    public float velocityXSmoothing;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .4f;
    float maxJumpVelocity;
    float minJumpVelocity;

    public Vector2 directionalInput;
    public bool groundCollison;
    // Use this for initialization
    void Start () {
        controller = GetComponent<Controller2D>();

    }

    // Update is called once per frame
    void Update()
    {
        CalculateVelocity();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            groundCollison = true;
            if (controller.collisions.slidingDownMaxSlope)
            {
                velocity.y += controller.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            }
            else
            {
                velocity.y = 0;
            }
        }
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }
}
