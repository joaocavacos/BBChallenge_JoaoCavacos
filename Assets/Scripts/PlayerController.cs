using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [Header("Player Movement"), Space]
    public float defaultMoveSpeed = 3f;
    public float initialMoveSpeed = 3f;
    [Header("Player Jumping"), Space]
    public float jumpSpeedMultiplier = 1f;
    public float jumpForce = 10f;
    public float jumpCooldown = 0.2f;
    private float lastJumpTime;
    
    public float playerHeight = 2f;
    public LayerMask GroundLayer;

    private Collider lastPlatformCollided;

    public ParticleSystem explosionParticle;
    public Transform particleTransform;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        lastJumpTime = -jumpCooldown;
    }

    void Update()
    {
        if (!GameDirector.Instance.isPlaying) return;
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded() && canJump())
        {
            PlayerJump();
        }

        //print("Can Jump? =" + canJump());
    }

    private void FixedUpdate()
    {
        MovePlayer();
        UpdateJumpCooldown();
    }

    private void MovePlayer()
    {
        rb.MovePosition(transform.position + Vector3.right * (defaultMoveSpeed * Time.deltaTime));
    }

    private void PlayerJump()
    {
        var jumpDir = transform.right + (Vector3.up * 1.5f);
        jumpDir.Normalize();
        
        rb.AddForce(jumpDir * jumpForce, ForceMode.Impulse);

        lastJumpTime = Time.fixedTime;
        
        //StartCoroutine(ForwardVelocityIncrease());
    }

    private IEnumerator ForwardVelocityIncrease()
    {
        yield return new WaitForSeconds(0.01f);
        
        while (!isGrounded())
        {
            //print("Increasing forward speed");
            defaultMoveSpeed += jumpSpeedMultiplier * Time.deltaTime;
            yield return null;
        }

        defaultMoveSpeed = initialMoveSpeed;
    }

    private void UpdateJumpCooldown()
    {
        if (!isGrounded() && Time.fixedTime - lastJumpTime <= jumpCooldown)
        {
            lastJumpTime = Time.fixedTime;
        }
    }

    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.51f, GroundLayer);
    }

    private bool canJump()
    {
        return Time.time - lastJumpTime > jumpCooldown;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            var go = Instantiate(explosionParticle, particleTransform.position, Quaternion.identity);
            Destroy(go.gameObject, 2f);
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, Vector3.down * (playerHeight * 0.51f), Color.red);
    }
}
