using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] players;

    public void CheckWinState()
    {
        var aliveCount = players.Count(player => player.activeSelf);
        if (aliveCount <= 1)
            Invoke(nameof(StartNewRound), 3f);
    }

    private void StartNewRound()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}