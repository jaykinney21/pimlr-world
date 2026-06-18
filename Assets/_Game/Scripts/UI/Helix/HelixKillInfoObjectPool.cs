using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixKillInfoObjectPool : MonoBehaviour
{
    public static HelixKillInfoObjectPool Instance { get; set; }
    public Transform killParent;
    public HelixGameplayKillInfo prefab;
    public int poolSize = 10;
    private Queue<HelixGameplayKillInfo> objectPool = new Queue<HelixGameplayKillInfo>();
    void Start()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(this.gameObject);

        InitializePool();
    }
    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            HelixGameplayKillInfo obj = Instantiate(prefab);
            obj.gameObject.SetActive(false);
            objectPool.Enqueue(obj);
            obj.transform.localScale = Vector3.one;
        }
    }
    public HelixGameplayKillInfo GetObjectFromPool(Vector3 position, Quaternion rotation,Data data)
    {
        if (objectPool.Count == 0)
        {
            // Pool is empty, create a new object
            HelixGameplayKillInfo obj = Instantiate(prefab, position, rotation);
            obj.transform.localScale = Vector3.one;
            return obj;
        }
        else
        {
            // Reuse an object from the pool
            HelixGameplayKillInfo obj = objectPool.Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.transform.localScale = Vector3.one;
            obj.gameObject.SetActive(true);
            return obj;
        }
    }
    public void ReturnObjectToPool(HelixGameplayKillInfo obj)
    {
        obj.gameObject.SetActive(false);
        objectPool.Enqueue(obj);
    }
}
