using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace gameProject
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static GameObject LocalPlayerInstance;

        [Tooltip("The current Health of our player")]
        public float health = 10.0f;

        [SerializeField]
        public GameObject PlayerUiPrefab;

        float maxHealth;
        public float Health => health / maxHealth;

        void Awake()
        {
            // #Important 
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized 
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
            }

            // #Critical 
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load. 
            DontDestroyOnLoad(this.gameObject);

            maxHealth = health;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(health);
            }
            else
            {
                health = (float)stream.ReceiveNext();
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (!photonView.IsMine) return;

            if (collision.collider.CompareTag("Bullet"))
            {
                health -= 1;

                if (health <= 0) GameManager.Instance.LeaveRoom();
            }
        }

        void Start()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

            if (PlayerUiPrefab != null)
            {
                GameObject _uiGo = Instantiate(PlayerUiPrefab);
                _uiGo.GetComponent<PlayerUI>().SetTarget(this);
            }
            else
            {
                Debug.LogWarning("<Color=Red><a>Missing</a></Color> PlayerUiPrefab reference on player Prefab.", this);
            }
        }

        void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }
        }

        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }

        void CalledOnLevelWasLoaded(int level)
        {
            GameObject _uiGo = Instantiate(PlayerUiPrefab);
            _uiGo.GetComponent<PlayerUI>().SetTarget(this);
        }

        public override void OnDisable()
        {
            // Always call the base to remove callbacks 
            base.OnDisable();

            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}