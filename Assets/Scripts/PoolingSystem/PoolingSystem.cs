using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Systems.PoolingSystem;
using UnityEngine;


[System.Serializable]
public class SourceObjects
{
    public string ID;

    public GameObject SourcePrefab;

    public int MinNumberOfObject = 0;
    public bool AllowGrow = true;
    public bool AutoDestroy = true;

     public List<GameObject> clones;
}

public class PoolingSystem : Singleton<PoolingSystem>
{
    public List<SourceObjects> SourceObjects = new List<SourceObjects>();


    public int DefaultCount = 10;

    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        InitilizePool();
    }


    public void InitilizePool()
    {
        InitilizeGameObjects();
    }

    private void InitilizeGameObjects()
    {
        foreach (var t in SourceObjects)
        {
            var copyNumber = DefaultCount;
            if (t.MinNumberOfObject != 0)
                copyNumber = t.MinNumberOfObject;

            for (var j = 0; j < copyNumber; j++)
            {
                var go = Instantiate(t.SourcePrefab, transform);
                go.SetActive(false);
                if (t.AutoDestroy)
                    go.AddComponent<PoolObject>();

                t.clones.Add(go);
            }
        }
    }


    public GameObject InstantiateAPS(string Id)
    {
        foreach (var t in SourceObjects.Where(t => string.Equals(t.ID, Id)))
        {
            GameObject firstInactive = null;

            foreach (var t1 in t.clones)
            {
                if (t1 != null && !t1.activeInHierarchy)
                {
                    firstInactive = t1;
                    break;
                }
            }

            if (firstInactive != null)
            {
                firstInactive.SetActive(true);

                var poolable = firstInactive.GetComponent<IPoolable>();
                if (poolable != null)
                    poolable.Initilize();

                return firstInactive;
            }

            if (t.AllowGrow)
            {
                var go = Instantiate(t.SourcePrefab, transform);
                t.clones.Add(go);

                var poolable = go.GetComponent<IPoolable>();
                if (poolable != null)
                    poolable.Initilize();

                if (t.AutoDestroy)
                    go.AddComponent<PoolObject>();

                return go;
            }
        }

        return null;
    }

    public GameObject InstantiateAPS(string iD, Vector3 position)
    {
        var go = InstantiateAPS(iD);
        if (go)
        {
            go.transform.position = position;
            return go;
        }
        else
            return null;
    }

    public GameObject InstantiateAPS(string iD, Vector3 position, Quaternion rotation)
    {
        var go = InstantiateAPS(iD);
        if (go)
        {
            go.transform.position = position;
            go.transform.rotation = rotation;
            return go;
        }
        else
            return null;
    }

    public GameObject InstantiateAPS(GameObject sourcePrefab)
    {
        foreach (var t in SourceObjects.Where(t => ReferenceEquals(t.SourcePrefab, sourcePrefab)))
        {
            foreach (var t1 in t.clones.Where(t1 => !t1.activeInHierarchy))
            {
                t1.SetActive(true);
                return t1;
            }

            if (!t.AllowGrow) continue;
            var go = Instantiate(t.SourcePrefab, transform);
            t.clones.Add(go);
            return go;
        }

        return null;
    }

    public GameObject InstantiateAPS(GameObject sourcePrefab, Vector3 position)
    {
        var go = InstantiateAPS(sourcePrefab);
        if (go)
        {
            go.transform.position = position;
            return go;
        }
        else
            return null;
    }

    

    public void DestroyAPS(GameObject clone)
    {
        clone.transform.position = transform.position;
        clone.transform.rotation = transform.rotation;
        clone.transform.localScale = Vector3.one;
        clone.transform.SetParent(transform);

        //ForEach e al
        var poolable = clone.GetComponent<IPoolable>();
        if (poolable != null)
            poolable.Dispose();
        clone.SetActive(false);
    }

    public void DestroyAPS(GameObject clone, float waitTime)
    {
        StartCoroutine(DestroyAPSCo(clone, waitTime));
    }

    IEnumerator DestroyAPSCo(GameObject clone, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        DestroyAPS(clone);
    }
}