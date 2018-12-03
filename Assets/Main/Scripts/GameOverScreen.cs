using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GvG
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] Text savedGombis_;
        [SerializeField] Text leftBehindGombis_;
        [SerializeField] Text martyredGombis_;
        [SerializeField] Image didntMakeIt_;
        [SerializeField] Image survived_;

        private void OnEnable()
        {
            GUIManager guiMgr = GameObject.FindGameObjectWithTag("GUIManager").GetComponent<GUIManager>();
            SetMartyredGombis(guiMgr.martyredGombis);
            SetGombisLeftBehind(guiMgr.gombisOnPlanet);
            RefugeeShip rShip = GameObject.FindGameObjectWithTag("RefugeeShip").GetComponent<RefugeeShip>();
            SetSavedGombis(rShip.carriedGombis);
            SetSurvived(rShip.health > 0);
        }
        
        public void SetMartyredGombis(int count)
        {
            martyredGombis_.text = "Martyred Gombis =\t\t" + count;
        }

        public void SetGombisLeftBehind(int count)
        {
            leftBehindGombis_.text = "Gombis Left Behind =\t" + count;
        }

        public void SetSavedGombis(int count)
        {
            savedGombis_.text = "Saved Gombis =\t\t" + count;
        }

        public void SetSurvived(bool survived)
        {
            if (survived)
            {
                survived_.gameObject.SetActive(true);
            }
            else
            {
                didntMakeIt_.gameObject.SetActive(true);
            }
        }
    }

}
