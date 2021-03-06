using System;
using UnityEngine;

namespace Sample2D {

    public class WorldAssets : MonoBehaviour {

        // 第一级
        [SerializeField] MapEntity mapPrefab;
        [SerializeField] RoleEntity rolePrefab;
        [SerializeField] MonsterEntity monsterPrefab;

        public MapEntity GetMapPrefab() {
            return mapPrefab;
        }

        public RoleEntity GetRolePrefab() {
            return rolePrefab;
        }

        public MonsterEntity GetMonsterPrefab() {
            return monsterPrefab;
        }

    }

}