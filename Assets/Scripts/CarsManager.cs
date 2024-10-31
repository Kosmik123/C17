using Bipolar;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class CarsManager : MonoBehaviour
{
    [System.Serializable]
    public class Road
    {
        [field: SerializeField]
        public bool Enabled { get; private set; } = true;
        [field: SerializeField]
        public Vector3 Start { get; private set; }
        [field: SerializeField]
        public Vector3 End { get; private set; }

        private float length = -1;
        public float Length
        {
            get
            {
                if (length < 0)
                    length = Vector3.Distance(Start, End);
                return length;
            }
        }
    }
    
    [System.Serializable]
    public struct CarState
    {
        [SerializeField, Range(0, 1)]
        private float progress;
        public float Progress
        {
            get => progress;
            set
            {
                progress = Mathf.Clamp01(value);
                Transform.position = Vector3.Lerp(Road.Start, Road.End, progress);
            }
        }
         
        [field: SerializeField]
        public Road Road { get; private set; }

        [field: SerializeField]
        public Transform Transform { get; private set; }

        public CarState(Transform transform, Road road)
        {
            progress = 0;
            Transform = transform;
            Road = road;
        }
    }

    [SerializeField]
    private GameObject[] cars;
    [SerializeField]
    private Road[] roads;

    private Road[] enabledRoads;
    private Road[] disabledRoads;

    [Space]
    [SerializeField]
    private float minSpawnInterval;
    [SerializeField]
    private float maxSpawnInterval;

    [Space]
    [SerializeField]
    private float moveSpeed;

    [Space]
    [SerializeField, ReadOnly]
    private List<CarState> spawnedCars = new List<CarState>();

    private void OnEnable()
    {
        SpawnCar();
    }
    
    private void SpawnCar()
    {
        var road = GetRoad();
        var car = Instantiate(cars.GetRandom(), transform).transform;
        car.position = road.Start;
        car.forward = road.End - road.Start;
        spawnedCars.Add(new CarState(car, road));
        Invoke(nameof(SpawnCar), GetSpawnInterval());
    }

    private void Update()
    {
        for (int i = 0; i < spawnedCars.Count; i++)
        {
            var carState = spawnedCars[i];
            carState.Progress += moveSpeed / carState.Road.Length * Time.deltaTime;
            spawnedCars[i] = carState;  
        }
            
        for (int i = spawnedCars.Count - 1; i >= 0; i--)
        {
            if (spawnedCars[i].Progress >= 1)
            {
                RemoveCar(i);
            }
        }
    }

    private void RemoveCar(int i)
    {
        Destroy(spawnedCars[i].Transform.gameObject);
        spawnedCars.RemoveAt(i);
    }

    public Road GetRoad() => roads.GetRandom();

    public float GetSpawnInterval() => Random.Range(minSpawnInterval, maxSpawnInterval);

    private void OnDrawGizmosSelected()
    {
        foreach (var road in roads)
        {
            Gizmos.DrawCube(road.Start, Vector3.one);
            Gizmos.DrawLine(road.Start, road.End);
            Gizmos.DrawSphere(road.End, 1);
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
        for (int i = spawnedCars.Count - 1; i >= 0; i--)
        {
            RemoveCar(i);
        }
    }

    private void OnValidate()
    {
        
    }
}
