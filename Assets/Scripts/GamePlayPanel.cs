using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanel : MonoSingleton<GamePlayPanel>
{
    [Header("References")] [SerializeField]
    private Button throwButton;

    [SerializeField] private TextMeshProUGUI throwCountTxt;
    [SerializeField] private TextMeshProUGUI diceTotalTxt;
    [SerializeField] private TextMeshProUGUI totalTxt;

    [Header("Config")] [SerializeField] private List<DiceData> diceData;

    [Header("Debug")] [SerializeField] private
        List<SelectedNumbersData> selectedNumbersData = new();

    private bool _isActive;
    private int _throwCounter;
    private int _generalTotal;

    private void Start()
    {
        throwButton.onClick.AddListener(ThrowDice);
    }

    private void ThrowDice()
    {
        RollingDiceRoutine();
    }

    private async UniTask RollingDiceRoutine()
    {
        throwButton.interactable = false;

        for (int i = 1; i <= 20; i++)
        {
            SelectedNumbersData snd;
            if (IsRoundTarget(i, out snd))
            {
                Debug.LogWarning("Round matched: " + snd.selectedNumber);
                RollDiceToMatchedNumber(snd);
                await UniTask.Delay(500);
            }
            else
            {
                for (var q = 0; q < diceData.Count; q++)
                {
                    var dt = diceData[q];
                    var randomValue = GetRandomDiceValue();

                    dt.diceText.text = randomValue.ToString();
                    dt.diceResult = randomValue;
                    await UniTask.Delay(500);
                }

                SetDiceTexts();
            }

            SetTotalText();
            _throwCounter++;
            throwCountTxt.text = _throwCounter.ToString();
        }
    }

    private void RollDiceToMatchedNumber(SelectedNumbersData snd)
    {
        var selectedNumber = snd.selectedNumber;
        const int minValue = 1;
        const int maxValue = 6;

        bool isValid = false;


        while (!isValid)
        {
            diceData[0].diceResult = Random.Range(minValue, Mathf.Min(maxValue + 1, selectedNumber - minValue * 2 + 1));
            diceData[1].diceResult = Random.Range(minValue,
                Mathf.Min(maxValue + 1, selectedNumber - diceData[0].diceResult - minValue + 1));

            diceData[2].diceResult = selectedNumber - diceData[0].diceResult - diceData[1].diceResult;

            if (diceData[2].diceResult is >= minValue and <= maxValue)
            {
                isValid = true;
            }
            else
            {
                Debug.Log("Invalid dice result, trying again...");
            }
        }

        SetDiceTexts();
        Debug.Log(
            $"Dice Results: {diceData[0].diceResult}, {diceData[1].diceResult}, {diceData[2].diceResult} " +
            $"| TOTAL: {diceData[0].diceResult + diceData[1].diceResult + diceData[2].diceResult}");
    }

    private bool IsRoundTarget(int rollingIndex, out SelectedNumbersData _snd)
    {
        bool flag = false;
        _snd = null;

        foreach (var snd in selectedNumbersData)
        {
            if (snd.targetRound == rollingIndex)
            {
                flag = true;
                _snd = snd;
                break;
            }
        }

        return flag;
    }

    private static int GetRandomDiceValue()
    {
        return Random.Range(1, 7);
    }

    public void SetActive(List<SelectedNumbersData> numbers)
    {
        _isActive = true;
        selectedNumbersData = numbers;
    }

    private void SetDiceTexts()
    {
        foreach (var d in diceData)
        {
            d.diceText.text = d.diceResult.ToString();
        }
    }

    private void SetTotalText()
    {
        int totalValue = 0;
        foreach (var d in diceData)
        {
            totalValue += d.diceResult;
        }

        diceTotalTxt.text = totalValue.ToString();
        _generalTotal += totalValue;
        totalTxt.text = _generalTotal.ToString();
    }
}