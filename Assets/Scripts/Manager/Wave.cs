using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Wave : MonoBehaviour
{
    public List<EnemyType> enemyList;

    [Serializable]
    public struct EnemyType
    {
        public GameObject enemyPrefab;
        public int count;
        public float rate;
        public float spawnDelay;
    }
}
