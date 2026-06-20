using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    private string pendingSpawnId;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string sceneName, string spawnId)
    {
        pendingSpawnId = spawnId;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.IsNullOrEmpty(pendingSpawnId))
            return;

        SceneSpawnPoint[] spawnPoints = FindObjectsByType<SceneSpawnPoint>();

        foreach (SceneSpawnPoint spawnPoint in spawnPoints)
        {
            if (spawnPoint.SpawnId == pendingSpawnId)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    CharacterController controller = player.GetComponent<CharacterController>();

                    if (controller != null)
                        controller.enabled = false;

                    player.transform.position = spawnPoint.transform.position;
                    player.transform.rotation = spawnPoint.transform.rotation;

                    if (controller != null)
                        controller.enabled = true;
                }

                break;
            }
        }

        pendingSpawnId = null;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}