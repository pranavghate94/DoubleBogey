using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;


/*
 * See: https://catlikecoding.com/unity/tutorials/movement/orbit-camera/
 */

namespace Bogey.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class MouseOrbit : MonoBehaviourPunCallbacks
    {
        [SerializeField] Transform focus = default;
        [SerializeField, Range(1f, 20f)] float distance = 1f;
        [SerializeField, Range(0.1f, 1f)] float smoothFactor = 0.5f;
        [SerializeField, Min(0f)] float focusRadius = 1f;
        [SerializeField, Range(1f, 360f)] float rotationSpeed = 90f;


        [SerializeField, Range(-89f, 89f)] float minVerticalAngle = -30f, maxVerticalAngle = 60f;

        Vector2 orbitAngles = new Vector2(45f, 0f);

        private Vector3 focusPoint;
        private Vector3 lookAtDirection;

        UnityEngine.Camera regularCamera;

        Vector3 cameraHalfExtends
        {
            get
            {
                Vector3 halfExtends;
                halfExtends.y = regularCamera.nearClipPlane * Mathf.Tan(0.5f * Mathf.Deg2Rad * regularCamera.fieldOfView);
                halfExtends.x = halfExtends.y * regularCamera.aspect;
                halfExtends.z = 0f;

                return halfExtends;
            }
            
        }
        

        private void Awake()
        {

            if(!photonView.IsMine)
            {
                this.gameObject.SetActive(false);
            }

            regularCamera = GetComponent<UnityEngine.Camera>();
            focusPoint = focus.position;
            transform.localRotation = Quaternion.Euler(orbitAngles);

        }


     

        private void LateUpdate()
        {
            if(photonView.IsMine)
            {
                ControlCamera();
            }
        }

        private void OnValidate()
        {
            if (maxVerticalAngle < minVerticalAngle)
                maxVerticalAngle = minVerticalAngle;
        }

        private bool ManualRotation()
        {
            Vector2 input = new Vector2(
                    Input.GetAxis("Mouse Y"),
                    Input.GetAxis("Mouse X")
                );
            const float e = 0.001f;
            if (input.x < -e || input.x > e || input.y < -e || input.y > e)
            {
                orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
                return true;
            }
            return false;
        }
        private void ConstrainAngles()
        {
            orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

            if (orbitAngles.y < 0f)
                orbitAngles.y += 360f;
            else if (orbitAngles.y >= 360f)
                orbitAngles.y -= 360f;
        }
      

        private void ControlCamera()
        {
            Quaternion lookRotation;
            Vector3 lookPosition;

            if (ManualRotation())
            {
                ConstrainAngles();
                lookRotation = Quaternion.Euler(orbitAngles);
            }
            else
            {
                lookRotation = transform.localRotation;
            }
            focusPoint = focus.position;
            lookAtDirection = lookRotation * Vector3.forward;
            lookPosition = focusPoint - lookAtDirection * distance;

            if (Physics.BoxCast(focusPoint, cameraHalfExtends, -lookAtDirection,
                out RaycastHit hit, lookRotation, distance - regularCamera.nearClipPlane))
            {
                lookPosition = focusPoint - lookAtDirection * (hit.distance + regularCamera.nearClipPlane);
            }

            transform.position = lookPosition;
            //Vector3.Slerp(transform.position, newPosition, smoothFactor);
            transform.rotation = lookRotation;
        }


    }

}