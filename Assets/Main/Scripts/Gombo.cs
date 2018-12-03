using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GvG
{
    public class Gombo : Character, ICaptureable
    {
        [SerializeField] private float scanRadius_ = 0;
        [SerializeField] private Muzzle muzzle_ = null;
        [SerializeField] private float followDistance_ = 1;
        [SerializeField] private float followDistanceRandomness_ = 5;
        [SerializeField] private float randomRate_ = 1;
        private Collider2D[] enemyColliderPool_;
        private Vector2 randomPosition_ = Vector2.zero;
        private IDamageable refugeeShip_;

        override protected void StartUpSequence()
        {
            enemyColliderPool_ = new Collider2D[200];
            StartCoroutine(RecalculateRandomPosition());
        }

        override protected void ResetState()
        {
            health_ = maxHealth_;
            rb2d_.velocity = Vector2.zero;
        }

        public IDamageable FindNearestEnemy()
        {
            float nearestDistance = float.MaxValue;
            IDamageable target = null;
            int capturedCount = Physics2D.OverlapCircleNonAlloc(transform.localPosition, scanRadius_, enemyColliderPool_);
            for (int i = 0; i < capturedCount; i++)
            {
                IDamageable targetCandidate = enemyColliderPool_[i].GetComponent<Character>() as IDamageable;
                if (targetCandidate != null && targetCandidate.damageeTag != tag)
                {
                    float targetCandidateDist = Vector2.Distance(transform.localPosition, targetCandidate.position);
                    if (targetCandidateDist < nearestDistance)
                    {
                        target = targetCandidate;
                        nearestDistance = targetCandidateDist;
                    }
                }
            }
            return target;
        }

        public void FindRefugeeShip()
        {
            refugeeShip_ = GameObject.FindGameObjectWithTag("RefugeeShip").GetComponent<RefugeeShip>() as IDamageable;
        }

        public bool FollowRefugeeShip()
        {
            if (refugeeShip_ == null)
                return false;
            currentTargetPos_ = (position - refugeeShip_.position).normalized * followDistance_ + refugeeShip_.position;
            currentTargetPos_ += randomPosition_;
            MoveTo(currentTargetPos_);
            return true;
        }

        public void GoToEnemy(IDamageable enemy)
        {
            currentTargetPos_ = (position - enemy.position).normalized * followDistance_ + enemy.position;
            currentTargetPos_ += randomPosition_;
            MoveTo(currentTargetPos_);
        }

        public void FireWeapon()
        {
            muzzle_.Fire();
        }

        public void StopFiring()
        {
            muzzle_.StopFire();
        }

        IEnumerator RecalculateRandomPosition()
        {
            while (true)
            {
                yield return new WaitForSeconds(1 / randomRate_);
                randomPosition_ = Random.insideUnitCircle.normalized * followDistanceRandomness_;

            }
        }
        #region Properties

        public float scanRadius { get { return scanRadius_; } }
        public Rigidbody2D captureeRigidbody2d { get { return rb2d_; } }
        #endregion
    }
}