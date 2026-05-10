using TMPro;
using UnityEngine;

public class RepUIController : MonoBehaviour
{

    public RepArrowController RepArrowController;
    public RepBar RepBar;
    public TextMeshProUGUI TextMeshProUGUI;
    public GameObject RepBarChunkPrefab;

    private void Awake()
    {
        if (RepBarChunkPrefab.GetComponent<RepChunkAnimation>() == null) Debug.LogError($"The assigned prefab requires a {nameof(RepChunkAnimation)} component");
        if (RepBarChunkPrefab.GetComponent<RectTransform>() == null) Debug.LogError($"The assigned prefab also requires a {nameof(RectTransform)} component");
    }

    /// <summary>
    /// Instantiates an animation prefab that represents a chunk of the rep bar being removed. The animation prefab should destroy itself when the animation ends.
    /// </summary>
    /// <param name="startPercentage">The percentage that determines the starting position of the animation on the rep bar</param>
    /// <param name="endPercentage">The percentage that determines the ending position of the animation on the rep bar. Must not be greater than the previous parameter</param>
    public void AnimateRemoveRepChunk(float startPercentage, float endPercentage)
    {

        if (endPercentage > startPercentage) Debug.LogError($"{nameof(AnimateRemoveRepChunk)}: endPercentage should not be greater than StartPercentage");

        GameObject animationGO = Instantiate(RepBarChunkPrefab);
        //animationGO.transform.SetParent(RepBar.transform.parent, worldPositionStays: false);
        animationGO.transform.parent = RepBar.transform.parent;

        //we need the initial values values of the rep bar
        RepBar repBar = RepBar.GetComponent<RepBar>();
        float repBarInitialWidth = repBar.InitialWidth;
        Vector3 repBarInitialPos = repBar.InitialPos;

        Debug.Log(repBarInitialWidth);


        RepChunkAnimation chunkAnimationScript = animationGO.GetComponent<RepChunkAnimation>();
        RectTransform chunkAnimationRectTransform = animationGO.GetComponent<RectTransform>();

        //unreadable math go

        //calculate width of the animation 
        float chunkAnimationWidth = repBarInitialWidth * (startPercentage - endPercentage);
        chunkAnimationRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, chunkAnimationWidth); //set width of chunk

        float repBarLeftEdgePos = repBarInitialPos.x - (repBarInitialWidth / 2);
        float repBarRemainingRightEdgePos = repBarLeftEdgePos + (repBarInitialWidth * endPercentage);
        chunkAnimationScript.InitialPosition = new Vector2(repBarRemainingRightEdgePos + (chunkAnimationWidth / 2), repBarInitialPos.y);

        animationGO.SetActive(true);

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
