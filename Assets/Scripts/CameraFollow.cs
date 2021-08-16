using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CameraFollow : MonoBehaviourPun
{
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    Transform cameraTransform;

    bool follow = false;

    void Start()
    {
        follow = (photonView.IsMine || !PhotonNetwork.IsConnected);
        cameraTransform = GameObject.Find("GameCamera").transform;
    }

    void FixedUpdate()
    {
        if (!follow) return;

        Vector3 targetPosition = target.position + offset;
        Vector3 direction = target.forward;
        direction = direction.normalized * 5;
        float y = targetPosition.y;
        targetPosition = targetPosition - direction;
        targetPosition.y = y;
        Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, targetPosition, smoothSpeed * Time.deltaTime);
        
        cameraTransform.position = smoothedPosition;

        cameraTransform.LookAt(target);
    }
}
