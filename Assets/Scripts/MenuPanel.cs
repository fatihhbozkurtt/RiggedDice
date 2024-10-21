using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class MenuPanel : MonoSingleton<MenuPanel>
{
    [Header("References")] [SerializeField]
    private List<TextMeshProUGUI> selectedNumbersTxts;

    [Header("Debug")] [SerializeField] private List<SelectedNumbersData>
        selectedNumbersData = new();

    private int _counter;
    private bool _block;

    public void AssignNumber(int value)
    {
        if (_block) return;
        _counter++;
        SelectedNumbersData snd = selectedNumbersData[_counter - 1];
        snd.selectedNumber = value;
        snd.targetRound = GetTargetRound(snd);
        selectedNumbersTxts[_counter - 1].text = value.ToString(CultureInfo.InvariantCulture);

        if (_counter < selectedNumbersTxts.Count) return;

        transform.GetChild(0).gameObject.SetActive(false);
        GamePlayPanel.instance.SetActive(selectedNumbersData);
        _block = true;
    }

    private int GetTargetRound(SelectedNumbersData snd)
    {
        int targetRound = 0;
        bool hasSame = true;

        while (hasSame)
        {
            snd.targetRound = Random.Range(snd.targetRoundRange.x, snd.targetRoundRange.y);
            targetRound = snd.targetRound;
            hasSame = false;

            foreach (var data in selectedNumbersData)
            {
                // Skip the check for the same targetRoundRange
                if (data.targetRoundRange == snd.targetRoundRange) continue;

                // If there's a match, set hasSame to true and retry
                if (data.targetRound == targetRound)
                {
                    hasSame = true;
                    break;
                }
            }
        }

        return targetRound;
    }
}