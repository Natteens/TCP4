using UnityEngine;
using System;
using System.Collections.Generic;
using Tcp4.Resources.Scripts.Core;

namespace Tcp4.Assets.Resources.Scripts.Core
{
    public class NPCPathfinding : MonoBehaviour
    {
        public enum PathfindingType
        {
            Sequential,
            PingPong,
            Random,
            Loop,
            OneWay
        }

        [Serializable]
        public class PathPoint
        {
            public Vector3 relativePosition;
            public float waitTime;
            public float interactionRadius = 1f;
            public UnityEngine.Events.UnityEvent onPointReached;

            public bool HasCustomInteractionRadius => interactionRadius != 1f;
        }

        [SerializeField] private PathfindingType pathfindingType = PathfindingType.Sequential;
        [SerializeField] private List<PathPoint> pathPoints = new List<PathPoint>();
        [SerializeField] private bool autoStart = true;

        [SerializeField] private float detectionRadius = 3f;
        [SerializeField] private LayerMask obstacleLayer;
        [SerializeField] private float obstacleAvoidanceStrength = 0.5f;

        [SerializeField] private Color pathColor = Color.blue;
        [SerializeField] private Color pointColor = Color.yellow;
        [SerializeField] private float gizmoSphereRadius = 0.2f;

        private int currentPointIndex = 0;
        private int direction = 1;
        private float waitCounter;
        private bool isMoving = false;
        private DynamicEntity entity;
        private Vector3 initialPosition;

        private Dictionary<PathfindingType, IPathStrategy> pathStrategies;
        private IPathStrategy currentPathStrategy;

        public event Action<Vector3> OnMove;
        public event Action<PathPoint> OnPointReached;

        public Vector3 CurrentTargetPosition => pathPoints.Count > 0 ? initialPosition + pathPoints[currentPointIndex].relativePosition : transform.position;
        public float CurrentWaitTime => pathPoints.Count > 0 ? pathPoints[currentPointIndex].waitTime : 0f;
        public bool HasReachedCurrentPoint
        {
            get
            {
                if (pathPoints.Count == 0) return true;
                Vector3 currentPos = transform.position;
                Vector3 targetPos = CurrentTargetPosition;
                float distanceXZ = new Vector2(currentPos.x - targetPos.x, currentPos.z - targetPos.z).magnitude;
                return distanceXZ < pathPoints[currentPointIndex].interactionRadius;
            }
        }

        private void Awake()
        {
            entity = GetComponent<DynamicEntity>();
            InitializePathStrategies();
            initialPosition = transform.position;
        }

        private void Start()
        {
            if (autoStart)
            {
                StartMoving();
            }
        }

        private void InitializePathStrategies()
        {
            pathStrategies = new Dictionary<PathfindingType, IPathStrategy>
            {
                { PathfindingType.Sequential, new SequentialPathStrategy() },
                { PathfindingType.PingPong, new PingPongPathStrategy() },
                { PathfindingType.Random, new RandomPathStrategy() },
                { PathfindingType.Loop, new LoopPathStrategy() },
                { PathfindingType.OneWay, new OneWayPathStrategy() }
            };
            UpdatePathStrategy();
        }

        public void StartMoving()
        {
            isMoving = true;
        }

        public void StopMoving()
        {
            isMoving = false;
        }

        public void SetPathfindingType(PathfindingType newType)
        {
            pathfindingType = newType;
            UpdatePathStrategy();
        }

        private void UpdatePathStrategy()
        {
            if (pathStrategies.TryGetValue(pathfindingType, out var strategy))
            {
                currentPathStrategy = strategy;
            }
        }

        public Vector3 GetMovementDirection()
        {
            if (pathPoints.Count == 0 || !isMoving || waitCounter > 0)
                return Vector3.zero;

            if (HasReachedCurrentPoint)
            {
                HandlePointReached();
                return Vector3.zero;
            }

            Vector3 currentPos = transform.position;
            Vector3 targetPos = CurrentTargetPosition;

            targetPos.y = currentPos.y;

            Vector3 direction = (targetPos - currentPos).normalized;

            Vector3 avoidanceForce = GetObstacleAvoidanceForce();
            avoidanceForce.y = 0;

            Vector3 finalDirection = (direction + avoidanceForce * obstacleAvoidanceStrength).normalized;

            return finalDirection;
        }



