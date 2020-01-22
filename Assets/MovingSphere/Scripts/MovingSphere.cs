using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSphere : MonoBehaviour
{
    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10f, maxAirAcceleration = 1f;

    [SerializeField, Range(0f, 10f)]
    float jumpHeight = 2f;

    [SerializeField, Range(0, 5)]
    int maxAirJumps = 0;
    // [SerializeField]
    // Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);

    // [SerializeField, Range(0f,1f)]
    // float bounciness = 0.5f;

    Rigidbody body;
    Vector3 velocity, desiredVelocity;
    bool desiredJump = false;
    bool onGround;
    int jumpPhase;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }
    void Start()
    {
        velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 playerInput;
        desiredJump |= Input.GetButtonDown("Jump");
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        // Vector3 displacement = velocity * Time.deltaTime;

        // Vector3 newPosition = transform.localPosition + displacement;

        // if(newPosition.x < allowedArea.xMin)
        // {
        //     newPosition.x = allowedArea.xMin;
        //     velocity.x = - velocity.x * bounciness;
        // }
        // else if(newPosition.x > allowedArea.xMax)
        // {
        //     newPosition.x = allowedArea.xMax;
        //     velocity.x = - velocity.x * bounciness;
        // }

        // if(newPosition.z < allowedArea.yMin)
        // {
        //     newPosition.z = allowedArea.yMin;
        //     velocity.z = - velocity.z * bounciness;
        // }
        // else if(newPosition.z > allowedArea.yMax)
        // {
        //     newPosition.z = allowedArea.yMax;
        //     velocity.z = - velocity.z * bounciness;
        // }
        // transform.localPosition = newPosition;
    }

    private void FixedUpdate()
    {
        UpdateState();
        float acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z = Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }

        body.velocity = velocity;
        onGround = false;
    }

    private void UpdateState()
    {
        velocity = body.velocity;
        if (onGround)
        {
            jumpPhase = 0;
        }
    }

    private void Jump()
    {
        if (onGround || jumpPhase < maxAirJumps)
        {
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            if(velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            velocity.y += jumpSpeed;

        }
    }

    private void OnCollisionEnter(Collision other)
    {
        onGround = true;
        EvaluateCollision(other);
    }
    private void OnCollisionStay(Collision other)
    {
        onGround = true;
        EvaluateCollision(other);
    }

    private void OnCollisionExit(Collision other)
    {
        onGround = false;
    }

    private void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= 0.9f;
        }
    }
}