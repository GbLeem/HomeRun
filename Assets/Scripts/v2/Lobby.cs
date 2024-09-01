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
        //난이도에 따라 다른 Scene 로드
        if (isEasy)
            SceneManager.LoadScene("EasyRank");
        else
            SceneManager.LoadScene("HardRank");
    }

    public void RestartGame()
    {                
        //게임을 다시 시작할때는 Lobby Scene 부터 시작
        SceneManager.LoadScene("Lobby");
        Time.timeScale = 1;
    }

    void Start()
    {
        isEasy = true;
        // 각각의 토글에 이벤트를 추가
        if(easyToggle != null && hardToggle != null)
        {
            easyToggle.onValueChanged.AddListener(OnEasyToggleChanged);
            hardToggle.onValueChanged.AddListener(OnHardToggleChanged);
        }
    }

    //하나의 난이도를 선택하면 다른 난이도 선택 못하도록 함수 구현
    void OnEasyToggleChanged(bool isOn)
    {
        if (isOn)
        {
            hardToggle.isOn = false;
            isEasy = true;
        }
        else
        {
            hardToggle.isOn = true;
            isEasy = false;
        }
    }

    void OnHardToggleChanged(bool isOn)
    {
        if (isOn)
        {
            easyToggle.isOn = false;
            isEasy = false;
        }
        else
        {
            easyToggle.isOn = true;
            isEasy = true;
        }
    }
}