        private void HandlePointReached()
        {
            OnPointReached?.Invoke(pathPoints[currentPointIndex]);
            pathPoints[currentPointIndex].onPointReached?.Invoke();
            waitCounter = pathPoints[currentPointIndex].waitTime;
            MoveToNextPoint();
        }

        private Vector3 GetObstacleAvoidanceForce()
        {
            Vector3 avoidanceForce = Vector3.zero;
            Collider[] obstacles = Physics.OverlapSphere(transform.position, detectionRadius, obstacleLayer);

            foreach (var obstacle in obstacles)
            {
                Vector3 directionToObstacle = obstacle.ClosestPoint(transform.position) - transform.position;
                directionToObstacle.y = 0;
                float distance = directionToObstacle.magnitude;

                if (distance < detectionRadius)
                {
                    float strength = 1 - (distance / detectionRadius);
                    avoidanceForce -= directionToObstacle.normalized * strength;
                }
            }

            return avoidanceForce.normalized;
        }

        public void MoveToNextPoint()
        {
            if (pathPoints.Count == 0) return;

            if (currentPathStrategy != null)
            {
                currentPointIndex = currentPathStrategy.GetNextPointIndex(currentPointIndex, pathPoints.Count, ref direction);
            }
        }

        public void AddPathPoint(Vector3 position, float waitTime = 0f, float interactionRadius = 1f)
        {
            PathPoint newPoint = new PathPoint
            {
                relativePosition = position - initialPosition,
                waitTime = waitTime,
                interactionRadius = interactionRadius
            };
            pathPoints.Add(newPoint);
        }

        public void ClearPath()
        {
            pathPoints.Clear();
            currentPointIndex = 0;
            direction = 1;
        }

        private void Update()
        {
            if (waitCounter > 0)
            {
                waitCounter -= Time.deltaTime;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = pointColor;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            if (pathPoints == null || pathPoints.Count == 0) return;

            Vector3 basePosition = Application.isPlaying ? initialPosition : transform.position;

            for (int i = 0; i < pathPoints.Count; i++)
            {
                var point = pathPoints[i];
                Vector3 worldPosition = basePosition + point.relativePosition;

                Gizmos.color = (i == currentPointIndex) ? Color.green : pathColor;
                Gizmos.DrawSphere(worldPosition, point.interactionRadius);

                if (i < pathPoints.Count - 1)
                {
                    Vector3 nextWorldPosition = basePosition + pathPoints[i + 1].relativePosition;
                    Gizmos.DrawLine(worldPosition, nextWorldPosition);
                }
                else if (pathfindingType != PathfindingType.OneWay)
                {
                    Vector3 firstWorldPosition = basePosition + pathPoints[0].relativePosition;
                    Gizmos.DrawLine(worldPosition, firstWorldPosition);
                }
            }
        }
    }

    public interface IPathStrategy
    {
        int GetNextPointIndex(int currentIndex, int pointCount, ref int direction);
    }

    public class SequentialPathStrategy : IPathStrategy
    {
        public int GetNextPointIndex(int currentIndex, int pointCount, ref int direction)
        {
            return (currentIndex + 1) % pointCount;
        }
    }

    public class PingPongPathStrategy : IPathStrategy
    {
        public int GetNextPointIndex(int currentIndex, int pointCount, ref int direction)
        {
            if (currentIndex == 0 || currentIndex == pointCount - 1)
            {
                direction *= -1;
            }
            return currentIndex + direction;
        }
    }

    public class RandomPathStrategy : IPathStrategy
    {
        public int GetNextPointIndex(int currentIndex, int pointCount, ref int direction)
        {
            int nextIndex;
            do
            {
                nextIndex = UnityEngine.Random.Range(0, pointCount);
            } while (nextIndex == currentIndex && pointCount > 1);
            return nextIndex;
        }
    }

    public class LoopPathStrategy : IPathStrategy
    {
        public int GetNextPointIndex(int currentIndex, int pointCount, ref int direction)
        {
            return (currentIndex + 1) % pointCount;
        }
    }

    public class OneWayPathStrategy : IPathStrategy
    {
        public int GetNextPointIndex(int currentIndex, int pointCount, ref int direction)
        {
            return currentIndex < pointCount - 1 ? currentIndex + 1 : currentIndex;
        }
    }
}