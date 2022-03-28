using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon;
using Photon.Pun;
using Photon.Realtime;


namespace Bogey
{

    public class GolfBallPlayerController : MonoBehaviourPunCallbacks, IPunObservable
    {
        // Start is called before the first frame update

        [Header("Movement Variables")]
        [Range(10, 200)] [SerializeField] private float shotPower = 100.0f;
        [SerializeField] private float shotPowerMultiplier = 1.0f;
        [Range(10, 200)] [SerializeField] private float shotPowerMin;
        [Range(10, 200)] [SerializeField] private float shotPowerMax;

        [Header("Physics")]
        [SerializeField] private Rigidbody ball;
        [SerializeField] private float ballVelocity;
        [SerializeField] public bool isMoving;

        [SerializeField] Transform playerInputSpace = default;

        private Vector3 lastPosition = default;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        [SerializeField] public static GameObject LocalPlayerInstance;


        private void Awake()
        {
            if(photonView.IsMine)
            {
                GolfBallPlayerController.LocalPlayerInstance = this.gameObject;
            }

            DontDestroyOnLoad(LocalPlayerInstance);
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if(photonView.IsMine)
            {
                ballVelocity = ball.velocity.magnitude;

                if (ballVelocity < 0.2)
                {
                    //stop ball movement
                    ball.velocity = Vector3.zero;
                    isMoving = false;
                }

                else
                    isMoving = true;

                ControlBall();
            }
 
        }

        #region Ball Controls
        public void ControlBall()
        {
            

            if(Input.GetMouseButtonDown(0) && !isMoving)
            {
                Vector3 force = Vector3.zero;
                if (playerInputSpace)
                {

                    Vector3 forward = playerInputSpace.forward;
                    forward.y = 0f;
                    forward.Normalize();

                    force = (forward) * shotPower * shotPowerMultiplier;
                }
                else
                {
                    force = transform.forward * shotPower * shotPowerMultiplier;
                }

                lastPosition = transform.position;
                ball.AddForce(force, ForceMode.Acceleration);
                
            }

        }
        #endregion

        #region IPunObservable implementation


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(shotPower);
                stream.SendNext(shotPowerMultiplier);
                stream.SendNext(ballVelocity);
                stream.SendNext(isMoving);
            }
            else
            {
                shotPower = (float)stream.ReceiveNext();
                shotPowerMultiplier = (float)stream.ReceiveNext();
                ballVelocity = (float)stream.ReceiveNext();
                isMoving = (bool)stream.ReceiveNext();
            }
        }


        #endregion
    }
}