using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;


public class Tracker : MonoBehaviour
{
    public Transform head;
    public Transform left;
    public Transform right;
    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            head.gameObject.SetActive(false);
            left.gameObject.SetActive(false);
            right.gameObject.SetActive(false);
            
            MapPostition(head, XRNode.Head);
            MapPostition(left, XRNode.LeftHand);
            MapPostition(right,XRNode.RightHand);
        }
        
    }

    void MapPostition(Transform target, XRNode node)
    {
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);
        
        target.position = position;
        target.rotation = rotation;
    }
}
