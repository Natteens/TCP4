using System.Collections.Generic;
using System;
using Tcp4.Resources.Scripts.Core;
using UnityEngine;

namespace Tcp4
{
    public class CollisionComponent : MonoBehaviour
    {
        [Serializable]
        public class CollisionCheck
        {
            public string name;
            public float radius = 0.5f;
            public Vector3 offset = Vector3.zero;
            public LayerMask layers;
            public Color collisionColor = Color.green;
            public Color noCollisionColor = Color.red;
            public CollisionType collisionType = CollisionType.Sphere;
            public float raycastDistance = 1.0f;
            public Vector3 boxSize = Vector3.one;
            [HideInInspector] public bool isColliding;
            [HideInInspector] public ICollisionResult collisionResult;

            public Vector3 GetCheckPosition(Transform transform, Vector3 facingDirection)
            {
                return transform.position + facingDirection * offset.x + Vector3.up * offset.y + transform.right * offset.z;
            }
        }

        public enum CollisionType
        {
            Sphere,
            Raycast,
            Box
        }

        [SerializeField] private List<CollisionCheck> collisionChecks = new List<CollisionCheck>();

        private Dictionary<CollisionType, ICollisionDetector> collisionDetectors;
        private Vector3 FacingDirection => transform.forward;
        [SerializeField] private bool showGizmosInEditor = true;
        private void Awake()
        {
            InitializeCollisionDetectors();
        }

        private void InitializeCollisionDetectors()
        {
            collisionDetectors = new Dictionary<CollisionType, ICollisionDetector>
            {
                { CollisionType.Sphere, new SphereCollisionDetector() },
                { CollisionType.Raycast, new RaycastCollisionDetector() },
                { CollisionType.Box, new BoxCollisionDetector() }
            };
        }

        private void FixedUpdate()
        {
            UpdateCollisionStates();
        }

        private void UpdateCollisionStates()
        {
            foreach (var check in collisionChecks)
            {
                if (collisionDetectors.TryGetValue(check.collisionType, out var detector))
                {
                    Vector3 checkPosition = check.GetCheckPosition(transform, transform.forward);
                    check.collisionResult = detector.Detect(checkPosition, check);
                    check.isColliding = check.collisionResult != null && check.collisionResult.HasHit;
                }
            }
        }

        public bool IsColliding(string checkName)
        {
            var check = collisionChecks.Find(c => c.name == checkName);
            return check != null && check.isColliding;
        }
        
        public bool IsColliding<T>(string checkName, out T result) where T : class, ICollisionResult
        {
            result = null;
            var check = collisionChecks.Find(c => c.name == checkName);
            if (check != null && check.collisionResult is T typedResult)
            {
                result = typedResult;
                return check.isColliding;
            }
            return false;
        }
        private void OnDrawGizmos()
        {
            if (!showGizmosInEditor && !Application.isPlaying) return;

            foreach (var check in collisionChecks)
            {
                if (collisionDetectors != null && collisionDetectors.TryGetValue(check.collisionType, out var detector))
                {
                    Vector3 checkPosition = check.GetCheckPosition(transform, transform.forward);
                    detector.DrawGizmos(check, checkPosition);
                }
            }
        }
    }

    public interface ICollisionResult 
    {
        GameObject HitObject { get; }
        Vector3 HitPoint { get; }
        float Distance { get; }
        bool HasHit { get; }
    }

    public class EntityCollisionResult : ICollisionResult
    {
        public BaseEntity Entity { get; }
        public GameObject HitObject => Entity?.gameObject;
        public Vector3 HitPoint => Entity?.transform.position ?? Vector3.zero;
        public float Distance => 0f;
        public bool HasHit => Entity != null;

        public EntityCollisionResult(BaseEntity entity) => Entity = entity;
    }

    public class CollisionResultBase : ICollisionResult
    {
        public GameObject HitObject { get; }
        public Vector3 HitPoint { get; }
        public float Distance { get; }
        public bool HasHit => HitObject != null;

        protected CollisionResultBase(GameObject hitObject, Vector3 hitPoint, float distance)
        {
            HitObject = hitObject;
            HitPoint = hitPoint;
            Distance = distance;
        }
    }
    
    public class RaycastResult : CollisionResultBase
    {
        public RaycastHit RawHit { get; }

        public RaycastResult(RaycastHit hit) 
            : base(
                hit.collider?.gameObject,
                hit.point,
                hit.distance)
        {
            RawHit = hit;
        }
    }

    public class BoxCollisionResult : CollisionResultBase
    {
        public Collider[] HitColliders { get; }
        public Vector3 Size { get; }

        public BoxCollisionResult(Collider[] hitColliders, Vector3 center, Vector3 size) 
            : base(
                hitColliders.Length > 0 ? hitColliders[0].gameObject : null,
                center,
                size.magnitude)
        {
            HitColliders = hitColliders;
            Size = size;
        }
    }
    
