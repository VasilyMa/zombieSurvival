using UnityEngine;
namespace Client {
    struct ViewComponent {
        public GameObject GameObject;
        public ECSInfo ECSInfo;
        public Rigidbody Rigidbody;
        public Transform FirePoint;
    }
}