using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttck : MonoBehaviour {

    public GameObject shellPrefab;

    private Transform firePos;
    private Transform shellParent;
    private float shellSpeed = 15f;
    private bool isFire = false;
    // Use this for initialization
    void Start () {
        firePos = transform.Find("FirePostion");
        shellParent = GameObject.FindGameObjectWithTag("PrefabParent").transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Attack()
    {
        if (!isFire)
        {
            Fire();
            Invoke("SetFireState", 1f);
        }
    }

    void Fire()
    {
        isFire = true;
        GameObject shellGo = GameObject.Instantiate(shellPrefab, firePos.position, firePos.rotation);
        shellGo.transform.SetParent(shellParent);//将创建出来的子弹放到shellParent下
        shellGo.GetComponent<Rigidbody>().velocity = firePos.forward * shellSpeed;
        shellGo.GetComponent<Shell>().isEnemyShell = true;//说明是敌人发的子弹
    }

    void SetFireState()
    {
        isFire = false;
    }

}
