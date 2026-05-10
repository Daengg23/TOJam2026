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
    [SerializeField] private TextMeshProUGUI finalScoreText;
    [SerializeField] private TextMeshProUGUI highscoreText;

    [SerializeField] private Image mascotImage;

    void Start()
    {
        reputationManager = GameManager.Instance.Reputation;
        reputationManager.OnReputationChanged += ReputationLossAction;

        notificationManager = GameManager.Instance.NotificationManager;
        notificationManager.OnNotificationMessage += NotificationMessageAction;
        notificationManager.OnSpriteChange += SpriteChangeAction;

        GameManager.Instance.OnScoreChanged += ScoreChangedAction;
    }

    public void ScoreChangedAction(int score)
    {
        var dahCurrentScore = GetDaysAndHours(score);
        var dahHighScore = GetDaysAndHours(GameManager.Instance.Highscore);

        scoreText.text = $"Survived: {dahCurrentScore}";
        finalScoreText.text = $"Survived:  {dahCurrentScore}";
        highscoreText.text = $"Highscore: {dahHighScore}";
    }

    public string GetDaysAndHours(int score)
    {
        int days = score/24;
        int hours = score % 24;

        string daysAndHours = $"{(days > 0 ? days+"d" : "")} {hours}h";
        return daysAndHours;
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

    public void ReputationLossAction()
    {
        reputationText.text = "" + reputationManager.CurrentReputation;
    }
}
