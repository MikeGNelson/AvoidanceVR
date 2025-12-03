using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Photon.Pun;
public class GameController : MonoBehaviourPunCallbacks
{
    // public List<Transform> startPoints;
    // public List<Transform> midPoints;
    // public List<Transform> endPoints;
    public List<GameObject> peoplePrefabs;

    public Transform midPoint;
    public Transform startPoint;
    public Transform endPoint;
    public Transform followPoint;
    public GameObject follower;

    public  List<GameObject> modelsList = new List<GameObject>();
    //public  

    public PathCreation.PathCreator smallCurve, mediumCurve, largeCurve;


    public PathCreation.PathCreator currentCurve;

    public NavMeshObstacle player;
   


    public bool setColor = false;

    public bool isRecording = false;

    public GameObject vrPrefab;
    public GameObject desktopPrefab;

    private GameObject spawnedPlayer = null;
    public Avoidance.PlayerController playerController;
    public GameObject vrRig;



    
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        //desktop
        // if(SessionManager.Instance.mode ==0)
        // {
        //     //if(spawnedPlayer !=null)
        //         spawnedPlayer =PhotonNetwork.Instantiate("Desktop Player",new Vector3(0,1,0),Quaternion.identity);
        //     vrRig.SetActive(false);
        // }
        // //Quest
        // else if(SessionManager.Instance.mode ==1)
        // {
        //     //if(spawnedPlayer !=null)
        //         spawnedPlayer = PhotonNetwork.Instantiate("VR Player",startPoint.position,Quaternion.identity);
        // }
        // //hololens
        // else
        // {

        // }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayer);
    }

    //public int destPoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        //desktop
        if(SessionManager.Instance.mode ==0)
        {
            //if(spawnedPlayer !=null)
                spawnedPlayer =PhotonNetwork.Instantiate("Desktop Player",new Vector3(0,1,0),Quaternion.identity);
            vrRig.SetActive(false);
        }
        //Quest
        else if(SessionManager.Instance.mode ==1)
        {
            //if(spawnedPlayer !=null)
                spawnedPlayer = PhotonNetwork.Instantiate("VR Player", vrRig.transform.position,Quaternion.identity);
        }
        //hololens
        else
        {

        }
        //foreach(GameObject person in crowd)
        //{
        //    person.destination = startPoints[0].position;
        //}
        //modelsPrefabs = peoplePrefabs;
        
    }


    public void GetConditions(int models, float width =0f, float speed = 0f)
    {
        foreach(GameObject model in modelsList)
        {
            PhotonNetwork.Destroy(model);
        }
        Clear();

        GameObject mod = PhotonNetwork.Instantiate(peoplePrefabs[models].name,midPoint.position, Quaternion.LookRotation(startPoint.position-midPoint.position));
        modelsList.Add(mod);

        GameObject follow;
        if (speed != 0)
        {
            follow = PhotonNetwork.Instantiate(peoplePrefabs[0].name, followPoint.position, Quaternion.LookRotation(midPoint.position - followPoint.position));
            follow.GetComponent<MoveAgent>().goal = endPoint;
            follow.GetComponent<MoveAgent>().speed = speed;
            follow.GetComponent<MoveAgent>().Play();
            follower = follow;
        }
        
        playerController = GameObject.FindObjectOfType<Avoidance.PlayerController>();
        Debug.Log("Start Recording");
        isRecording = true;
        // playerController.is
    }

    public void Clear()
    {
        modelsList.Clear();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

   

//    public Vector3 RandomSpawnPoint(int spawnPoint)
//     {
        
//         Vector3 pos = RandomCircle(spawnPoints[spawnPoint].position, spawnRadius);
//         return pos;
//     }

    // Vector3 RandomCircle(Vector3 center, float radius)
    // {
    //     float ang = Random.value * 360;
    //     Vector3 pos;
    //     pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
    //     pos.y = center.y;
    //     pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
    //     return pos;
    // }
}
