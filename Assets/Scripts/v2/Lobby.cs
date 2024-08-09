using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("Test2");
    }

    public void RestartGame()
    {
        // 현재 활성화된 씬의 이름을 가져와서 다시 로드
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Time.timeScale = 1;
    }
}
