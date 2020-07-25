using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAttack : MonoBehaviour {
    
    public GameObject shellPrefab;
    public AudioClip shellAudio;
    public KeyCode fireKey = KeyCode.K;
    public float shellSpeed = 15f;
    
    private Transform firePos;
    private Transform shellParent;

    // Use this for initialization
    void Start () {
        firePos = transform.Find("FirePostion");
        shellParent = GameObject.FindGameObjectWithTag("PrefabParent").transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(fireKey))//发射子弹
        {
            AudioSource.PlayClipAtPoint(shellAudio, transform.position);//发射子弹音效
            GameObject shellGo = GameObject.Instantiate(shellPrefab, firePos.position, firePos.rotation);
            shellGo.transform.SetParent(shellParent);//将创建出来的子弹放到shellParent下
            shellGo.GetComponent<Rigidbody>().velocity = firePos.forward * shellSpeed;
        }
    }
}
