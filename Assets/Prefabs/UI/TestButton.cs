using UnityEngine;
using UnityEngine.UI;

public class TestButton : MonoBehaviour
{
    public Button yourButton;
    public GameObject GameObject;

    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Debug.Log("You have clicked the button!");

        this.GameObject.GetComponent<RepUIController>().AnimateRemoveRepChunk(1f, 0.7f);
    }
}
