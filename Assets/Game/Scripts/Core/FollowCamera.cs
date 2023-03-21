using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform target;
        [SerializeField]
        float cameraRotationSpeed;

        void Start()
        {
        
        }

        void LateUpdate()
        {
            float cameraRotation = 0f;
            if (Input.GetKey(KeyCode.D))
            {
                cameraRotation = 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                cameraRotation = -1;
            }
            RotateCamera(cameraRotation);
            transform.position = target.position;
        }

        private void RotateCamera(float direction)
        {
            transform.RotateAround(target.position, Vector3.up, direction * cameraRotationSpeed * Time.deltaTime);
        }
    }
}
