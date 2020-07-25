using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    private GameObject loadGo;
    private Slider loadSlider;
    private Text pregrassTxt;
    private AsyncOperation asyncOpra;

    // Use this for initialization
    void Start () {
        loadGo = transform.Find("LoadGo").gameObject;
        loadGo.SetActive(false);
        loadSlider = loadGo.transform.Find("loadSlider").GetComponent<Slider>();
        pregrassTxt = loadSlider.transform.Find("pregrassTxt").GetComponent<Text>();

    }
	
	// Update is called once per frame
	void Update () {
        if ((asyncOpra != null) && !asyncOpra.isDone)
        {
            pregrassTxt.text = asyncOpra.progress  * 100 + "%";
        }
	}

    public void OnClickStart()
    {
        loadGo.SetActive(true);
        asyncOpra = SceneManager.LoadSceneAsync(1);
    }

}