    public class SphereCollisionResult : CollisionResultBase
    {
        public Collider[] HitColliders { get; }
        public float Radius { get; }

        public SphereCollisionResult(Collider[] hitColliders, Vector3 center, float radius) 
            : base(
                hitColliders.Length > 0 ? hitColliders[0].gameObject : null,
                center,
                radius)
        {
            HitColliders = hitColliders;
            Radius = radius;
        }
    }
    
    public interface ICollisionDetector
    {
        ICollisionResult Detect(Vector3 position, CollisionComponent.CollisionCheck check);
        void DrawGizmos(CollisionComponent.CollisionCheck check, Vector3 position);
    }

    public class SphereCollisionDetector : ICollisionDetector
    {
        public ICollisionResult Detect(Vector3 position, CollisionComponent.CollisionCheck check)
        {
            Collider[] hitColliders = new Collider[10];
            int numColliders = Physics.OverlapSphereNonAlloc(position, check.radius, hitColliders, check.layers);
            
            if (numColliders > 0)
            {
                Collider[] results = new Collider[numColliders];
                Array.Copy(hitColliders, results, numColliders);
                
                var result = new SphereCollisionResult(results, position, check.radius);
                
                if (result.HitObject != null)
                {
                    var entity = result.HitObject.GetComponent<BaseEntity>();
                    return entity != null ? new EntityCollisionResult(entity) : result;
                }
            }
            return null;
        }

        public void DrawGizmos(CollisionComponent.CollisionCheck check, Vector3 position)
        {
            Gizmos.color = check.isColliding ? check.collisionColor : check.noCollisionColor;
            Gizmos.DrawWireSphere(position, check.radius);
            
            float size = 0.1f;
            Gizmos.DrawLine(position + Vector3.up * size, position - Vector3.up * size);
            Gizmos.DrawLine(position + Vector3.right * size, position - Vector3.right * size);
        }
    }

    public class RaycastCollisionDetector : ICollisionDetector
    {
        public ICollisionResult Detect(Vector3 position, CollisionComponent.CollisionCheck check)
        {
            const float skinWidth = 0.01f;
            
            if (Physics.SphereCast(position, check.radius * 0.5f, Vector3.down, out RaycastHit hit, 
                    check.raycastDistance + skinWidth, check.layers))
            {
                var entity = hit.collider.GetComponent<BaseEntity>();
                return entity != null ? new EntityCollisionResult(entity) : new RaycastResult(hit);
            }
            return null;
        }

        public void DrawGizmos(CollisionComponent.CollisionCheck check, Vector3 position)
        {
            Gizmos.color = check.isColliding ? check.collisionColor : check.noCollisionColor;
            Gizmos.DrawLine(position, position + Vector3.down * check.raycastDistance);
            Gizmos.DrawWireSphere(position, check.radius * 0.5f);
            Gizmos.DrawWireSphere(position + Vector3.down * check.raycastDistance, check.radius * 0.5f);
        }
    }
    
    public class BoxCollisionDetector : ICollisionDetector
    {
        public ICollisionResult Detect(Vector3 position, CollisionComponent.CollisionCheck check)
        {
            Vector3 halfExtents = check.boxSize * 0.5f;
            
            Collider[] hitColliders = new Collider[10];
            int numColliders = Physics.OverlapBoxNonAlloc(position, halfExtents, hitColliders, 
                Quaternion.identity, check.layers);

            if (numColliders > 0)
            {
                Collider[] results = new Collider[numColliders];
                Array.Copy(hitColliders, results, numColliders);

                var result = new BoxCollisionResult(results, position, check.boxSize);
                
                if (result.HitObject != null)
                {
                    var entity = result.HitObject.GetComponent<BaseEntity>();
                    return entity != null ? new EntityCollisionResult(entity) : result;
                }
            }
            return null;
        }

        public void DrawGizmos(CollisionComponent.CollisionCheck check, Vector3 position)
        {
            Gizmos.color = check.isColliding ? check.collisionColor : check.noCollisionColor;
            Gizmos.DrawWireCube(position, check.boxSize);
        }
    }
    
    public class CollisionHelper
    {
        public static bool TryGetEntity<T>(ICollisionResult result, out T entity) where T : BaseEntity
        {
            entity = null;
            if (result?.HitObject == null) return false;
            
            entity = result.HitObject.GetComponent<T>();
            return entity != null;
        }

        public static bool TryGetComponent<T>(ICollisionResult result, out T component) where T : Component
        {
            component = null;
            if (result?.HitObject == null) return false;
            
            component = result.HitObject.GetComponent<T>();
            return component != null;
        }
    }
}