using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GvG
{
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField] float delay_ = 1;

        private void OnEnable()
        {
            StartCoroutine(DestroyImpl());
        }
        private void OnDisable()
        {
            StopAllCoroutines();
        }
        IEnumerator DestroyImpl()
        {
            yield return new WaitForSeconds(delay_);
            Destroy(gameObject);
        }
    }

}
