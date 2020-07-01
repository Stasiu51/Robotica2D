using GameObjects;
using ObjectAccess;
using UnityEngine;

namespace UIControl
{
    public class CameraControl : MonoBehaviour
    {
        private Blocks _blocks;
        private const float PANSPEED = 5f;
        private const float ZOOMSPEED = 20f;
        private const float ZOOMINNERLIMIT = -10f;
        void Start()
        {
            if (_blocks == null) _blocks = GameObject.Find("ObjectAccess").GetComponent<Managers>().Blocks;
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
            return new Vector3(x*PANSPEED,y*PANSPEED,z*ZOOMSPEED);
        }

        private Vector3 clampedMovement(Vector3 input)
        {
            float zc = Mathf.Clamp(input.z, -20f, ZOOMINNERLIMIT);
            float xc = input.x;
            float yc = input.y;
            return new Vector3(xc,yc,zc);
        }
    }
}
