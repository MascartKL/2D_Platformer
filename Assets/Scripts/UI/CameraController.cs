using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform player;
    private Vector3 pos;

    //[SerializeField]
   public  static float leftLimit;
   // [SerializeField]
   public  static float rightLimit;
    [SerializeField]
    float downLimit;
    [SerializeField]
    float upLimit;
    
    private void Awake()
    {
        if (!player)
            player = FindObjectOfType<Player>().transform;       
    }
    void Update()
    {
        pos = player.position;
        pos.z = -10f;
        pos.y = 0;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 10);

        //////////////////////////
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, leftLimit, rightLimit), Mathf.Clamp(transform.position.y, downLimit, upLimit), transform.position.z);
        //ограничение камеры для создания отдельных закрытых комнат на сцене
    }
}
