using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour {

    public float speed = 5f;
    public float angleSpeed = 3f;
    public AudioClip idleAudio;
    public AudioClip moveAudio;
    
    private Rigidbody rgdbody;
    private AudioSource audio;
    
    // Use this for initialization
    void Start ()
    {
        rgdbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
    //控制坦克移动
    void FixedUpdate ()
    {
        float v = Input.GetAxis("Vertical");
        rgdbody.velocity = transform.forward * v * speed;
        float h = Input.GetAxis("Horizontal");
        rgdbody.angularVelocity = transform.up * h * angleSpeed;

        if (Mathf.Abs(v) > 0.1 || Mathf.Abs(h) > 0.1)
        {
            audio.clip = moveAudio;
            if (!audio.isPlaying)
                audio.Play();
        }
        else
        {
            audio.clip = idleAudio;
            if (!audio.isPlaying)
                audio.Play();
        }
    }
}
