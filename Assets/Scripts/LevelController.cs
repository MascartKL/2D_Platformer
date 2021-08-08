using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{                                                       //  не доделано
    public delegate void LevelEnDelegate(int level);

    public static event LevelEnDelegate LevelEnd;

    private GameObject player;
    [SerializeField] private int arenaMax;
    [SerializeField] private int[] numMobOfWave;
    public GameObject borders;

    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    [SerializeField] private string[] isProgressStage = new string [3];
    [SerializeField] private Transform[] arenaBord;
    [SerializeField] private GameObject[] mobGroundPref;
    [SerializeField] private GameObject[] mobAirPref;
    [SerializeField] private GameObject leftEdge;
    [SerializeField] private GameObject rightEdge;

    [Header("1 Stage")]
    [SerializeField] private Vector3[] spawnTranAir1;
    [SerializeField] private Vector3[] spawnTranGround1;

    [SerializeField] private int numGroundMob1_1;
    [SerializeField] private GameObject[] prefabMobGr1_1;

    [SerializeField] private int numGroundMob1_2;
    [SerializeField] private GameObject[] prefabMobGr1_2;

    [SerializeField] private int numGroundMob1_3;
    [SerializeField] private GameObject[] prefabMobGr1_3;

    [SerializeField] private int numAirMob1_1;
    [SerializeField] private GameObject[] prefabMobAir1_1;

    [SerializeField] private int numAirMob1_2;
    [SerializeField] private GameObject[] prefabMobAir1_2;

    [SerializeField] private int numAirMob1_3;
    [SerializeField] private GameObject[] prefabMobAir1_3;

    [Header("2 Stage")]
    [SerializeField] private Vector3[] spawnTranAir2;
    [SerializeField] private Vector3[] spawnTranGround2;

    [SerializeField] private int numGroundMob2_1;
    [SerializeField] private GameObject[] prefabMobGr2_1;


    [SerializeField] private int numGroundMob2_2;
    [SerializeField] private GameObject[] prefabMobGr2_2;

    [SerializeField] private int numGroundMob2_3;
    [SerializeField] private GameObject[] prefabMobGr2_3;

    [SerializeField] private int numAirMob2_1;
    [SerializeField] private GameObject[] prefabMobAir2_1;

    [SerializeField] private int numAirMob2_2;
    [SerializeField] private GameObject[] prefabMobAir2_2;

    [SerializeField] private int numAirMob2_3;
    [SerializeField] private GameObject[] prefabMobAir2_3;

    [Header("3 Stage")]
    [SerializeField] private Vector3[] spawnTranAir3;
    [SerializeField] private Vector3[] spawnTranGround3;

    [SerializeField] private int numGroundMob3_1;
    [SerializeField] private GameObject[] prefabMobGr3_1;


    [SerializeField] private int numGroundMob3_2;
    [SerializeField] private GameObject[] prefabMobGr3_2;

    [SerializeField] private int numGroundMob3_3;
    [SerializeField] private GameObject[] prefabMobGr3_3;

    [SerializeField] private int numAirMob3_1;
    [SerializeField] private GameObject[] prefabMobAir3_1;

    [SerializeField] private int numAirMob3_2;
    [SerializeField] private GameObject[] prefabMobAir3_2;

    [SerializeField] private int numAirMob3_3;
    [SerializeField] private GameObject[] prefabMobAir3_3;

    [Header("4 Stage")]
    [SerializeField] private Vector3[] spawnTranAir4;
    [SerializeField] private Vector3[] spawnTranGround4;

    [SerializeField] private int numGroundMob4_1;
    [SerializeField] private GameObject[] prefabMobGr4_1;


    [SerializeField] private int numGroundMob4_2;
    [SerializeField] private GameObject[] prefabMobGr4_2;

    [SerializeField] private int numGroundMob4_3;
    [SerializeField] private GameObject[] prefabMobGr4_3;

    [SerializeField] private int numAirMob4_1;
    [SerializeField] private GameObject[] prefabMobAir4_1;

    [SerializeField] private int numAirMob4_2;
    [SerializeField] private GameObject[] prefabMobAir4_2;

    [SerializeField] private int numAirMob4_3;
    [SerializeField] private GameObject[] prefabMobAir4_3;

    void Start()
    {
        
       
        CameraController.leftLimit = leftEdge.transform.position.x + 5f;
        CameraController.rightLimit = rightEdge.transform.position.x;

        DamageableObj.Death += Death;
        StartCoroutine("BorderUp");

        for (int i = 0; i < 4; i++)
        {
            isProgressStage[i] = "not started";
        }
    }

    void Death(GameObject go)
    {
        enemies.RemoveAt(enemies.IndexOf(go));        
    }

    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            LevelEnd(1);
        }
        player = GameObject.FindWithTag("Player");

        if (isProgressStage[0] == "not started" && isProgressStage[1] == "not started" && isProgressStage[2]  == "not started" && isProgressStage[3] == "not started")
        {
            BorderUp();
        }
        else if (isProgressStage[0] == "started" || isProgressStage[1] == "started" || isProgressStage[2] == "started" || isProgressStage[2] == "started")
        {
            BorderDown();
            Debug.Log("down");
        }
        else if((isProgressStage[0] == "ended" && isProgressStage[1] == "not started" )|| (isProgressStage[1] == "ended" && isProgressStage[2] == "not started" && arenaMax > 2) || (isProgressStage[2] == "ended" && isProgressStage[3] == "not started" && arenaMax > 3))
        {
            BorderUp();
        }


        if(player.transform.position.x > arenaBord[0].position.x+8.5f && isProgressStage[0] == "started")
        {
            CameraController.leftLimit = arenaBord[0].position.x + 8f;
            CameraController.rightLimit = arenaBord[1].position.x - 8f;
        }else if (player.transform.position.x > arenaBord[1].position.x + 8.5f && isProgressStage[1] == "started")
        {
            CameraController.leftLimit = arenaBord[1].position.x + 8f;
            CameraController.rightLimit = arenaBord[2].position.x - 8f;
        }else if (player.transform.position.x > arenaBord[2].position.x + 8.5f && isProgressStage[2] == "started")
        {
            CameraController.leftLimit = arenaBord[2].position.x + 8f;
            CameraController.rightLimit = arenaBord[3].position.x - 8f;
        }else if (player.transform.position.x > arenaBord[3].position.x + 8.5f && isProgressStage[3] == "started")
        {
            CameraController.leftLimit = arenaBord[3].position.x + 8f;
            CameraController.rightLimit = rightEdge.transform.position.x - 8f;
        }

        if (player.transform.position.x > arenaBord[0].position.x && player.transform.position.x < arenaBord[1].position.x && isProgressStage[0] == "not started")
        {
            isProgressStage[0] = "started";
            StartCoroutine("Stage1");
        }
        else if(player.transform.position.x > arenaBord[1].position.x && player.transform.position.x < arenaBord[2].position.x && isProgressStage[1] == "not started")
        {
            isProgressStage[1] = "started";
            StartCoroutine("Stage2");
        }
        else if(player.transform.position.x > arenaBord[2].position.x && player.transform.position.x < arenaBord[3].position.x && isProgressStage[2] == "not started" && arenaMax>2)
        {
            isProgressStage[2] = "started";
            StartCoroutine("Stage3");
        }
        else if (player.transform.position.x > arenaBord[3].position.x && isProgressStage[2] == "not started" && arenaMax > 3)
        {
            isProgressStage[3] = "started";
            StartCoroutine("Stage4");
        }
    }

    private void BorderDown()
    {
        borders.transform.position = Vector3.MoveTowards(borders.transform.position, new Vector3(0, 0, 0), 5f * Time.deltaTime);
    }
    private void BorderUp()
    {
        borders.transform.position = Vector3.MoveTowards(borders.transform.position, new Vector3(0, 5f, 0), 5f * Time.deltaTime);

    }


    private IEnumerator Stage1()
    {

        for (int i = 0; i < numGroundMob1_1; i++)
        {
            enemies.Add(Instantiate(prefabMobGr1_1[i], spawnTranGround1[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), 0, 0), Quaternion.identity)); 
                                                                                // (i%2) для того чтобы мобы спавнились поочередно
        }                                                                      // то в одной точке, то в другой, коих всего 2 штуки(обычно)
                                                                               //если будет больше, придется менять

        for (int i = 0; i < numAirMob1_1; i++)
        {
            enemies.Add(Instantiate(prefabMobAir1_1[i], spawnTranAir1[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), Random.Range(-1f,1f), 0), Quaternion.identity)); 
        }
