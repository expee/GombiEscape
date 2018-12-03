using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GvG
{
    public class GUIManager : MonoBehaviour
    {
        [SerializeField] private GameOverScreen gameOverScreen_ = null;
        [SerializeField] private Text gombisOnPlanetText_;
        [SerializeField] private GombiOnBoard onboardCounter_;

        private void Awake()
        {
            gombisOnPlanet = 300;
            Gombi.gombiDeadEvent += UpdateGombisToSave;
            GombiShield.gombiShieldDeadEvent += UpdateMartyredGombis;
            GombiTractor.gombiTractorDeadEvent += UpdateMartyredGombis;
            GombiWorker.gombiWorkerDeadEvent += UpdateMartyredGombis;
        }
        public void ShowGameOverScreen()
        {
            gameOverScreen_.gameObject.SetActive(true);
        }

        public void HideGameOverScreen()
        {
            gameOverScreen_.gameObject.SetActive(false);
        }

        public void UpdateGombisToSave()
        {
            gombisOnPlanet--;
            gombisOnPlanetText_.text = "Stranded Gombis = " + gombisOnPlanet;
        }

        public void UpdateMartyredGombis()
        {
            martyredGombis++;
        }

        private void OnDestroy()
        {
            Gombi.gombiDeadEvent -= UpdateGombisToSave;
            GombiShield.gombiShieldDeadEvent -= UpdateMartyredGombis;
            GombiTractor.gombiTractorDeadEvent -= UpdateMartyredGombis;
            GombiWorker.gombiWorkerDeadEvent -= UpdateMartyredGombis;
        }

        public void UpdateOnboardCounter(int carried, int maxCapacity)
        {
            onboardCounter_.UpdateOnboardCounter(carried, maxCapacity);
        }

        public int gombisOnPlanet { get; set; }
        public int martyredGombis { get; set; }
    }

}