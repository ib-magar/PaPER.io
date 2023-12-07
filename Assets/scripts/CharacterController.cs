using System;
using Unity.Properties;
using UnityEditor.SearchService;
using UnityEditorInternal;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of character movement
    public float rotationSpeed = 200f; // Speed of character rotation

    private Rigidbody rb;
    public bool _continuousWalk = false;
    public Vector3 continuousVelDir;
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        rb.velocity=new Vector3 (0, 0, moveSpeed);  
    }

    void Update()
    {
        Vector3 movement = Vector3.zero; // Initialize movement as zero vector

        // Get input from WASD keys
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical"); // Use 'z' for 3D movement


        // Normalize the movement vector to ensure consistent speed in all directions
        movement.Normalize();
        // Move the character based on the input
        if (!_continuousWalk)
            rb.velocity = new Vector3(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);
        else
            rb.velocity = continuousVelDir*moveSpeed;   
        // Rotate the character's up direction towards the movement direction
        if (movement != Vector3.zero && !_continuousWalk)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
    public LayerMask pathLayer;
    public LayerMask enemyLayer;
    public GameObject gameOverUI;
    public GameObject dieEffect;
    private void OnTriggerEnter(Collider other)
    {
        int objLayerMask = 1 << other.gameObject.layer;
        if ((pathLayer.value & objLayerMask) != 0 || (enemyLayer.value & objLayerMask) != 0)
        {
           if(other.TryGetComponent<pathObj>(out pathObj _pathobj))
            {
                _pathobj.characterController.Die();
            }
            else
            {
                Die();
            }

        }
       
    }

    public void Die()
    {
        gameOverUI.SetActive(true);
        Instantiate(dieEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
