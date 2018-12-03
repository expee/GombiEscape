using UnityEngine;

namespace GvG
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DamageParticle : MonoBehaviour
    {
        private ParticleSystem ps_ = null;
        private void Awake()
        {
            ps_ = GetComponent<ParticleSystem>();
        }
    }
}
