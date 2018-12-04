using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ChainObject : MonoBehaviour, IEqualityComparer<ChainObject> {
    [SerializeField]
    private ChainType type;
    public ChainType Type => type;

    private HashSet<ChainObject> chainObjects = new HashSet<ChainObject>();

    protected void DestroyChain(HashSet<ChainObject> alreadyDestroyed) {
        var destroyObjects = new HashSet<ChainObject>(chainObjects);

        if (alreadyDestroyed == null) {
            alreadyDestroyed = new HashSet<ChainObject>();
        }
        alreadyDestroyed.Add(this);

        destroyObjects.ExceptWith(alreadyDestroyed);
        alreadyDestroyed.UnionWith(destroyObjects);

        var destroyList = new List<ChainObject>(destroyObjects);
        destroyList.ForEach(o => o.DestroyChain(alreadyDestroyed));

        Destroy(gameObject);
        chainObjects.Clear();
    }

    private void Update() {
        if (GetChainCount(null) >= 4) {
            DestroyChain(null);
        }
    }

    public int GetChainCount(HashSet<ChainObject> alreadyCounted) {
        var findObjects = new HashSet<ChainObject>(chainObjects);

        if (alreadyCounted == null) {
            alreadyCounted = new HashSet<ChainObject>();
        }
        alreadyCounted.Add(this);

        findObjects.ExceptWith(alreadyCounted);

        alreadyCounted.UnionWith(findObjects);
        return findObjects.Sum(o => o.GetChainCount(alreadyCounted)) + 1;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("ChainObj")) { return; }
        var chainObj = collision.gameObject.GetComponent<ChainObject>();

        if (chainObj.Type == this.type) {
            chainObjects.Add(chainObj);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("ChainObj")) { return; }
        var chainObj = collision.gameObject.GetComponent<ChainObject>();

        if (chainObj.Type == this.type) {
            if (chainObjects.Contains(chainObj)) {
                chainObjects.Remove(chainObj);
            }
        }
    }

    #region IEqualityComparer implements
    bool IEqualityComparer<ChainObject>.Equals(ChainObject x, ChainObject y) {
        if (x == null) { return false; }
        return x.GetInstanceID().Equals(y?.GetInstanceID());
    }

    int IEqualityComparer<ChainObject>.GetHashCode(ChainObject obj) {
        return obj.GetInstanceID().GetHashCode();
    }
    #endregion
}
