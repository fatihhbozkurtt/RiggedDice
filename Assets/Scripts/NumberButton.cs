using UnityEngine;
using UnityEngine.UI;

public class NumberButton : MonoBehaviour
{
    [SerializeField] public int numberValue;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener
            (() => MenuPanel.instance.AssignNumber(numberValue));
    }
}