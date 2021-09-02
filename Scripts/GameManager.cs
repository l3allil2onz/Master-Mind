using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField[] playerRateInput;
    public GameObject resultPrefab, scoredisplayPanel,congratulationsPanel;
    public GameObject randomText, recommendText, congratulationsText;
    public GameObject randomPanel, playerPanel;
    private RectTransform scoreDisplayRect;
    public Color32[] color;

    public List<int> numberOfRandomList = new List<int>();
    public List<int> playerRateInputList = new List<int>();
    [SerializeField]
    private float addScorePanelPosY;
    [SerializeField]
    private int maxRandomNumber,minRanddomNumber,countOfRandom;
    public int guessCount;


    private void Awake()
    {
        scoreDisplayRect = scoredisplayPanel.GetComponent<RectTransform>();
        for(int i = 0;i < 4;i++)
        {
            playerRateInput[i] = GameObject.Find("Input"+i).GetComponent<TMP_InputField>();
        }

    }

    void Start()
    {
        StartSetting();
        SettingScoreDisplayAtStart();
    }

    public void AcceptButton()
    {
        if (NullInputChecker() == 0)
        {
            if (scoredisplayPanel.transform.childCount >= 4)
            {
                InputContainer();
                AddScorePanelPositionY();
                SettingResultPrefab();
                ResultChecker();
                ClearInputList();
            }
            else
            {
                InputContainer();
                SettingResultPrefab();
                ResultChecker();
                ClearInputList();
            }
        }
    }

    public void PlayAgainButton()
    {
        SceneManager.LoadScene("MasterMind");
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }

    public void RandomButton()
    {
        SwitchToPlayPanel();
        RandomSet();
    }

    public void ResetInputButton()
    {
        for(int i = 0; i < playerRateInput.Length;i++)
        {
            playerRateInput[i].text = null;
        }
    }

    private void RandomSet()
    {
        int thisNumber;
        int index;

        for (int i = 0; i < countOfRandom; i++)
        {
            do
            {
                thisNumber = Random.Range(minRanddomNumber, maxRandomNumber + 1);
                int indexInner = numberOfRandomList.FindIndex(x => x == thisNumber);
                index = indexInner;
            }
            while (index != -1);
            numberOfRandomList.Add(thisNumber);
        }
    }

    private void InputContainer()
    {
        foreach (TMP_InputField input in playerRateInput)
        {
            string thisInput = input.text;
            playerRateInputList.Add(int.Parse(thisInput.ToString()));
        }
    }

    private int NullInputChecker()
    {
        for (int i = 0; i < playerRateInput.Length; i++)
        {
            if (playerRateInput[i].text == "")
            {
                return 1;
            }
        }
        return 0;
    }

    private void ResultChecker()
    {
        GameObject thisPrefab;
        int correctValue,correctIndex;

        thisPrefab = GameObject.Find("#" + guessCount).gameObject;
        correctValue = CorrectValueCountChecker();
        correctIndex = CorrectIndexCountChecker();
        thisPrefab.transform.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = correctValue.ToString();
        thisPrefab.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>().text = correctIndex.ToString();
        /*if (correctValue >= numberOfRandomList.Count)
            thisPrefab.transform.GetChild(6).GetComponent<Image>().color = color[1];
        else
            thisPrefab.transform.GetChild(6).GetComponent<Image>().color = color[0];

        if (correctIndex >= numberOfRandomList.Count)
            thisPrefab.transform.GetChild(7).GetComponent<Image>().color = color[1];
        else
            thisPrefab.transform.GetChild(7).GetComponent<Image>().color = color[0];*/

        if(correctValue >= 4 && correctIndex >= 4)
        {
            thisPrefab.transform.GetChild(8).gameObject.SetActive(true);
            congratulationsText.GetComponent<TextMeshProUGUI>().text = "Congratulations !!!!" + "\n" +"You guessed it "+ guessCount+" times,"+
                                                                       " Want to play again ?";
            SwitchToCongratulationsPanel();
        }
    }

    private int CorrectIndexCountChecker()
    {
        GameObject thisPrefab;
        int correctCount = 0;

        thisPrefab = GameObject.Find("#" + guessCount).gameObject;
        for (int i = 0; i < playerRateInputList.Count; i++)
        {
            if (playerRateInputList[i] == numberOfRandomList[i])
            {
                correctCount++;
                thisPrefab.transform.GetChild(i + 1).GetComponent<Image>().color = color[1];
            }
            else
            {
                thisPrefab.transform.GetChild(i + 1).GetComponent<Image>().color = color[0];
            }
        }
        if(correctCount > 0)
        {
            return correctCount;
        }
        return 0;
    }

    private int CorrectValueCountChecker()
    {
        int correctCount = 0;

        for (int i = 0; i < playerRateInputList.Count; i++)
        {
            int index = playerRateInputList.IndexOf(numberOfRandomList[i]);
            if(index != -1)
            {
                correctCount++;
            }
            print(correctCount);
        }
        if (correctCount > 0)
        {
            return correctCount;
        }
        return 0;
    }

    private void SettingResultPrefab()
    {
        guessCount++;

        GameObject thisPrefab = Instantiate(resultPrefab, scoredisplayPanel.transform);
        thisPrefab.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "#" + guessCount;
        thisPrefab.transform.name = "#"+ guessCount;
        for (int i = 0; i < playerRateInputList.Count; i++)
        {
            thisPrefab.transform.GetChild(i + 1).GetChild(0).GetComponent<TextMeshProUGUI>().text = playerRateInputList[i].ToString();
        }
    }

    private void AddScorePanelPositionY()
    {
        scoreDisplayRect.localPosition = new Vector3(scoreDisplayRect.localPosition.x,
                                            scoreDisplayRect.localPosition.y + addScorePanelPosY,
                                            scoreDisplayRect.localPosition.z);
        scoreDisplayRect.sizeDelta = new Vector2(scoreDisplayRect.sizeDelta.x,
                                                scoreDisplayRect.sizeDelta.y + addScorePanelPosY);
    }

    private void SettingScoreDisplayAtStart()
    {
        scoreDisplayRect.localPosition = new Vector3(scoreDisplayRect.localPosition.x,
                                                    0f,
                                                    scoreDisplayRect.localPosition.z);
        scoreDisplayRect.sizeDelta = new Vector2(scoreDisplayRect.sizeDelta.x,
                                                222f);
    }

    private void ClearInputList()
    {
        playerRateInputList.Clear();
    }

    private void SwitchToPlayPanel()
    {
        randomText.SetActive(false);
        recommendText.SetActive(true);
        randomPanel.SetActive(false);
        playerPanel.SetActive(true);
    }

    private void SwitchToCongratulationsPanel()
    {
        randomText.SetActive(false);
        recommendText.SetActive(false);
        congratulationsText.SetActive(true);
        randomPanel.SetActive(false);
        playerPanel.SetActive(false);
        congratulationsPanel.SetActive(true);
    }

    private void StartSetting()
    {
        guessCount = 0;
        randomPanel.SetActive(true);
        playerPanel.SetActive(false);
    }
}
