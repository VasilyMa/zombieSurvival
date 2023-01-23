using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    public T prefab { get; }
    public bool isAutoExpand { get; set; }
    public Transform Container { get; }
    private List<T> pool;
    public Pool(T prefab, int capacity)
    {
        this.prefab = prefab;
        this.Container = null;

        this.CreatePool(capacity);
    }

    public Pool(T prefab, int capacity, Transform container)
    {
        this.prefab = prefab;
        Container = container;

        this.CreatePool(capacity);
    }

    void CreatePool(int capacity)
    {
        this.pool = new List<T>();

        for (int i = 0; i < capacity; i++)
        {
            this.CreateObject();
        }
    }
    T CreateObject(bool isActiveDefault = false)
    {
        var createdObject = Object.Instantiate(this.prefab, this.Container);
        createdObject.gameObject.SetActive(isActiveDefault);
        this.pool.Add(createdObject);
        return createdObject;
    }
    public bool HasAvailableElement(out T element)
    {
        foreach (var item in pool)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                element = item;
                item.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }
    public T GetAvailableElement()
    {
        if (this.HasAvailableElement(out var element)) return element;
        if (this.isAutoExpand) return this.CreateObject(true);

        throw new System.Exception($"There is not available elements in pool of type {typeof(T)}");
    }
}
