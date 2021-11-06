using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [HideInInspector]
    public static Ball Instance;

    [HideInInspector]
    public AudioSource sound;

    [HideInInspector]
    public bool isOutOfBricks;

    [Tooltip("Drag here the shoot clip.")] public AudioClip shoot;
    [Tooltip("Drag here the bump clip.")] public AudioClip bump;

    class Tags
    {
        public static string brick = "Brick";
    }

    private Rigidbody m_Rigidbody;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ObtainComponents();
    }
    
    private void OnCollisionExit(Collision other)
    {
        PlaySoundOnBounce(other);
        Bounce();
        isOutOfBricks = CheckOutOfBricks();        
    }

    // Obtain the ball components needed to the ball code.
    private void ObtainComponents()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();        
    }

    // Make the ball bounce when it hits an obstacle.
    private void Bounce()
    {
        var velocity = m_Rigidbody.velocity;

        //after a collision we accelerate a bit
        velocity += velocity.normalized * 0.01f;

        //check if we are not going totally vertically as this would lead to being stuck, we add a little vertical force
        if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.1f)
        {
            velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
        }

        //max velocity
        if (velocity.magnitude > 3.0f)
        {
            velocity = velocity.normalized * 3.0f;
        }

        m_Rigidbody.velocity = velocity;
    }

    void PlaySoundOnBounce(Collision obstacle)
    {
        if (obstacle.gameObject.CompareTag(Tags.brick))
        {
            sound.PlayOneShot(bump);
        }
        else
        {
            sound.PlayOneShot(shoot);
        }
    }

    bool CheckOutOfBricks()
    {
        bool isEmpty = false;
        isEmpty = GameObject.FindGameObjectsWithTag(Tags.brick).Length < 1 ? true : false;        
        return isEmpty;
    }
}
