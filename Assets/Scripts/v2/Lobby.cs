using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    public Toggle easyToggle;
    public Toggle hardToggle;

    //난이도 설정
    private bool isEasy;

    public void GameStart()
    {
        if (isEasy)
            SceneManager.LoadScene("EasyRank");
        else
            SceneManager.LoadScene("Test2");
    }

    public void RestartGame()
    {
        // 현재 활성화된 씬의 이름을 가져와서 다시 로드
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        Time.timeScale = 1;
    }

    void Start()
    {
        isEasy = true;
        // 각각의 토글에 이벤트를 추가합니다.
        if(easyToggle != null && hardToggle != null)
        {
            easyToggle.onValueChanged.AddListener(OnEasyToggleChanged);
            hardToggle.onValueChanged.AddListener(OnHardToggleChanged);
        }
    }

    // Easy 토글이 변경될 때 호출되는 함수
    void OnEasyToggleChanged(bool isOn)
    {
        if (isOn)
        {
            hardToggle.isOn = false; // Easy가 켜지면 Hard를 끕니다.
            isEasy = true;
        }
        else
        {
            hardToggle.isOn = true; // Easy가 꺼지면 Hard를 켭니다.
            isEasy = false;
        }
    }

    // Hard 토글이 변경될 때 호출되는 함수
    void OnHardToggleChanged(bool isOn)
    {
        if (isOn)
        {
            easyToggle.isOn = false; // Hard가 켜지면 Easy를 끕니다.
            isEasy = false;
        }
        else
        {
            easyToggle.isOn = true; // Hard가 꺼지면 Easy를 켭니다.
            isEasy = true;
        }
    }
}
