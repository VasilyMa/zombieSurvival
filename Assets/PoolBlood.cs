using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBlood : MonoBehaviour
{
    [SerializeField] private int poolCapacity = 10;
    [SerializeField] private bool isAutoExpand = false;
    [SerializeField] private BloodEffect prefab;

    private Pool<BloodEffect> pool;

    private void Start()
    {
        pool = new Pool<BloodEffect>(prefab, poolCapacity, transform);
        pool.isAutoExpand = isAutoExpand;
    }
    public Pool<BloodEffect> GetPool()
    {
        return pool;
    }
}
