using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Client
{
    [CreateAssetMenu(fileName = "InteraceConfig", menuName = "Configs/Interface")]
    public class InterfaceConfig : ScriptableObject
    {
        public Item Item;
    }
}