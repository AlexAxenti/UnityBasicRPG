using UnityEngine;

public class SceneTeleporter : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    [SerializeField] private string targetSpawnId;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Teleporter hit by: {other.name}");

        if (!other.CompareTag("Player"))
        {
            Debug.Log("Hit something, but it was not tagged Player.");
            return;
        }

        Debug.Log($"Loading scene: {targetSceneName} at spawn: {targetSpawnId}");

        if (SceneTransitionManager.Instance != null)
        {
            SceneTransitionManager.Instance.LoadScene(targetSceneName, targetSpawnId);
        }
        else
        {
            Debug.LogWarning("SceneTransitionManager.Instance was null.");
        }
    }
}