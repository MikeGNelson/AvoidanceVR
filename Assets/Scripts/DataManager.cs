using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using Photon.Pun;
using System;

public class DataManager : MonoBehaviourPunCallbacks
{

    //public List<int> answers = null;
    public List<double> speeds = null;
    public List<float> times = null;
    public List<Vector3> positions = null;
    public List<float> interPersonalDistance = null;
    public float time = 0;
    public TextMeshProUGUI conditionIndicator;

    public float outerDistance = 0;

    private string conditionPrompt = "Record this number in your post round survey:";

    public string conditionText = "Default";
    
    public int models = 0;

    public int UId;
    string path;
    string path1;

    public GameController gameController;



    


    public List<Record> records = new List<Record>();
    public struct Record
    {
        public float time;
        public float distance,x,y,z;
        public bool isFront;
        public bool isCenter;
        public bool isSide;
        public bool isLeft;
        public bool isOutside;
        public float distanceToCharacter, x_c, y_c, z_c;
        public bool isFront_C;
        public bool isCenter_C;
        public bool isSide_C;
        public bool isLeft_C;
        public Vector3 position;
        public float outerDistance;
        public bool eventTriggered;

        public int condition;

        //public float[] eyeData;
        //public string currentGazeObject;
        public DataPoint eyeData;

        public Record(float _time, Vector3 _position, float _x, float _y, float _z, Transform _midpoint, int _condition, Transform _c_position, float _outerDistance, DataPoint _eyeData, bool _eventTriggered)
        {
            Debug.Log("Add Data: " +  _time);
            condition = _condition;
            time = _time;
            position = _position;
            x = _x;
            y = _y;
            z = _z;
            distanceToCharacter = 0f;

            eventTriggered = _eventTriggered;
            
            x_c = _c_position.position.x;
            y_c = _c_position.position.y;
            z_c = _c_position.position.z;
            outerDistance = _outerDistance;
            if(condition == 8 || condition ==11)
            {
                outerDistance = 0.5f;
            }
            else if (condition == 9 || condition == 12)
            {
                outerDistance = 1;
            }
            else if (condition == 10 || condition == 13)
            {
                outerDistance = 1.85f;
            }

            //eyeData = _eyeData.eyeData;
            //currentGazeObject = _eyeData.currentGazeObject;

            eyeData = _eyeData;


            //isFront_C = false;
            //isCenter_C = false;
            //isSide_C = false;
            //isLeft_C =false;
            distance = Vector3.Distance(_position,_midpoint.position);
            distanceToCharacter = Vector3.Distance(_position, _c_position.position);

            Debug.Log(outerDistance);
            Debug.Log("Condition: " + condition);
            if (Mathf.Abs(_position.x) > outerDistance)
            {
                isOutside = true;
                Debug.Log("is Outside");
            }
            else
            {
                isOutside = false;
                Debug.Log("Not Outsuide");
            }

            Vector3 ITP = _midpoint.InverseTransformPoint(_position);
            if(ITP.x < -0.001)
            {
                isLeft = true;
                isCenter = false;
                Debug.Log("Left");
            }
            else if  (ITP.x > 0.001)
            {
                isLeft = false;
                isCenter = false;
                Debug.Log("Right");
            }
            else
            {
                isLeft = false;
                isCenter = true;
                Debug.Log("Center");
            }

            if(ITP.z < -0.1)
            {
                
                isFront = true;
                isSide = false;
                Debug.Log("Front");
            }
            else if  (ITP.z > 0.001)
            {
                isFront = false;
                isSide = false;
                Debug.Log("Back");
            }
            else
            {
                isFront = false;
                isSide = true;
                Debug.Log("Side");
            }

            Vector3 ITPC = _c_position.InverseTransformPoint(_position);
            if (ITPC.x < -0.001)
            {
                isLeft_C = true;
                isCenter_C = false;
                Debug.Log("Left");
            }
            else if (ITPC.x > 0.001)
            {
                isLeft_C = false;
                isCenter_C = false;
                Debug.Log("Right");
            }
            else
            {
                isLeft_C = false;
                isCenter_C = true;
                Debug.Log("Center");
            }

            if (ITPC.z < -0.1)
            {

                isFront_C = true;
                isSide_C = false;
                Debug.Log("Front");
            }
            else if (ITPC.z > 0.001)
            {
                isFront_C = false;
                isSide_C = false;
                Debug.Log("Back");
            }
            else
            {
                isFront_C = false;
                isSide_C = true;
                Debug.Log("Side");
            }

            

        }

    }


