using UnityEngine;
using UnityEngine.UI;

namespace GvG
{
    public class GombiOnBoard : MonoBehaviour
    {
        [SerializeField] Text onboardCounter_;
        private RefugeeShip refugeeShip_;
        private void Start()
        {
            refugeeShip_ = GameObject.FindGameObjectWithTag("RefugeeShip").GetComponent<RefugeeShip>();
        }

        private void Update()
        {
            UpdateOnboardCounter(refugeeShip_.carriedGombis, refugeeShip_.maxCapacity);
        }
        public void UpdateOnboardCounter(int carried, int maxCapacity)
        {
            onboardCounter_.text = carried + "/" + maxCapacity;
        }
    }
}
