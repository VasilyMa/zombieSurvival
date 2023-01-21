using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player")]
public class PlayerConfig : ScriptableObject
{
    public GameObject PlayerObject;

    public float MoveSpeed;
    public int BaseHealth;
}
