using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnControl : MonoBehaviour
{
    public static PlayerSpawnControl Instance { get; private set;}


    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


    }
    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(!string.IsNullOrEmpty(SceneSpawn.TargetSpawn))
        {
            GameObject spawnPoint = GameObject.Find(SceneSpawn.TargetSpawn);

            if (spawnPoint != null)
            {
                // Přesune hráče na pozici tohoto bodu
                transform.position = spawnPoint.transform.position;
            }
            else
            {
                Debug.LogWarning("Spawn point " + SceneSpawn.TargetSpawn + " nebyl nalezen!");
            }
        }
    }


}
