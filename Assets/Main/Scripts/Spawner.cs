using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GvG
{
    public abstract class Spawner<T> : MonoBehaviour where T : Object
    {
        [SerializeField] protected T prefab_;
        [SerializeField] protected float spawnRadius_;
        [SerializeField] protected float spawnRate_;
        [SerializeField] protected int quota_;


        public void StartSpawn()
        {
            StartCoroutine(SpawnImpl());
        }

        public void StopSpawn()
        {
            StopAllCoroutines();
            spawning = false;
        }

        protected void IncrementSpawned()
        {
            if (!spawningIndefinitely)
                spawned++;
            else
                spawned = -1;
        }

        abstract protected void SpawnAction();

        abstract protected void SpawnPostAction();

        abstract protected bool SpawnConditionTest();
        IEnumerator SpawnImpl()
        {
            spawning = true;
            while (SpawnConditionTest())
            {
                SpawnAction();
                yield return new WaitForSeconds(1.0f / spawnRate_);
            }
            SpawnPostAction();
        }

        public T prefab { get { return prefab_; } }
        public bool spawning { get; private set; }
        public bool spawningIndefinitely { get { return quota_ == 0; } }
        public int spawned { get; protected set; }
    }
}
