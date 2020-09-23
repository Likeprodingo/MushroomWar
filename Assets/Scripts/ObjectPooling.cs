using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Pool/PoolObject")]
public class ObjectPooling
{
    #region Data
    
    private List<PoolObject> _objects = default;
    private Transform _objectsParent = default;
    
    #endregion
    
    void AddObject(PoolObject sample, Transform objectsParent) {
        GameObject temp = GameObject.Instantiate(sample.gameObject);
        temp.name = sample.name;
        temp.transform.SetParent (objectsParent);
        _objects.Add(temp.GetComponent<PoolObject> ());
        if (temp.GetComponent<Animator> ())
            temp.GetComponent<Animator> ().StartPlayback ();
        temp.SetActive(false);
    }

    public PoolObject GetObject()
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            if (_objects[i].gameObject.activeInHierarchy == false)
            {
                return _objects[i];
            }
        }

        AddObject(_objects[0], _objectsParent);
        return _objects[_objects.Count - 1];
    }

    public void Initialize(int count, PoolObject sample, Transform objectsParent)
    {
        _objects = new List<PoolObject>();
        _objectsParent = objectsParent;
        for (int i = 0; i < count; i++)
        {
            AddObject(sample, objectsParent);
        }
    }
} 