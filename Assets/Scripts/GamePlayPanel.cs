using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanel : MonoSingleton<GamePlayPanel>
{
    [Header("References")] [SerializeField]
    private Button throwButton;

    [SerializeField] private TextMeshProUGUI throwCountTxt;
    [SerializeField] private TextMeshProUGUI diceTotalTxt;
    [SerializeField] private List<TextMeshProUGUI> diceTexts;

    [Header("Debug")] [SerializeField] private List<int> assignedNumbers = new();
    private bool _isActive;
    private int _throwCounter;

    private void Start()
    {
        throwButton.onClick.AddListener(ThrowDice);
    }

    private void ThrowDice()
    {
        throwButton.interactable = false;

        IEnumerator Routine()
        {
            for (int i = 0; i < 20; i++)
            {
                int totalRoundValue = 0;
                foreach (var diceText in diceTexts)
                {
                    var randomValue = GetRandomDiceValue();
                    diceText.text = randomValue.ToString();
                    totalRoundValue += randomValue;
                    yield return new WaitForSeconds(0.5f);
                }

                diceTotalTxt.text = totalRoundValue.ToString();
                _throwCounter++;
                throwCountTxt.text = _throwCounter.ToString();
            }
        }

        StartCoroutine(Routine());
    }

    private int GetRandomDiceValue()
    {
        return Random.Range(1, 7);
    }

    public void SetActive(List<int> numbers)
    {
        _isActive = true;
        assignedNumbers = numbers;
    }
}