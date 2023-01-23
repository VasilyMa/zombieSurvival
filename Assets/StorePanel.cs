using UnityEngine;
using UnityEngine.UI;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Client
{
    public class StorePanel : MonoBehaviour
    {
        EcsWorld _world;
        GameState _state;
        EcsPool<EncounterComponent> _encountPool = default;
        EcsPool<InterfaceComponent> _interfacePool = default;
        EcsPool<DamageComponent> _damagePool = default;
        [SerializeField] Transform slots;
        [SerializeField] Transform HolderStore;
        Item currentItem;
        public GraphicRaycaster m_Raycaster;
        public PointerEventData m_PointerEventData;
        public EventSystem m_EventSystem;
        public void Init(EcsWorld world, GameState state)
        {
            _world = world;
            _state = state;
            _interfacePool = _world.GetPool<InterfaceComponent>();
            _encountPool = _world.GetPool<EncounterComponent>();


            _damagePool = _world.GetPool<DamageComponent>();
            //Fetch the Raycaster from the GameObject (the Canvas)
            m_Raycaster = GetComponent<GraphicRaycaster>();
            //Fetch the Event System from the Scene
            m_EventSystem = FindObjectOfType<EventSystem>();
        }
        public void BuyItem()
        {
            if (_state.Money >= 100)
            {
                var newItem = Instantiate(_state.InterfaceConfig.Item, slots);
                newItem.GetComponent<Item>().Init(_world, _state);
                _state.Money -= 100;
                _state.Save();
                _interfacePool.Get(_state.EntityInterface).ResourcePanel.SetMoney();
            }
        }
        private void Update()
        {
            if (!HolderStore.gameObject.activeSelf) 
                return;

            if (Input.GetMouseButtonDown(0))
            { 
                m_PointerEventData = new PointerEventData(m_EventSystem);
                m_PointerEventData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                m_Raycaster.Raycast(m_PointerEventData, results);
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.CompareTag("Item"))
                    {
                        if (result.gameObject.TryGetComponent<Item>(out var item))
                        {
                            currentItem = item;
                            currentItem.GetComponent<Image>().raycastTarget = false;
                            break;
                        }
                    }
                }
            }

            if (Input.GetMouseButton(0))
                if(currentItem != null)
                    currentItem.transform.position = Input.mousePosition;

            if (Input.GetMouseButtonUp(0))
            {
                m_PointerEventData = new PointerEventData(m_EventSystem);
                m_PointerEventData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                m_Raycaster.Raycast(m_PointerEventData, results);
                foreach (RaycastResult result in results)
                {
                    if (result.gameObject.CompareTag("Item"))
                    {
                        if (result.gameObject.TryGetComponent<Item>(out var item))
                        {
                            if (item.ItemID == currentItem.ItemID)
                            {
                                _damagePool.Get(_state.EntityPlayer).DamageAmount += 10;
                                currentItem.gameObject.SetActive(false);
                                currentItem = null;
                                return;
                            }
                        }
                    }
                }
            }
        }


        public void Next()
        {
            _encountPool.Add(_world.NewEntity());
            HolderStore.gameObject.SetActive(false);
        }
        public Transform GetHolderStore()
        {
            return HolderStore;
        }
    }
}