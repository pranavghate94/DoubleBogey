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

        private Vector3 lastPosition;



        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void FixedUpdate()
        {
            if (ballVelocity < 0.1f)
                isMoving = false;
            else
                isMoving = true;

            ControlBall();
        }

        #region Ball Controls
        public void ControlBall()
        {

            if(Input.GetMouseButtonDown(0))
            {
                if (!isMoving)
                {
                    lastPosition = transform.position;
                    ball.AddForce(transform.forward * shotPower * shotPowerMultiplier);
                }
            }

        }
        #endregion
    }
}