    public enum Conditons 
    {
        
        One_Default,
        Two_Toon,
        Three_Creepy,
        Four_Spooky,
        Five_Robot,

        Cylinder_Small_No_Follow, //5
        Cylinder_Medium_No_Follow,
        Cylinder_Large_No_Follow,

        Cylinder_Small_Follow_Slow, //8
        Cylinder_Medium_Follow_Slow,
        Cylinder_Large_Follow_Slow,

        Cylinder_Small_Follow_Medium, //11
        Cylinder_Medium_Follow_Medium,
        Cylinder_Large_Follow_Medium,

        Cylinder_Small_Follow_Fast, //14
        Cylinder_Medium_Follow_Fast,
        Cylinder_Large_Follow_Fast,


        Close_Idle, //17
        Middle_Idle,
        Far_Idle,

        Close_Minor, //8
        Middle_Minor,
        Far_Minor,

        Close_Moderate, //11
        Middle_Moderate,
        Far_Moderate,

        Close_Major, //14
        Middle_Major,
        Far_Major,

        Sneeze_None,
        Sneeze_Away,
        Sneeze_Towards



        //Follow_Slow,
        //Follow_Medium,
        //Follow_Fast

    };
    public Conditons conditions;



   


    public 

    // Start is called before the first frame update
    void Start()
    {
        if(UId <0)
        {
            path = "Assets/Results/test.txt";
        }
        else
        {
            Debug.Log("Set Path");
            path = "Assets/Results/" + DateTime.Now.ToFileTime() + ".csv";
        }

        

        //TODO
        // Send the data based on the conditon
        //SendCondition();
    }

