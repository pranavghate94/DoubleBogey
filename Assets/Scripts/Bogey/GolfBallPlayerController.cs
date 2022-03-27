using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bogey
{

    public class GolfBallPlayerController : MonoBehaviour
    {
        // Start is called before the first frame update

        [Header("Movement Variables")]
        [Range(10, 200)] [SerializeField] private float shotPower = 100.0f;
        [SerializeField] private float shotPowerMultiplier = 1.0f;
        [Range(10, 200)] [SerializeField] private float showPowerMin;
        [Range(10, 200)] [SerializeField] private float shotPowerMax;

        [Header("Physics")]
        [SerializeField] private Rigidbody ball;
        [SerializeField] private float ballVelocity;
        [SerializeField] public bool isMoving;

        [SerializeField] Transform playerInputSpace = default;

        private Vector3 lastPosition = default;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if (ballVelocity < 3f)
                isMoving = false;
            else
                isMoving = true;

            ControlBall();
        }

        #region Ball Controls
        public void ControlBall()
        {
            Vector3 force = Vector3.zero;

            if(Input.GetMouseButtonDown(0) && !isMoving)
            {
                
                if(playerInputSpace)
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
                ball.AddForce(force);
                
            }

        }
        #endregion
    }
}