///////////////////////////////////////////////////////////////
        yield return new WaitUntil(() => enemies.Count == 0);
///////////////////////////////////////////////////////////////
        for (int i = 0; i < numGroundMob1_2; i++)
        {
            enemies.Add(Instantiate(prefabMobGr1_2[i], spawnTranGround1[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), 0, 0), Quaternion.identity));
        }

        for (int i = 0; i < numAirMob1_2; i++)
        {
            enemies.Add(Instantiate(prefabMobAir1_2[i], spawnTranAir1[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 1f), 0), Quaternion.identity));
        }

///////////////////////////////////////////////////////////////
        yield return new WaitUntil(() => enemies.Count == 0);
///////////////////////////////////////////////////////////////
        for (int i = 0; i < numGroundMob1_3; i++)
        {
            enemies.Add(Instantiate(prefabMobGr1_3[i], spawnTranGround1[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), 0, 0), Quaternion.identity));
        }

        for (int i = 0; i < numAirMob1_3; i++)
        {
            enemies.Add(Instantiate(prefabMobAir1_3[i], spawnTranAir1[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 1f), 0), Quaternion.identity));
        }
        ///////////////////////////////////////////////////////////////
        yield return new WaitUntil(() => enemies.Count == 0);
        ///////////////////////////////////////////////////////////////
        isProgressStage[0] = "ended";
        CameraController.rightLimit = arenaBord[2].position.x - 8f;
        yield return null;
    }

    private IEnumerator Stage2()
    {
        Debug.Log("stage 2");
        for (int i = 0; i < numGroundMob2_1; i++)
        {
            enemies.Add(Instantiate(prefabMobGr2_1[i], spawnTranGround2[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), 0, 0), Quaternion.identity));
        }

        for (int i = 0; i < numAirMob2_1; i++)
        {
            enemies.Add(Instantiate(prefabMobAir2_1[i], spawnTranAir2[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 1f), 0), Quaternion.identity));
        }
        ///////////////////////////////////////////////////////////////
        yield return new WaitUntil(() => enemies.Count == 0);
        ///////////////////////////////////////////////////////////////
        for (int i = 0; i < numGroundMob2_2; i++)
        {
            enemies.Add(Instantiate(prefabMobGr2_2[i], spawnTranGround2[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), 0, 0), Quaternion.identity));
        }

        for (int i = 0; i < numAirMob2_2; i++)
        {
            enemies.Add(Instantiate(prefabMobAir2_2[i], spawnTranAir2[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 1f), 0), Quaternion.identity));
        }

        ///////////////////////////////////////////////////////////////
        yield return new WaitUntil(() => enemies.Count == 0);
        ///////////////////////////////////////////////////////////////
        for (int i = 0; i < numGroundMob2_3; i++)
        {
            enemies.Add(Instantiate(prefabMobGr2_3[i], spawnTranGround2[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), 0, 0), Quaternion.identity));
        }

        for (int i = 0; i < numAirMob2_3; i++)
        {
            enemies.Add(Instantiate(prefabMobAir2_3[i], spawnTranAir2[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 1f), 0), Quaternion.identity));
        }
        ///////////////////////////////////////////////////////////////
        yield return new WaitUntil(() => enemies.Count == 0);
        ///////////////////////////////////////////////////////////////
        isProgressStage[1] = "ended";
        if (arenaMax > 2)
        {
            CameraController.rightLimit = arenaBord[3].position.x - 8f;
        }
        yield return null;
    }

    private IEnumerator Stage3()
    {
        Debug.Log("stage 3");
        for (int i = 0; i < numGroundMob3_1; i++)
        {
            enemies.Add(Instantiate(prefabMobGr3_1[i], spawnTranGround3[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), 0, 0), Quaternion.identity));
        }

        for (int i = 0; i < numAirMob3_1; i++)
        {
            enemies.Add(Instantiate(prefabMobAir3_1[i], spawnTranAir3[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 1f), 0), Quaternion.identity));
        }
        ///////////////////////////////////////////////////////////////
        yield return new WaitUntil(() => enemies.Count == 0);
        ///////////////////////////////////////////////////////////////
        for (int i = 0; i < numGroundMob3_2; i++)
        {
            enemies.Add(Instantiate(prefabMobGr3_2[i], spawnTranGround3[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), 0, 0), Quaternion.identity));
        }

        for (int i = 0; i < numAirMob3_2; i++)
        {
            enemies.Add(Instantiate(prefabMobAir3_2[i], spawnTranAir3[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 1f), 0), Quaternion.identity));
        }

        ///////////////////////////////////////////////////////////////
        yield return new WaitUntil(() => enemies.Count == 0);
        ///////////////////////////////////////////////////////////////
        for (int i = 0; i < numGroundMob3_3; i++)
        {
            enemies.Add(Instantiate(prefabMobGr3_3[i], spawnTranGround3[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), 0, 0), Quaternion.identity));
        }

        for (int i = 0; i < numAirMob3_3; i++)
        {
            enemies.Add(Instantiate(prefabMobAir3_3[i], spawnTranAir3[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 1f), 0), Quaternion.identity));
        }
        ///////////////////////////////////////////////////////////////
        yield return new WaitUntil(() => enemies.Count == 0);
        ///////////////////////////////////////////////////////////////
        isProgressStage[2] = "ended";
        if (arenaMax > 3)
        {
            CameraController.rightLimit = rightEdge.transform.position.x - 8f;
        }
        yield return null;
    }

    private IEnumerator Stage4()
    {
        Debug.Log("stage 4");
        for (int i = 0; i < numGroundMob4_1; i++)
        {
            enemies.Add(Instantiate(prefabMobGr4_1[i], spawnTranGround4[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), 0, 0), Quaternion.identity));
        }

        for (int i = 0; i < numAirMob4_1; i++)
        {
            enemies.Add(Instantiate(prefabMobAir4_1[i], spawnTranAir4[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 1f), 0), Quaternion.identity));
        }


        ///////////////////////////////////////////////////////////////
        yield return new WaitUntil(() => enemies.Count == 0);
        ///////////////////////////////////////////////////////////////
        for (int i = 0; i < numGroundMob4_2; i++)
        {
            enemies.Add(Instantiate(prefabMobGr4_2[i], spawnTranGround4[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), 0, 0), Quaternion.identity));
        }

        for (int i = 0; i < numAirMob4_2; i++)
        {
            enemies.Add(Instantiate(prefabMobAir4_2[i], spawnTranAir4[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 1f), 0), Quaternion.identity));
        }

        ///////////////////////////////////////////////////////////////
        yield return new WaitUntil(() => enemies.Count == 0);
        ///////////////////////////////////////////////////////////////
        for (int i = 0; i < numGroundMob4_3; i++)
        {
            enemies.Add(Instantiate(prefabMobGr4_3[i], spawnTranGround4[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), 0, 0), Quaternion.identity));
        }

        for (int i = 0; i < numAirMob4_3; i++)
        {
            enemies.Add(Instantiate(prefabMobAir4_3[i], spawnTranAir4[((i + 1) % 2)] + new Vector3(Random.Range(-5f, 5f), Random.Range(-1f, 1f), 0), Quaternion.identity));
        }
        ///////////////////////////////////////////////////////////////
        yield return new WaitUntil(() => enemies.Count == 0);
        ///////////////////////////////////////////////////////////////
        isProgressStage[3] = "ended";
        yield return null;
    }
}
