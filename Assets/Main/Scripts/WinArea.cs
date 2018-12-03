using UnityEngine;

namespace GvG
{
    public class WinArea : MonoBehaviour
    {
        private RefugeeShip refugeeShip_;
        private void Start()
        {
            refugeeShip_ = GameObject.FindGameObjectWithTag("RefugeeShip").GetComponent<RefugeeShip>();
        }

        private void Update()
        {
            FollowRefugeeShipX();
        }
        
        public void FollowRefugeeShipX()
        {
            Vector2 shipPos = refugeeShip_.position;
            transform.localPosition = new Vector2(shipPos.x, transform.localPosition.y);
        }
    }
}
