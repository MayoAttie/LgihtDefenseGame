
using UnityEngine;
using UnityEngine.UI;
public class ExplainManager : MonoBehaviour
{
    [SerializeField] Image screenShot1;
    [SerializeField] Image screenShot2;
    [SerializeField] Text ExplainText;
    [SerializeField] Button switchButton;
    [SerializeField] Image underLine;
    [SerializeField] Transform[] ImagePos;
    int switchCount = 0;
    int switchMaxCount = 5;
    float newWidthSize = 1900f;
    void Start()
    {
        switchButton.onClick.AddListener(SwitchText);
        underLine.enabled= false;

        
    }

    // Update is called once per frame


    void SwitchText()
    {
        switch(switchCount)
        {
            case 0:
                underLine.enabled= true;
                ExplainText.text = "붉은 선으로 표시된 곳은 미니맵입니다.\n플레이어와 오브젝트, 큐브의 위치를 확인할 수 있습니다.";
                break;
            case 1:
                underLine.transform.position = ImagePos[switchCount - 1].position;
                ExplainText.text = "다음은 필드 내에 존재하는 큐브의 숫자입니다.\n20마리 이상이 필드에 있다면 게임은 패배하게 됩니다.";
                break;
            case 2:
                underLine.transform.position = ImagePos[switchCount - 1].position;
                ExplainText.text = "큐브를 파괴하면 골드를 얻을 수 있습니다.";
                break;
            case 3:
                underLine.transform.position = ImagePos[switchCount - 1].position;
                ExplainText.text = "위 버튼을 클릭하면, 상점 창이 오픈됩니다.\n골드를 소모하여, 캐릭터나 캐논을 강화할 수 있습니다.";
                break;
            case 4:
                screenShot1.enabled= false;
                screenShot2.gameObject.SetActive(true);
                underLine.enabled= false;
                ExplainText.text = "상점 창은 다음과 같이 되어있습니다. \n상점은 알트키를 눌러, 커서를 꺼낸 후에 들어올 수 있습니다.";
                break;
            case 5:
                underLine.transform.position = ImagePos[switchCount - 2].position;
                Vector2 size = underLine.rectTransform.sizeDelta;
                size.x = newWidthSize;
                underLine.rectTransform.sizeDelta = size;
                underLine.enabled = true;
                ExplainText.text = "플레이어 공격력 강화 부분입니다.\n강화할 때마다, 플레이어의 공격력이 상승되며, 요구 골드량도 증가합니다.";
                break;
            case 6:
                underLine.transform.position = ImagePos[switchCount - 2].position;
                ExplainText.text = "캐논 공격력 강화 부분입니다.\n강화할 때마다, 캐논의 공격력이 상승되며, 요구 골드량도 증가합니다.";
                break;
            case 7:
                underLine.enabled = false;
                ExplainText.text = "기본적인 게임 조작법은 WASD키와 마우스를 이용합니다.\n쉬프트를 이용하여 달리기도 가능합니다.";
                break;
            case 8:
                ExplainText.text = "마우스 오른쪽 버튼은 플레이어 공격입니다.\n마우스 왼쪽 버튼은 캐논 설치입니다.";
                break;
            case 9:
                ExplainText.text = "승리 조건은 모든 스테이지를 클리어하고 보스 큐브를 잡는 것입니다.\n보스큐브는 한 싸이클 내에 잡아야 하며, 실패하면 패배합니다.";
                break;
            case 10:
                SceneLoader._instance._LoadScene("UI_Scene");
                break;
        }

        switchCount++;
    }
}
