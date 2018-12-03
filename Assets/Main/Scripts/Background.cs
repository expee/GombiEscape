using UnityEngine;

namespace GvG
{
    public class Background : MonoBehaviour
    {
        private Camera cam_;
        private void Start()
        {
            cam_ = Camera.main;
        }

        private void Update()
        {
            FollowCameraX();
        }
        public void FollowCameraX()
        {
            Vector2 camPos = cam_.transform.localPosition;
            transform.localPosition = new Vector2(camPos.x, transform.localPosition.y);
        }
    }
}
