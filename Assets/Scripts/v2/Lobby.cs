using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("Test2");
    }
}
