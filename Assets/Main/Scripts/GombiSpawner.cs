using UnityEngine;

namespace GvG
{
    public class GombiSpawner : Spawner<Gombi>
    {
        override protected void SpawnAction()
        {
            Vector2 currentPosition = transform.localPosition;
            Vector2 spawnPosition = Random.insideUnitCircle * spawnRadius_ + currentPosition;
            Instantiate(prefab_, spawnPosition, Quaternion.identity);
            IncrementSpawned();
        }
        override protected void SpawnPostAction()
        {

        }

        override protected bool SpawnConditionTest()
        {
            if (spawningIndefinitely)
            {
                return true;
            }
            else
            {
                return spawned < quota_;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.localPosition, spawnRadius_);
        }
    }
}
