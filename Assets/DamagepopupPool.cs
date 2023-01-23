using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagepopupPool : MonoBehaviour
{
    [SerializeField] private int poolCapacity = 10;
    [SerializeField] private bool isAutoExpand = false;
    [SerializeField] private DamagePopup prefab;

    private Pool<DamagePopup> pool;

    private void Start()
    {
        pool = new Pool<DamagePopup>(prefab, poolCapacity, transform);
        pool.isAutoExpand = isAutoExpand;
    }
    public Pool<DamagePopup> GetPool()
    {
        return pool;
    }
}