    public void SendCondition()
    {
        string conditionText = conditionPrompt;
        float width = 0f;
        float speed = 0f;
        
        switch(conditions)
        {

            #region models

            // Study Styles
            case Conditons.One_Default:
                //Set model group
                
                models = 0;
                conditionText = conditionPrompt + " 1";
                break;
            case Conditons.Two_Toon:
                //Set model group
               
                models = 1;//2
                conditionText = conditionPrompt +  " 2";
                break;
            case Conditons.Three_Creepy:
                //Set model group
                
                models = 2;
                conditionText = conditionPrompt + " 3";
                break;
            case Conditons.Four_Spooky:
                //Set model group
               
                models = 3;
                conditionText = conditionPrompt + " 4";
                break;
            case Conditons.Five_Robot:
                //Set model group
                
                models = 4;
                conditionText = conditionPrompt + " 5";
                break;
            #endregion

            #region follow
            /// Study Cylinders
            //No follow
            case Conditons.Cylinder_Small_No_Follow:
                //Set model group

                models = 5;
                conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Cylinder_Medium_No_Follow:
                //Set model group

                models = 6;
                conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Cylinder_Large_No_Follow:
                //Set model group

                models = 7;
                conditionText = conditionPrompt + " 5";
                break;

            //Follow Slow
            case Conditons.Cylinder_Small_Follow_Slow:
                //Set model group

                models = 5;
                speed = 0.5f;
                gameController.currentCurve = gameController.smallCurve;
                conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Cylinder_Medium_Follow_Slow:
                //Set model group

                models = 6;
                speed = 0.5f;
                gameController.currentCurve = gameController.mediumCurve;
                conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Cylinder_Large_Follow_Slow:
                //Set model group

                models = 7;
                speed = 0.5f;
                gameController.currentCurve = gameController.largeCurve;
                conditionText = conditionPrompt + " 5";
                break;


            //Follow Medium
            case Conditons.Cylinder_Small_Follow_Medium:
                //Set model group

                models = 5;
                speed = 1.2f;
                gameController.currentCurve = gameController.smallCurve;
                conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Cylinder_Medium_Follow_Medium:
                //Set model group

                models = 6;
                speed = 1.2f;
                gameController.currentCurve = gameController.mediumCurve;
                conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Cylinder_Large_Follow_Medium:
                //Set model group

                models = 7;
                speed = 1.2f;
                gameController.currentCurve = gameController.largeCurve;
                conditionText = conditionPrompt + " 5";
                break;

            //Follow Fast
            case Conditons.Cylinder_Small_Follow_Fast:
                //Set model group

                models = 5;
                speed = 1.4f;
                gameController.currentCurve = gameController.smallCurve;
                conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Cylinder_Medium_Follow_Fast:
                //Set model group

                models = 6;
                speed = 1.4f;
                conditionText = conditionPrompt + " 5";
                gameController.currentCurve = gameController.mediumCurve;
                break;

            case Conditons.Cylinder_Large_Follow_Fast:
                //Set model group

                models = 7;
                speed = 1.4f;
                gameController.currentCurve = gameController.largeCurve;
                conditionText = conditionPrompt + " 5";
                break;
            #endregion

        /// Study Sneeze
        /// 
        #region proxemics

            // Idle Modes
            case Conditons.Close_Idle:
                //Set model group

                models = 11;
                outerDistance = .5f;
                //conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Middle_Idle:
                //Set model group

                models = 12;
                outerDistance = 1;
                //conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Far_Idle:
                //Set model group

                models = 13;
                outerDistance = 3.5f;
                //conditionText = conditionPrompt + " 5";
                break;


            // Minor arugement modes
            case Conditons.Close_Minor:
                //Set model group

                models = 8;
                outerDistance = .5f;
                //conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Middle_Minor:
                //Set model group

                models = 9;
                outerDistance = 1;
                //conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Far_Minor:
                //Set model group

                models = 10;
                outerDistance = 3.5f;
                //conditionText = conditionPrompt + " 5";
                break;


            // Moderate arugement modes
            case Conditons.Close_Moderate:
                //Set model group

                models = 8;
                outerDistance = .5f;
                //conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Middle_Moderate:
                //Set model group

                models = 9;
                outerDistance = 1;
                //conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Far_Moderate:
                //Set model group

                models = 10;
                outerDistance = 3.5f;
                //conditionText = conditionPrompt + " 5";
                break;


            // Major arugement modes
            case Conditons.Close_Major:
                //Set model group

                models = 8;
                outerDistance = .5f;
                //conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Middle_Major:
                //Set model group

                models = 9;
                outerDistance = 1;
                //conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Far_Major:
                //Set model group

                models = 10;
                outerDistance = 3.5f;
                //conditionText = conditionPrompt + " 5";
                break;

            #endregion

            #region sneeze
            case Conditons.Sneeze_None:
                //Set model group

                models = 14;
                conditionText = "Sneeze_None";

                //conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Sneeze_Away:
                //Set model group
                conditionText = "Sneeze_Away";
                models = 15;

                //conditionText = conditionPrompt + " 5";
                break;

            case Conditons.Sneeze_Towards:
                //Set model group

                models = 16;
                conditionText = "Sneeze_Towards";

                //conditionText = conditionPrompt + " 5";
                break;

                #endregion

        }
        int mode = models;
        conditionIndicator.text = conditionText;

        photonView.RPC("UpdatePromptText", RpcTarget.All, conditionText, mode);
        gameController.GetConditions(models, width, speed);
    }

   


