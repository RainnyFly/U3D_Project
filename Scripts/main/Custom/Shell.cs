using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

    public GameObject shellExplsPfb;
    public AudioClip shellExplosionAudio;

    private Transform pfbParent;
    [HideInInspector]
    public bool isEnemyShell = false;

	// Use this for initialization
	void Start () {
        pfbParent = GameObject.FindGameObjectWithTag("PrefabParent").transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayClipAtPoint(shellExplosionAudio, transform.position);
        GameObject go = GameObject.Instantiate(shellExplsPfb, transform.position, transform.rotation);
        go.transform.SetParent(pfbParent);
        Destroy(go, 1.2f);
        Destroy(gameObject);
        //判断子弹类别 
        if ((!isEnemyShell && other.tag == "Tank") || other.tag == "Player")
        {
            other.SendMessage("TakeDamage");
        }
        
    }
}
