using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CheckpointFlag : MonoBehaviour
{
    [SerializeField] private string checkpointID = "Flag1";

    private PlayerStats _stats;
    private bool _isSaved = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isSaved) return;
        if (other.CompareTag("Player"))
        {
            _stats = other.GetComponent<PlayerStats>();
            gameObject.transform.rotation = new Quaternion(0,180,0,0);
            AudioManager.Instance.Play("Checkpoint");
            SaveCheckpoint(other.transform.position);
            //_stats.SaveCoinsCheckpoint();
        }
    }

    private void SaveCheckpoint(Vector3 playerPosition)
    {
        CheckpointData data = new CheckpointData
        {
            checkpointID = checkpointID,
            timestamp = System.DateTime.Now,
            sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            playerPosition = playerPosition,
            hp = _stats.Health,
            //timer forse no
            collectedCoins = _stats.Coins,
            collectedGems = _stats.CollectedGems
                            .Select(g => g.ToString()) 
                            .ToList()
        };

        SaveSystem.SaveCheckpoint(data);
        _stats.ClearRecentPickups();
        _isSaved = true;
    }
}
