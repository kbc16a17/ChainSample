using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private ChainObjectSpawner objectSpawner;

    private static readonly Dictionary<KeyCode, ChainType> KeyMap = new Dictionary<KeyCode, ChainType> {
        { KeyCode.Alpha1, ChainType.Black },
        { KeyCode.Alpha2, ChainType.Blue },
        { KeyCode.Alpha3, ChainType.Green },
        { KeyCode.Alpha4, ChainType.Purple },
        { KeyCode.Alpha5, ChainType.Red },
        { KeyCode.Alpha6, ChainType.Yellow },
    };

    private void Update() {
        foreach (var pair in KeyMap) {
            if (Input.GetKeyDown(pair.Key)) {
                spawn(pair.Value);
            }
        }
    }

    private Vector3 getSpawnPosition() {
        var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0.0f;
        return worldPosition;
    }

    private void spawn(ChainType type) {
        var position = getSpawnPosition();
        var obj = objectSpawner.Spawn(type, position);
    }
}
