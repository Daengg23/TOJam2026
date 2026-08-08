using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    private ReputationManager reputationManager;
    private NotificationManager notificationManager;

    [SerializeField] private TextMeshProUGUI reputationText;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private Image mascotImage;

    void Start()
    {
        reputationManager = GameManager.Instance.Reputation;
        reputationManager.OnReputationLoss += ReputationLossAction;

        notificationManager = GameManager.Instance.NotificationManager;
        notificationManager.OnNotificationMessage += NotificationMessageAction;
        notificationManager.OnSpriteChange += SpriteChangeAction;

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

    public void SpriteChangeAction(Sprite sprite)
    {
        if(sprite == null) return;
        mascotImage.sprite = sprite;
    }

    public void ReputationLossAction(float amountLoss)
    {
        reputationText.text = "" + reputationManager.CurrentReputation;
    }
}
