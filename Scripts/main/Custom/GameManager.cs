using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    private AudioSource audio;
    public bool GameRunning = true;

    private void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start () {
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PauseGame()
    {
        audio.Pause();
        Time.timeScale = 0;
        GameRunning = false;
    }

}
