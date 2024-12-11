using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Insect_VFX;

namespace Insect_VFX
{
    public class InsectEmitter : MonoBehaviour
    {
        [Header("Emission Settings")]
        [Range(1, 100)]
        public int numberOfEmissions = 5;
        public GameObject emissionObject;
        [Range(0.1f, 10f)]
        public float simulationRangeX = 1f;
        [Range(0.1f, 10f)]
        public float simulationRangeZ = 1f;
        [Range(0.5f, 5f)]
        public float simulationHeigth = 1f;

        [Header("Entities Setgings")]
        public SettingsPreset settingsPreset;
        public float moveSpeed = 1f;
        public float rotationSpeed = 15f;
        public float pauseTime = 1f;

        [Header("PathFinding Settings")]
        [System.ComponentModel.DefaultValue("Default")] public LayerMask surface_LayerMask;
        [Range(1, 30)]
        public int pathSteps = 10;  // High performance impact

        public System.Action<int> OnEmittedObjectStart;
        public System.Action<int> OnEmittedObjectPaused;
        public System.Action<int> OnEmittedObjectResume;
        public System.Action<int> OnEmittedObjectEnd;
        public System.Action<int> OnEmittedObjectDestroyed;

        private Queue<GameObject> objectPool;
        GameObject[] emittedObjects = new GameObject[0];

        Vector3[] targetPositions;
        Vector3[][] pathPoints;
        float[] pauseTimers;

        Vector3 spawnOrigin;
        Transform entityParent = null;

        bool loopMode = false;
        bool dieOnFinishPath = false;
        bool debugMode = false;

        public float xOffset = 0f;
        public float yOffset = 0f; // z좌표 오프셋 추가
        public float zOffset = 0f;

        #region Public Methods

        public void StartSimulation()
        {
            emittedObjects = new GameObject[numberOfEmissions];
            targetPositions = new Vector3[numberOfEmissions];
            pauseTimers = new float[numberOfEmissions];
            pathPoints = new Vector3[numberOfEmissions][];

            dieOnFinishPath = false;
            loopMode = false;

            RaycastHit hit;
            Physics.Raycast(transform.position, -transform.up, out hit, 100, surface_LayerMask);
            //spawnOrigin = hit.point + Vector3.down * 0.25f;
            spawnOrigin = transform.position;
            for (int i = 0; i < numberOfEmissions; i++)
            {
                SpawnEntity(i, /*hit.point*/spawnOrigin, Quaternion.FromToRotation(transform.up, hit.normal));
            }
        }

        public void StartSimulation(float simulationTime)
        {
            StartSimulation();
            Invoke("EndSimulation", simulationTime);
        }

        public void RestartSimulation()
        {
            dieOnFinishPath = false;

            for (int i = 0; i < emittedObjects.Length; i++)
            {
                DeactivateEntity(i);
            }

            StartSimulation();
        }

        public void EndSimulation()
        {
            dieOnFinishPath = true;
            loopMode = false;

            for (int i = 0; i < emittedObjects.Length; i++)
            {
                if (pathPoints[i] != null && pathPoints[i].Length > 0)
                {
                    pathPoints[i][pathPoints[i].Length - 1] += Vector3.down;
                }
                else
                {
                    OnEntityDestroyed(i);
                }
            }

            OnEmittedObjectDestroyed -= OnEntityDestroyed;
        }

        public void StartLoopSimulation()
        {
            for (int i = 0; i < emittedObjects.Length; i++)
            {
                DeactivateEntity(i);
            }

            emittedObjects = new GameObject[numberOfEmissions];
            targetPositions = new Vector3[numberOfEmissions];
            pauseTimers = new float[numberOfEmissions];
            pathPoints = new Vector3[numberOfEmissions][];

            RaycastHit hit;
            Physics.Raycast(transform.position, -transform.up, out hit, 100, surface_LayerMask);
            spawnOrigin = hit.point + Vector3.down * 0.25f;

            for (int i = 0; i < numberOfEmissions; i++)
            {
                SpawnEntity(i, spawnOrigin, Quaternion.FromToRotation(transform.up, hit.normal));
            }

            loopMode = true;

            OnEmittedObjectDestroyed -= OnEntityDestroyed;
            OnEmittedObjectDestroyed += OnEntityDestroyed;
        }

