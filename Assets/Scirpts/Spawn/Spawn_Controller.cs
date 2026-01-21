using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Controller : MonoBehaviour
{
    [Header("Obstaculos")]
    [SerializeField]GameObject[] _obstacuñePrefab;
    [SerializeField]int _amountPerPrefab = 2;
    [SerializeField] float _spawnInterval = 2f;

    List<GameObject> _obstacleList = new List<GameObject>();

    int _nextObstacleIndex = 0;

    Coroutine _spawnCoroutine;

    void SuscriptionBySignals()
    {
        Main_Manager.Instance.Game_Manager.onStateGameCharged += HandleSpawnState;
    }

    void UnSuscriptionBySignals()
    {
        Main_Manager.Instance.Game_Manager.onStateGameCharged -= HandleSpawnState;
    }

    private void OnEnable()
    {
        SuscriptionBySignals();
    }

    private void OnDisable()
    {
        UnSuscriptionBySignals();
    }

    private void Awake()
    {
        InitialicePolling();
    }


    void HandleSpawnState(GameState state)
    {
        switch(state)
        {
            case GameState.Playing:
            case GameState.Returning:
                if (_spawnCoroutine == null)
                {
                    _spawnCoroutine = StartCoroutine(SpawnRoutine());
                }
                break;
            case GameState.Pause:
            case GameState.GameOver:
                if (_spawnCoroutine != null)
                {
                    StopCoroutine(_spawnCoroutine);
                    _spawnCoroutine = null;
                }
                break;
        }
    }


    void InitialicePolling()
    {
        _obstacleList.Clear();
        // Recorremos cada TIPO de prefab (A, B, C...)
        foreach (GameObject prefab in _obstacuñePrefab)
        {
            // Creamos las copias necesarias de ESE tipo
            for (int i = 0; i < _amountPerPrefab; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                _obstacleList.Add(obj);
            }
        }

        // 2. Barajamos el mazo inicial para que no salgan en orden (A,A,B,B,C,C...)
        ShufflePool();
    }

    GameObject GetObstcuale()
    {
        // Intentamos encontrar uno libre recorriendo la lista una vez completa
        for (int i = 0; i < _obstacleList.Count; i++)
        {
            // Calculamos el índice actual usando módulo (%) para dar la vuelta si llegamos al final
            int indexToCheck = (_nextObstacleIndex + i) % _obstacleList.Count;

            // Si encontramos uno inactivo, ES EL ELEGIDO
            if (!_obstacleList[indexToCheck].activeInHierarchy)
            {
                // Actualizamos el puntero para la próxima vez (apuntamos al siguiente)
                _nextObstacleIndex = (indexToCheck + 1) % _obstacleList.Count;

                // Si dimos la vuelta completa (llegamos al índice 0), volvemos a barajar para más variedad
                if (_nextObstacleIndex == 0)
                {
                    ShufflePool();
                }

                return _obstacleList[indexToCheck];
            }
        }

        // Si todos están activos (muy raro con 2 copias), devolvemos null
        return null;
    }

    void StartPolling()
    {
        GameObject readyObstucule = GetObstcuale();

        if (readyObstucule != null)
        {
            // Movemos el objeto a la posición del Spawner antes de activarlo
            readyObstucule.transform.position = transform.position;
            readyObstucule.SetActive(true);

            // Esperamos el tiempo definido (Respeta la pausa automáticamente si usas TimeScale, 
            // pero como tu Pause pone TimeScale = 0, esto se congela solo. ¡Perfecto!)
        }
    }

    // Método para barajar la lista de referencias
    void ShufflePool()
    {
        int n = _obstacleList.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            // Intercambiamos las posiciones en la lista
            GameObject value = _obstacleList[k];
            _obstacleList[k] = _obstacleList[n];
            _obstacleList[n] = value;
        }
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            StartPolling();
            yield return new WaitForSeconds(_spawnInterval);
        }

    }
}
