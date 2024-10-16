using ComponentUtils;
using System.Collections;
using UnityEngine;

namespace Tcp4
{
    public abstract class BaseEntity : MonoBehaviour
    {
        public BaseEntitySO baseStatus;
        public StatusComponent statusComponent { get; private set; }
        public Animator anim { get; private set; }
        public Rigidbody rb { get; private set; }
        public Collider coll { get; private set; }
        public CollisionComponent checker { get; private set; }
        public ServiceLocator serviceLocator { get; private set; }

        public virtual void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            serviceLocator = new ServiceLocator();
            RegisterComponents();
            GetComponents();
        }

        private void RegisterComponents()
        {
            serviceLocator.RegisterService(baseStatus);
        }

        private void GetComponents()
        {
            statusComponent = GetComponent<StatusComponent>();
            anim = GetComponentInChildren<Animator>();
            rb = GetComponent<Rigidbody>();
            coll = GetComponent<Collider>();
            checker = GetComponent<CollisionComponent>();
        }
    }

}