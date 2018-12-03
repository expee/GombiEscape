using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GvG
{
    public class Gombi : Character
    {
        public delegate void GombiDeadEvent();
        public static GombiDeadEvent gombiDeadEvent;
        [SerializeField] private float scanRadius_ = 0;
        [SerializeField] private DamageParticle damageParticle_ = null;
        [SerializeField] float directionChangeRate_ = .5f;
        Collider2D[] othersColliderPool_;
        ContactPoint2D[] contactPoints_;
        protected IDamageable refugeeShip_;
        protected bool entering_ = false;

        protected void Start()
        {
            FindRefugeeShip();
        }
        protected new void Update()
        {
            base.Update();
            IDamageable rShip = ScanForRefugeeShip();
            if (rShip != null)
                EnterRefugeeShip();
        }

        protected void FixedUpdate()
        {
            MoveTo(currentTargetPos_);
        }

        override protected void StartUpSequence()
        {
            contactPoints_ = new ContactPoint2D[200];
            othersColliderPool_ = new Collider2D[200];
        }

        override protected void ResetState()
        {
            health_ = maxHealth_;
            rb2d_.velocity = Vector2.zero;
            rb2d_.gravityScale = 0;
        }

        public IDamageable FindNearestEnemy()
        {
            float nearestDistance = float.MaxValue;
            IDamageable target = null;
            int capturedCount = Physics2D.OverlapCircleNonAlloc(transform.localPosition, scanRadius_, othersColliderPool_);
            for (int i = 0; i < capturedCount; i++)
            {
                IDamageable targetCandidate = othersColliderPool_[i].GetComponent<Character>() as IDamageable;
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
        public IDamageable ScanForRefugeeShip()
        {
            int capturedCount = Physics2D.OverlapCircleNonAlloc(transform.localPosition, scanRadius_, othersColliderPool_);
            for (int i = 0; i < capturedCount; i++)
            {
                IDamageable targetCandidate = othersColliderPool_[i].GetComponent<Character>() as IDamageable;
                if (targetCandidate != null && targetCandidate.damageeTag == "RefugeeShip")
                {
                    return targetCandidate;
                }
            }
            return null;
        }

        public void StartMoving()
        {
            //StartCoroutine(ChangeDirectionImpl());
        }

        public bool DamageCheck(Collision2D collision)
        {
            bool damageTook = false;
            IProjectile projectile = collision.collider.GetComponent<PainBullet>() as IProjectile;
            if (projectile != null && !dead)
            {
                damageTook = true;
                TakeDamage(projectile.damage);
            }
            return damageTook;
        }

        public void EmitDamageParticle(Collision2D collision)
        {
            if (damageParticle_ == null)
                return;
            int contactCount = collision.GetContacts(contactPoints_);
            int selectedContactPointIdx = Random.Range(0, contactCount);
            Vector2 contactPosition = contactPoints_[selectedContactPointIdx].point;
            Vector2 contactNormal = contactPoints_[selectedContactPointIdx].normal;
            Quaternion rotationTowardsNormal = Quaternion.LookRotation(Vector3.forward, contactNormal);
            Instantiate(damageParticle_, contactPosition, rotationTowardsNormal);
        }
        public void FindRefugeeShip()
        {
            refugeeShip_ = GameObject.FindGameObjectWithTag("RefugeeShip").GetComponent<RefugeeShip>() as IDamageable;
        }

        public void ExitRefugeeShip()
        {
            entering_ = false;
            currentTargetPos_ = refugeeShip_.position + Random.insideUnitCircle.normalized * 5.0f;
        }

        public void EnterRefugeeShip()
        {
            entering_ = true;
            currentTargetPos_ = refugeeShip_.position;
        }

        public void DetermineNextPositionRandomly()
        {
            currentTargetPos_ = currentPosition + Random.insideUnitCircle * maxSpeed_;
            ChangeTargetPosition(currentTargetPos_);
        }

        override public void Dead()
        {
            base.Dead();
            rb2d_.gravityScale = 1;
        }

        #region Events
        protected void OnDisable()
        {
            ResetState();
        }

        protected void OnCollisionEnter2D(Collision2D other)
        {
            bool damaged = DamageCheck(other);
            if (damaged && dead && gombiDeadEvent != null)
            {
                gombiDeadEvent.Invoke();
            }
        }
        #endregion
        IEnumerator ChangeDirectionImpl()
        {
            while (true)
            {
                DetermineNextPositionRandomly();
                yield return new WaitForSeconds(1 / directionChangeRate_);
            }
        }

        #region Properties
        public float scanRadius { get { return scanRadius_; } }
        public bool insideShip { get; set; }
        public bool entering { get { return entering_; } }
        public IDamageable refugeeShip { get { return refugeeShip_; } }
        #endregion
    }
}
