using GameObjects;
using ObjectAccess;
using UnityEngine;

namespace UIControl
{
    public class CameraControl : MonoBehaviour
    {
        private Blocks _blocks;
        private const float PANGRADIENT = 20f;
        private const float PANSPEED = 5f;
        private const float ZOOMSPEED = 20f;
        private const float ZOOMINNERLIMIT = -10f;
        private const float ZOOMOUTERLIMIT = -40f;
        void Start()
        {
            if (_blocks == null) _blocks = Access.managers.Blocks;
        }
        
        void Update()
        {
            transform.position = clampedMovement(transform.position + getInputMovement() * Time.deltaTime);
        }

        private Vector3 getInputMovement()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            float z = Input.GetAxis("Mouse ScrollWheel");
            return new Vector3(x*panSpeed(),y*panSpeed(),z*ZOOMSPEED);
        }

        private float panSpeed()
        {
            return PANSPEED + PANGRADIENT*(Mathf.Abs(transform.position.z - ZOOMINNERLIMIT) + 1)/(ZOOMINNERLIMIT- ZOOMOUTERLIMIT);
        }

        private Vector3 clampedMovement(Vector3 input)
        {
            float zc = Mathf.Clamp(input.z, ZOOMOUTERLIMIT, ZOOMINNERLIMIT);
            float xc = input.x;
            float yc = input.y;
            return new Vector3(xc,yc,zc);
        }
    }
}
