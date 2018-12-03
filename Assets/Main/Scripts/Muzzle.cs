using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GvG
{
    public class Muzzle : MonoBehaviour
    {
        [SerializeField] float fireRate_ = 1;
        [SerializeField] Bullet projectile_ = null;
        [SerializeField] [Range(0.0f, 1.0f)] float accuracy_ = 1;
        private AudioSource audio_;
        private float deviation = 0;
        private void Awake()
        {
            audio_ = GetComponent<AudioSource>();
            deviation = 90 - 90 * accuracy_;
        }
        public void Fire()
        {
            if (!isFiring)
                StartCoroutine(FireImpl());
        }

        public void StopFire()
        {
            StopAllCoroutines();
            isFiring = false;
        }

        IEnumerator FireImpl()
        {
            isFiring = true;
            while (true)
            {
                IProjectile projectile = Instantiate(projectile_, transform.position, transform.rotation) as IProjectile;
				float currentDeviation = Random.Range(-deviation, deviation);
				Vector2 fireDirection = transform.TransformDirection(Vector2.up);
				Quaternion fireDirRotation = Quaternion.AngleAxis(currentDeviation, Vector3.back);
				fireDirection = fireDirRotation * fireDirection;
				projectile.getTransform.rotation *= fireDirRotation;
                projectile.Launch(fireDirection);
                if(audio_.clip != null)
                    audio_.PlayOneShot(audio_.clip);
                yield return new WaitForSeconds(1.0f / fireRate_);
            }
        }

        public bool isFiring { get; private set; }
    }

}
