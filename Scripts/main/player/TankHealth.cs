using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour {

    public AudioSource audio;
    public float hp = 100;
    public GameObject TankExplsPfb;
    public AudioClip TankExplsAudio;
    public Slider hpSlider;
    private Transform pfbParent;
    private float hpTotal;

    // Use this for initialization
    void Start () {
        hpTotal = hp;
        pfbParent = GameObject.FindGameObjectWithTag("PrefabParent").transform;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void TakeDamage()
    {
        if (hp <= 0) return;
        hp -= Random.Range(10, 20);
        hpSlider.value = hp / hpTotal; 
        if (hp <= 0)
        {
            if (transform.tag == "Player")
            {
                GameManager.Instance.PauseGame();
            }
            //处理重生
            EnemyManager.Instance.GenEnemy();
            //处理死亡
            AudioSource.PlayClipAtPoint(TankExplsAudio, transform.position);//播放音爆炸音效
            GameObject go = GameObject.Instantiate(TankExplsPfb, transform.position + Vector3.up, transform.rotation);
            go.transform.SetParent(pfbParent);
            Destroy(go, 1.1f);
            Destroy(gameObject);
            
        }
    }

}
