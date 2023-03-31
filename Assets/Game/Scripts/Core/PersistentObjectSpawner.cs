using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> persistentObjectPrefabs;

        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned)
            {
                return;
            }

            SpawnPersistentObjects();

            hasSpawned = true;
        }

        private void SpawnPersistentObjects()
        {
            persistentObjectPrefabs.ForEach(prefab =>
            {
                var instance = Instantiate(prefab);
                DontDestroyOnLoad(instance);
            });
        }
    }
}