        public void SetNumberOfEmissions(int newNumber)
        {
            if (newNumber < 1) return;

            // Cambiar el tama? de los arrays
            GameObject[] newEmittedObjects = new GameObject[newNumber];
            Vector3[] newTargetPositions = new Vector3[newNumber];
            float[] newPauseTimers = new float[newNumber];
            Vector3[][] newPathPoints = new Vector3[newNumber][];

            // Copiar datos existentes si el nuevo tama? es mayor al antiguo
            int minLength = Mathf.Min(emittedObjects.Length, newNumber);
            for (int i = 0; i < minLength; i++)
            {
                newEmittedObjects[i] = emittedObjects[i];
                newTargetPositions[i] = targetPositions[i];
                newPauseTimers[i] = pauseTimers[i];
                newPathPoints[i] = pathPoints[i];
            }

            // Asignar los nuevos arrays a los existentes
            emittedObjects = newEmittedObjects;
            targetPositions = newTargetPositions;
            pauseTimers = newPauseTimers;
            pathPoints = newPathPoints;

            // Ajustar el n?ero de emisiones
            numberOfEmissions = newNumber;

            // Reiniciar la simulaci? para aplicar los cambios
            RestartSimulation();
        }

        public GameObject GetEntityByIndex(int index)
        {
            return emittedObjects[index];
        }

        #endregion

        void Awake()
        {
            if (entityParent == null)
            {
                if (GameObject.Find("_EntityParent") != null)
                    entityParent = GameObject.Find("_EntityParent").transform;
                else
                    entityParent = new GameObject("_EntityParent").transform;
            }

            InitializePooling();
        }

        void Update()
        {
            if (emittedObjects.Length == 0)
                return;

            for (int i = 0; i < numberOfEmissions; i++)
            {
                if (emittedObjects[i] == null || pathPoints[i] == null || pathPoints[i].Length == 0)
                    continue;

                if (pauseTimers[i] > 0)
                {
                    pauseTimers[i] -= Time.deltaTime;

                    if (pauseTimers[i] <= 0)
                    {
                        OnEmittedObjectResume?.Invoke(i);
                    }
                    continue;
                }

                Vector3 nextPosition = pathPoints[i][0];
                Vector3 direction = (nextPosition - emittedObjects[i].transform.position).normalized;

                Quaternion lookRot = Quaternion.LookRotation(direction);
                emittedObjects[i].transform.rotation = Quaternion.Slerp(
                    emittedObjects[i].transform.rotation,
                    lookRot,
                    Time.deltaTime * rotationSpeed
                );

                emittedObjects[i].transform.position += emittedObjects[i].transform.forward * moveSpeed * Time.deltaTime;

                if (Vector3.Distance(emittedObjects[i].transform.position, nextPosition) < 0.1f)
                {
                    pathPoints[i] = RemoveReachedPoint(pathPoints[i]);
                    if (pathPoints[i].Length == 0)
                    {
                        OnEmittedObjectEnd?.Invoke(i);

                        if (dieOnFinishPath || loopMode)
                        {
                            DeactivateEntity(i);
                            OnEmittedObjectDestroyed?.Invoke(i);
                        }
                        else
                        {
                            targetPositions[i] = GetRandomPosition(transform.position);

                            pauseTimers[i] = GetPauseTime();
                            OnEmittedObjectPaused?.Invoke(i);   // Event

                            pathPoints[i] = CalculatePath(emittedObjects[i].transform.position, targetPositions[i]);
                        }
                    }
                }
            }
        }

