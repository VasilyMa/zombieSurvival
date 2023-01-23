using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolBullet : MonoBehaviour
{
    [SerializeField] private int poolCapacity = 10;
    [SerializeField] private bool isAutoExpand = false;
    [SerializeField] private Projectile prefab;

    private Pool<Projectile> pool;

    private void Start()
    {
        pool = new Pool<Projectile>(prefab, poolCapacity, transform);
        pool.isAutoExpand = isAutoExpand;
    }
    public Pool<Projectile> GetPool()
    {
        return pool;
    }
}
