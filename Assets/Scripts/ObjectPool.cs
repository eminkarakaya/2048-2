using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [System.Serializable]
    public struct Pool
    {
        public Queue<Item> pooledObjects;
        public Item objectPrefab;
        public int poolSize;
    }

    public Pool[] pools = null;
    void Awake()
    {
        for (int j = 0; j < pools.Length; j++)
        {
            pools[j].pooledObjects = new Queue<Item>();
            for (int i = 0; i < pools[j].poolSize; i++)
            {
                Item obj = Instantiate(pools[j].objectPrefab);
                obj.name = "item " + i + " " + j; 
                obj.gameObject.SetActive(false);

                pools[j].pooledObjects.Enqueue(obj);
            }
        }
    }
    public Item GetPooledObject(int objectType)
    {
        if(objectType >= pools.Length)
            return null;
        Item obj = pools[objectType].pooledObjects.Dequeue();
        obj.itemType = objectType;
        // obj.gameObject.SetActive(true);

        // pools[objectType].pooledObjects.Enqueue(obj);
        return obj;
    }
    public void SetPooledObject(int objectType,Item item)
    {
        pools[objectType].pooledObjects.Enqueue(item);
        
    }
}
