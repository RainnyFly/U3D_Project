using UnityEngine;

public class CameraFollow : MonoBehaviour {

    //相机距离人物高度
    float m_Height = 10.5f;
    //相机距离人物距离
    float m_Distance = 14.5f;
    //相机跟随速度
    float m_Speed = 1.3f;
    //目标位置
    Vector3 m_TargetPosition;
    //要跟随的人物
    Transform PlayerTf;
    
    // Use this for initialization
    void Start ()
    {
        PlayerTf = GameObject.FindWithTag("Player").transform;
    }
	
	void LateUpdate ()
    {
        if (PlayerTf == null)
            return;
        //得到这个目标位置
        m_TargetPosition = PlayerTf.position + Vector3.up * m_Height - PlayerTf.forward * m_Distance;
        //相机位置
        transform.position = Vector3.Lerp(transform.position, m_TargetPosition, m_Speed * Time.deltaTime);
        //相机时刻看着人物
        transform.LookAt(PlayerTf);
    }
}
