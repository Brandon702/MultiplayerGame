using System;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        //private InputSystem playerInput;
        //private Rigidbody2D rb;
        //public Animator animator;
        SpriteRenderer spriteRenderer;
        private MenuController menuController;
        //public PlayerController player;
        public float speed = 5f;
        private Vector2 moving;
        //public GameObject text;

        CharacterController characterController;


        void Awake()
        {
            characterController = GetComponent<CharacterController>();
            //playerInput = new InputSystem();
            //rb = GetComponent<Rigidbody2D>();
            //spriteRenderer = GetComponent<SpriteRenderer>();
            //player = GetComponent<PlayerController>();
            //menuController = GameObject.Find("MenuController").GetComponent<MenuController>();
            //text.SetActive(false);
        }

        void FixedUpdate()
        {
            //if (GameController.Instance.state == eState.GAME)
            {
                //Vector2 moveInput = playerInput.Player.Move.ReadValue<Vector2>();
                //rb.velocity = moveInput * speed;
                //Debug.Log(moving);

                characterController.Move(speed * moving);
                //rb.velocity = speed * moving;

                //animator.SetFloat("Speed", characterController.velocity.magnitude);

                //if (rb.velocity.x > 0) spriteRenderer.flipX = false;
                //if (rb.velocity.x < 0) spriteRenderer.flipX = true;
            }

        }

        private void Update()
        {
            //if (GameController.Instance.state == eState.GAME)
            {
                if (Input.GetKeyDown(KeyCode.W))
                {
                    moving = new Vector3(1, 0, 0);
                }

                if (Input.GetKeyDown(KeyCode.S))
                {
                    moving = new Vector3(-1, 0, 0);
                }

                if(Input.GetKeyUp(KeyCode.D))
                {
                    moving = new Vector3(0, 0, 1);
                }

                if (Input.GetKeyUp(KeyCode.A))
                {
                    moving = new Vector3(0, 0,-1);
                }

                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
                {
                    moving = new Vector3(0, 0);
                    menuController.Pause();
                }
            }
            //else
            //{
            //    moving = new Vector2(0, 0);
            //}
        }
    }
}
