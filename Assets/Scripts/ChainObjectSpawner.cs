using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainObjectSpawner : MonoBehaviour {
    [SerializeField]
    private ChainObject[] prefabs;

    private Dictionary<ChainType, ChainObject> typeMap;

    private void Awake() {
        typeMap = prefabs.ToDictionary(o => o.Type);
    }

    public ChainObject Spawn(ChainType type, Vector3 position) {
        var prefab = typeMap[type];
        var quaternion = Quaternion.identity;
        return Instantiate(prefab, position, quaternion);
    }
}
