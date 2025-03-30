using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Avoidance
{

    public class PlayerController : MonoBehaviour
    {

        public bool hasReachedDestination = false;
        public bool stopWriting = false;
        public bool isRecording = false;
        public GameObject Manager;

        public DataManager DM;
        public GameManager GM;
        public ETRecorder ET;

        public GameController GC;
        //private GameManager EM;

        public TextMeshProUGUI message;

        public float startTime;
        public float endTime;
        public float currentSpeed;

        public Vector3 prevTrans = new Vector3(0, 0, 0);
        private float previousTime;

        public List<double> speeds = null;
        public List<float> times = null;
        public List<Vector3> positions = null;
        public List<float> averageDistances = null;
        public float time = 0;

        public bool eventTriggered = false;
        public GameObject screenBlanker;

        // public GameObject TimeCol;
        // public GameObject TimeCol1;
        // public string prevTimeCol; 

        //public Rigidbody rb;
        //Rigidbody m_Rigidbody;
        float m_Speed;

        public Vector3 forwardDir;


        // Start is called before the first frame update
        void Start()
        {
            //previousTime = 0;
            //rb = GetComponent<Rigidbody>();
            DM = GameObject.FindObjectOfType<DataManager>();
            GC = GameObject.FindObjectOfType<GameController>();
            ET = GameObject.FindObjectOfType<ETRecorder>();

            ToggleScreen(false);
            //EM = Manager.GetComponent<EventManager>();
            //Fetch the Rigidbody component you attach from your GameObject
            //m_Rigidbody = GetComponent<Rigidbody>();
            //Set the speed of the GameObject
            m_Speed = 5.0f;

        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyDown("space"))
            //{
            //    loadNextTest();
            //}
            // if (Input.GetKey(KeyCode.UpArrow))
            // {
            //     //Move the Rigidbody forwards constantly at speed you define (the blue arrow axis in Scene view)
            //     m_Rigidbody.velocity = transform.forward * m_Speed;
            // }

            // if (Input.GetKey(KeyCode.DownArrow))
            // {
            //     //Move the Rigidbody backwards constantly at the speed you define (the blue arrow axis in Scene view)
            //     m_Rigidbody.velocity = -transform.forward * m_Speed;
            // }

            // if (Input.GetKey(KeyCode.RightArrow))
            // {
            //     //Rotate the sprite about the Y axis in the positive direction
            //     transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * m_Speed, Space.World);
            // }

            // if (Input.GetKey(KeyCode.LeftArrow))
            // {
            //     //Rotate the sprite about the Y axis in the negative direction
            //     transform.Rotate(new Vector3(0, -1, 0) * Time.deltaTime * m_Speed, Space.World);
            // }

        }

        void FixedUpdate()
        {
            isRecording = GC.isRecording;
            if (GC.isRecording)
            {
                message.text = "Walk";
                if (Vector3.Distance(this.transform.position, GC.endPoint.position) < 1.8)
                {
                    //Debug.Log("Reached Dest");

                    if (!stopWriting)
                    {
                        hasReachedDestination = true;
                    }
                }
                else
                {
                    //Debug.Log(Vector3.Distance(this.transform.position, GC.endPoint.position));
                    //Debug.Log(Vector3.Distance(this.transform.position, GC.endPoint.position));
                }
                if (hasReachedDestination)
                {
                    hasReachedDestination = false;
                    stopWriting = true;
                    GC.isRecording = false;
                    ToggleScreen(true);
                    DM.WriteData();
                    ToggleScreen(false);
                }
                else
                {
                    //Debug.Log("Recording");
                    if (!stopWriting)
                    {
                        //Debug.Log("Write");
                        //if (Vector3.Distance(prevTrans, this.transform.position) > .05)
                        {
                            float x = this.transform.position.x;
                            float y = this.transform.position.y;
                            float z = this.transform.position.z;
                            Transform follow = this.transform;
                            if (GC.follower != null)
                            {
                                follow = GC.follower.transform;
                            }

                            DM.records.Add(new DataManager.Record(Time.time, this.transform.position, x, y, z, GC.midPoint, DM.models, follow, DM.outerDistance, ET.GetEyeData(), eventTriggered));
                            prevTrans = this.transform.position;
                        }


                    }

                }
            }
            else
            {
                message.text = "Stop";
            }

        }

        void ToggleScreen(bool mode)
        {
            if (screenBlanker != null)
            {
                screenBlanker.gameObject.SetActive(mode);
            }
        }


        public void WriteData()
        {
            print("writeData");
            DM.AddSpeeds(speeds);
            DM.AddTimes(times);
            DM.AddTime(time);
            DM.AddPositions(positions);
            DM.AddInterPersonalDistance(averageDistances);
            DM.WriteData();
        }

        public float GetAverageDistances()
        {
            List<float> distances = new List<float>();
            // foreach ( GameObject person in DM.crowdController.crowd)
            // {
            //     if (person.GetComponent<Person>().state == Person.State.Crossing)
            //     {
            //         Vector3 directionToTarget = transform.position - person.transform.position;
            //         float angle = Vector3.Angle(transform.forward, directionToTarget);
            //         if (Mathf.Abs(angle) < 90)
            //             Debug.Log("target is behind me");
            //         else
            //         {
            //             distances.Add(Vector3.Distance(transform.position, person.transform.position));
            //         }
            //     }


            // }
            distances.Sort();
            float shortestDistancesTotal = 0;
            if (distances.Count > 0)
            {
                for (int i = 0; i < 4 && i < distances.Count; i++)
                {
                    shortestDistancesTotal += distances[i];
                }
                float averageShortestDistances = shortestDistancesTotal / 4;
                //print(averageShortestDistances);
                return averageShortestDistances;
            }
            else
            {
                return 0;
            }


            // Bit shift the index of the layer (8) to get a bit mask
            //int layerMask = 1 << 8;

            //// This would cast rays only against colliders in layer 8.
            //// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            //layerMask = ~layerMask;


            //Vector3 boxSize = new Vector3(2, 2, 5);

            //Vector3 newCenter = transform.position + (-transform.forward) * boxSize.z ;

            //RaycastHit[] hit = Physics.BoxCastAll(transform.position, boxSize, 
            //                        -transform.forward, Quaternion.identity,
            //                        15f, layerMask, QueryTriggerInteraction.UseGlobal);

            //Vector3[] points = CubePoints(newCenter, boxSize, Quaternion.identity);
            //DrawCubePoints(points);

            //// Does the ray intersect any objects excluding the player layer
            //if (hit.Length >0)
            //{
            //    //.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //    //Debug.Log("Did Hit");

            //    foreach(RaycastHit h in hit)
            //    {
            //        print(hit.Length);
            //    }
            //}
            //else
            //{
            //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //    Debug.Log("Did not Hit");
            //}

            return 0;
        }

    }

}