using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviourPun
{
    public GameObject bulletPrefab;
    public Transform weaponLocation;
    public float speed = 10;
    CharacterController characterController;
    MenuController menuController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        menuController = GameObject.Find("Controllers").GetComponent<MenuController>();
    }

    void Update()
    {
        if(!photonView.IsMine && PhotonNetwork.IsConnected)
        {
            return;
        }

        transform.Rotate(0, Input.GetAxis("Horizontal") * 180 * Time.deltaTime, 0);
        characterController.SimpleMove(transform.forward * Input.GetAxis("Vertical") * speed);

        if(Input.GetButtonDown("Fire1"))
        {
            GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, weaponLocation.position, Quaternion.identity);
            GameObject.Destroy(bullet, 4);
            bullet.GetComponent<Rigidbody>().AddForce(transform.forward * 30, ForceMode.VelocityChange);
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            menuController.Pause();
        }
    }
}