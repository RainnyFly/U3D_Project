using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public GameObject[] enemys;
    private Dictionary<int, GameObject> enemysDic = new Dictionary<int, GameObject>();
    private int enemyCnt = 3;
    private static EnemyManager instance;
    public static EnemyManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        InitData();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitData()
    {
        for (int i= 0; i < enemyCnt; i++)
        {
            InitanciaEnemy(i);
            //隐藏模板坦克
            enemys[i].SetActive(false);
        }

    }

    public void GenEnemy()
    {
        Invoke("WaitGenEnemy", 10f);
    }

    void WaitGenEnemy()
    {
        for (int i = 0; i < enemyCnt; i++)
        {
            if (enemysDic[i] == null)
            {
                enemysDic.Remove(i);
                InitanciaEnemy(i);
            }
        }
    }

    void InitanciaEnemy(int index)
    {
        GameObject go = Instantiate(enemys[index], enemys[index].transform.position, enemys[index].transform.rotation);
        go.transform.SetParent(transform);
        go.SetActive(true);
        go.name = "Enemy" + (index + 1);
        enemysDic.Add(index, go);
    }

}