        void InitializePooling()
        {
            objectPool = new Queue<GameObject>();
            for (int i = 0; i < numberOfEmissions; i++)
            {
                GameObject obj = Instantiate(emissionObject, entityParent);
                obj.transform.position = transform.position;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
        }

        void SpawnEntity(int index, Vector3 position, Quaternion rotation)
        {
            GameObject entity;

            if (objectPool.Count > 0)
            {
                entity = objectPool.Dequeue();
            }
            else
            {
                entity = Instantiate(emissionObject, entityParent);
            }

            position.x += xOffset;
            position.y += yOffset;
            position.z += zOffset;

            entity.transform.position = position;
            entity.transform.rotation = rotation;
            entity.SetActive(true);

            emittedObjects[index] = entity;
            targetPositions[index] = GetRandomPosition(position);
            pauseTimers[index] = GetPauseTime();
            pathPoints[index] = CalculatePath(entity.transform.position, targetPositions[index]);

            OnEmittedObjectStart?.Invoke(index);
        }

        void DeactivateEntity(int index)
        {
            if (emittedObjects[index] != null)
            {
                emittedObjects[index].transform.position = spawnOrigin + Vector3.down;
                emittedObjects[index].SetActive(false);
                objectPool.Enqueue(emittedObjects[index]);
                emittedObjects[index] = null;
            }
        }

        void OnEntityDestroyed(int index)
        {
            SpawnEntity(index, spawnOrigin, Quaternion.identity);
        }

        Vector3 GetRandomPosition(Vector3 origin)
        {
            float rangeX = simulationRangeX * 0.5f;
            float rangeZ = simulationRangeZ * 0.5f;
            Vector3 newPos = origin + new Vector3(
                Random.Range(-rangeX, rangeX),
                Random.Range(-simulationHeigth * 0.5f, simulationHeigth * 0.5f),
                Random.Range(-rangeZ, rangeZ)
            );

            Vector3 rayOrigin = newPos;
            rayOrigin.y = transform.position.y;

            Ray ray = new Ray(rayOrigin, -Vector3.up);
            Physics.Raycast(ray, out RaycastHit hitInfo, 100, surface_LayerMask);
            newPos.y = hitInfo.point.y + 0.01f;

            return newPos;
        }

        Vector3[] CalculatePath(Vector3 start, Vector3 end)
        {
            Vector3[] points = new Vector3[pathSteps];
            Vector3 direction = (end - start) / pathSteps;

            for (int i = 0; i < pathSteps; i++)
            {
                Vector3 point = start + direction * i;

                if (Physics.Raycast(point + transform.up, -transform.up, out RaycastHit hitInfo, 100, surface_LayerMask))
                {
                    point.y = hitInfo.point.y + 0.01f;
                }

                if (loopMode)
                {
                    if (i == pathSteps - 1)
                        point.y -= .25f;
                    else if (i == 1)
                        point.y += .05f;
                }

                points[i] = point;
            }

            return points;
        }

        Vector3[] RemoveReachedPoint(Vector3[] path)
        {
            if (path.Length <= 1) return new Vector3[0];

            Vector3[] newPath = new Vector3[path.Length - 1];
            for (int i = 1; i < path.Length; i++)
            {
                newPath[i - 1] = path[i];
            }
            return newPath;
        }

        float GetPauseTime()
        {
            return pauseTime * Random.Range(0.1f, 1f);
        }

        [ContextMenu("Toggle DebugMode")]
        void ToggleDebugMode()
        {
            debugMode = !debugMode;
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(
               transform.position,
               new Vector3(simulationRangeX, simulationHeigth, simulationRangeZ)
           );

            if (Physics.Raycast(transform.position + (Vector3.up * (simulationHeigth * 0.5f)), Vector3.down, out RaycastHit hit, simulationHeigth, surface_LayerMask))
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(hit.point, 0.01f);
                Gizmos.DrawWireCube(
                   hit.point - (Vector3.up * hit.distance * 0.5f),
                   new Vector3(simulationRangeX, hit.distance, simulationRangeZ)
               );
            }

            if (!debugMode)
                return;

            if (emittedObjects.Length > 0)
            {
                for (int i = 0; i < emittedObjects.Length; i++)
                {
                    if (pathPoints[i] != null)
                    {
                        Gizmos.color = UnityEngine.Color.blue;
                        for (int j = 0; j < pathPoints[i].Length - 1; j++)
                        {
                            Gizmos.DrawLine(pathPoints[i][j], pathPoints[i][j + 1]);
                        }
                    }
                }
            }
        }
    }

    public enum SettingsPreset
    {
        Custom,
        Cockrach,
        Spider
    }

    [CustomEditor(typeof(InsectEmitter))]
    public class InsectEmitterEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            InsectEmitter emitter = (InsectEmitter)target;

            switch (emitter.settingsPreset)
            {
                case SettingsPreset.Custom:
                    break;
                case SettingsPreset.Cockrach:
                    emitter.moveSpeed = 0.75f;
                    emitter.rotationSpeed = 15f;
                    emitter.pauseTime = 2f;
                    break;
                case SettingsPreset.Spider:
                    emitter.moveSpeed = 0.5f;
                    emitter.rotationSpeed = 15f;
                    emitter.pauseTime = 5f;
                    break;
            }

            DrawDefaultInspector();

            GUI.enabled = Application.isPlaying;

            if (GUILayout.Button("Start Simulation"))
            {
                emitter.StartSimulation();
            }

            if (GUILayout.Button("Restart Simulation"))
            {
                emitter.RestartSimulation();
            }

            if (GUILayout.Button("End Simulation"))
            {
                emitter.EndSimulation();
            }

            GUILayout.Space(16);

            if (GUILayout.Button("Start Loop Simulation [Beta]"))
            {
                emitter.StartLoopSimulation();
            }

            GUI.enabled = true;
        }
    }
}