    /// <summary>
    /// Write the data when reaching the last node
    /// </summary>
    public void WriteData()
    {
        //string rep = " A";
        //path = "Assets/Results/" + (records[0].condition + 1).ToString() + "A.csv";
        //path1 = "Assets/Results/" + (records[0].condition + 1).ToString() + "A_Summary.csv";
        //if (System.IO.File.Exists(path))
        //{
        //    path = "Assets/Results/" + (records[0].condition + 1).ToString() + "B.csv";
        //    path1 = "Assets/Results/" + (records[0].condition + 1).ToString() + "B_Summary.csv";
        //    rep = " B";
        //}

        switch (records[0].condition)
        {
            case 14:
                conditionText = "Sneeze_None";
                break;
            case 15:
                conditionText = "Sneeze_Away";
                break;
            case 16:
                conditionText = "Sneeze_Towards";
                break;

        }
        Debug.Log("Write Data");
        #if UNITY_EDITOR
            path  = Application.dataPath + "/" + conditionText + "_" + ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString() + "_Raw.csv";
            path1  = Application.dataPath + "/" + conditionText + "_" + ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString() + "_Summary.csv";
#else
            path  = Application.persistentDataPath  + "/" + conditionText + "_" + ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString() + "_Raw.csv";
            path1  = Application.persistentDataPath  + "/" + conditionText + "_" + ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds().ToString() + "_Summary.csv";
#endif

        float sumDistance = 0;
        float totalTime = 0;
        float startTime = records[0].time;
        float averageSpeed = 0;
        float minimumDistance = 100;

        List<float> passingDistances = new List<float>();
        float totalPassing = 0;
        float passingDistance = 100;

        bool isMinDistSide = false;
        string isMinDistanceFrontOrBack = "";
        string passingSide = "";
        string outer = "";

        

        int tIndex = 0;
        int maxTIndex = records.Count - 1;

        StreamWriter writer = new StreamWriter(path, true);

        

        int i =0;
        Debug.Log("--------------------------------");
        Debug.Log("Condition: " + models);
        Debug.Log("--------------------------------");
        //writer.WriteLine("--------------------------------");
        //writer.WriteLine("Condition: " + (records[0].condition +1).ToString() + rep);
        //writer.WriteLine("--------------------------------");
        writer.WriteLine("Condition, Index, Time, Distance, X, Y, Z, isCenter, isFront, isSide, isLeft, isOuter,GazeObject,GP_X,GP_X,GP_Z,GZ_X,GZ_Y,GZ_Y,GZD_X,GZD_Y,GZD_Z,RE_X,RE_Y,RE_Z,RED_X,RED_Y,RED_Z,LE_X,LE_Y,LE_Z,LED_X,LED_Y,LED_Z,H_X,H_Y,H_Z,HR_X,HR_Y,HR_Z,blinks");


        // Write everything from the array.

       
        foreach (Record record in records)
        {

            if (tIndex != maxTIndex)
            {
                sumDistance += Vector3.Distance(record.position, records[tIndex + 1].position);
            }
            else
            {
                totalTime = record.time;
            }

            if (minimumDistance > record.distance)
            {
                minimumDistance = record.distance;
                if (record.isSide)
                {
                    isMinDistSide = true;
                    if (record.isOutside)
                    {
                        outer = "Outside";
                    }
                    else
                    {
                        outer = "Inside";
                    }
                }
                else
                {
                    isMinDistSide = false;
                    if (record.isFront)
                    {
                        isMinDistanceFrontOrBack = "Front";
                    }
                    else
                    {
                        isMinDistanceFrontOrBack = "Back";
                    }

                }
            }

            if (record.isSide)
            {
                passingDistances.Add(record.distance);
                totalPassing += record.distance;
                if (record.isLeft)
                {
                    passingSide = "Left";
                }
                else
                {
                    passingSide = "Right";
                }

                if (record.isOutside)
                {
                    outer = "Outside";
                }
                else
                {
                    outer = "Inside";
                }
            }

            Debug.Log("--------------------------------");
            Debug.Log("Index: " + i);
            Debug.Log("--------------------------------");
            Debug.Log("Time: " +record.time);
            Debug.Log("Distance: " + record.distance);
            Debug.Log("position: " + record.position);
            Debug.Log("isCenter: " + record.isCenter);
            Debug.Log("isFront: " + record.isFront);
            Debug.Log("isSide: " + record.isSide);
            Debug.Log("isLeft: " + record.isLeft);
            Debug.Log("isOutside: " + record.isOutside);

            //writer.WriteLine("--------------------------------");
            //writer.WriteLine("Index: " + i);
            //writer.WriteLine("--------------------------------");
            //writer.WriteLine("Time: " + record.time);
            //writer.WriteLine("Distance: " + record.distance);
            //writer.WriteLine("position: " + record.position);
            //writer.WriteLine("isCenter: " + record.isCenter);
            //writer.WriteLine("isFront: " + record.isFront);
            //writer.WriteLine("isSide: " + record.isSide);
            //writer.WriteLine("isLeft: " + record.isLeft);

            writer.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11},{12}"
                , conditionText, i, record.time, record.distance, record.x, record.y, record.z, record.isCenter, record.isFront, record.isSide, record.isLeft, record.isOutside, record.eyeData.JoinValues()));

            
            

            tIndex++;
            i++;
        }
        

        writer.Close();

        //Summary
        Debug.Log("write sum");

        passingDistance = totalPassing / (passingDistances.Count-1);
        averageSpeed = sumDistance / totalTime;
        if(isMinDistSide)
        {
            isMinDistanceFrontOrBack = "";
        }

        totalTime -= startTime;

        StreamWriter sw1 = new StreamWriter(path1);
        sw1.WriteLine("Condition, Total Distance, Total Time, Average Speed, Minimum Distance, Passing Distance, Is Minimum Distance Side, Is Minimum Distance Front or Back, Passing Side, Inside or Outside");
        sw1.WriteLine(string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}",
            conditionText,
            sumDistance.ToString(),
            totalTime.ToString(),
            averageSpeed.ToString(),
            minimumDistance.ToString(),
            passingDistance.ToString(),
            isMinDistSide,
            isMinDistanceFrontOrBack,
            passingSide,
            outer
            ));

        sw1.Close();


        records.Clear();

    }


    [PunRPC]
    void UpdatePromptText(string text, int mode)
    {
        Debug.Log(text);
        gameController.isRecording = true;
        models = mode;
        //path = "Assets/Results/" + DateTime.Now.ToFileTime() + ".txt";
        conditionIndicator.text = text;
    }


    //public void AddAnswers(List<int> a)
    //{
    //    if (a != null)
    //    {
    //        if (a.Count > 0)
    //        {
    //            answers.AddRange(a);
    //        }
    //    }

    //}


    /// <summary>
    /// Add speeds
    /// </summary>
    /// <param name="s"></param>
    public void AddSpeeds(List<double> s)
    {
        if (s != null)
        {
            Debug.Log("Not Null");
            if (s.Count > 0)
            {
                Debug.Log("Not zero");
                speeds.AddRange(s);
            }

        }

    }

    /// <summary>
    /// Add the times
    /// </summary>
    /// <param name="t"></param>
    public void AddTimes(List<float> t)
    {

        if (t != null)
        {
            if (t.Count > 0)
            {
                times.AddRange(t);

            }
        }

    }

    /// <summary>
    /// Add the average interpersonal distances
    /// </summary>
    /// <param name="d"></param>
    public void AddInterPersonalDistance(List<float> d)
    {

        if (d != null)
        {
            if (d.Count > 0)
            {
                interPersonalDistance.AddRange(d);

            }
        }

    }

    /// <summary>
    /// Add Positions
    /// </summary>
    /// <param name="p"></param>
    public void AddPositions(List<Vector3> p)
    {

        if (p != null)
        {
            if (p.Count > 0)
            {
                positions.AddRange(p);

            }
        }

    }

    /// <summary>
    /// Add Time
    /// </summary>
    /// <param name="t"></param>
    public void AddTime(float t)
    {

        time = t;

    }
}
