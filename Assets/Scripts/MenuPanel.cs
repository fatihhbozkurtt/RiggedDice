using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class MenuPanel : MonoSingleton<MenuPanel>
{
    [Header("References")] [SerializeField]
    private List<TextMeshProUGUI> selectedNumbersTxts;

    [Header("Debug")] private int _counter;
    private readonly List<int> _assignedNumbers = new();
    private bool _block;
    public void AssignNumber(int value)
    {
        if(_block) return;
        _counter++;
        _assignedNumbers.Add(value);
        selectedNumbersTxts[_counter - 1].text = value.ToString(CultureInfo.InvariantCulture);

        if (_counter < selectedNumbersTxts.Count) return;
        
        transform.GetChild(0).gameObject.SetActive(false);
        GamePlayPanel.instance.SetActive(_assignedNumbers);
        _block = true;
    }
}