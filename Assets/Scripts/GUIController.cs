using TMPro;
using UnityEngine;

public class GUIController : MonoBehaviour
{
    private ReputationManager reputationManager;
    private NotificationManager notificationManager;

    [SerializeField] private TextMeshProUGUI reputationText;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Start()
    {
        reputationManager = GameManager.Instance.Reputation;
        reputationManager.OnReputationLoss += ReputationLossAction;

        notificationManager = GameManager.Instance.NotificationManager;
        notificationManager.OnNotificationMessage += NotificationMessageAction;

        GameManager.Instance.OnScoreChanged += ScoreChangedAction;
    }

    public void ScoreChangedAction(int score)
    {
        int days = score/24;
        int hours = score % 24;

        scoreText.text = $"Survived: {(days > 0 ? days+"d" : "")} {hours}h";
    }

    public void NotificationMessageAction(string text)
    {
        notificationText.text = text;
    }

    public void ReputationLossAction(float amountLoss)
    {
        reputationText.text = "" + reputationManager.CurrentReputation;
    }
}
