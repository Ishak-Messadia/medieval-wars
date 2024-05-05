using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InfoCardController : MonoBehaviour
{
    private static InfoCardController instance;
    public static InfoCardController Instance
    {
        get
        {
            // Lazy initialization
            if (instance == null)
            {
                // Check if an instance of UnitController exists in the scene
                instance = FindObjectOfType<InfoCardController>();

                // If not found, create a new GameObject with UnitController attached
                if (instance == null)
                {
                    GameObject obj = new GameObject("InfoCardController");
                    instance = obj.AddComponent<InfoCardController>();
                }
            }
            return instance;
        }
    }


    public GameObject card;
    public CanvasGroup canvasGroup;
    private RectTransform canvasRect;


    // Card dimensions
    public float cardHeight;
    public float cardWidth;


    // x coordinate of the card position in both right and left sides
    public float XCardRightPosition;
    public float XCardLeftPosition;

    // y coordinate of the card position 
    public float YcardPosition;



    // initial x coordinate of the card position in both right and left sides , (where to start the animation)
    public float initialXCardRightPosition;
    public float initialXCardLeftPosition;
    public float initialYCardPosition;




    // position of the card in both right and left sides
    Vector3 RightCardPosition;
    Vector3 LeftCardPosition;
    // Initial position of the card in both right and left sides
    Vector3 InitialRightPositionOfTheCard;
    Vector3 InitialLeftPositionOfTheCard;

    // Vector3 rightCardPosition = new Vector3(637.5f, -330, 0);


    public float HideAnimationDuration; // Adjust as needed
    public float AppearAnimationDuration; // Adjust as needed

    public bool IsTheCardWillDisplayedInRightSide;
    public bool SideHasChanged;
    public bool IsAnimating;
    public bool IsTheCardActivated;


    int infantryIndex = 0;
    int HorseIndex = 1;     // cavalry
    int TireIndex = 2;   // catapulte
    int SeaIndex = 3;   // 
    int TShipIndex = 4;



    [SerializeField] private GameObject TerrainInformationSprite;
    [SerializeField] private GameObject TerrainInformationText;
    [SerializeField] private GameObject TerrainInformationFillStars;
    [SerializeField] private GameObject TerrainInformationNum;
    [SerializeField] private GameObject TerrainMoveCostInf;
    [SerializeField] private GameObject TerrainMoveCostHorse;
    [SerializeField] private GameObject TerrainMoveCostTire;
    [SerializeField] private GameObject TerrainMoveCostSea;
    [SerializeField] private GameObject TerrainMoveCostTShip;
    [SerializeField] private GameObject TerrainReport;



    public void UpdateTerrainBIGIntel(Terrain terrain, Vector3 mousePositionWhenClickOnTerrain)
    {
        if (!IsTheCardActivated) ActivateCard();

        TerrainInformationSprite.GetComponent<UnityEngine.UI.Image>().sprite = terrain.spriteRenderer.sprite;
        TerrainInformationText.GetComponent<Text>().text = terrain.terrainName.ToString();
        TerrainInformationFillStars.GetComponent<UnityEngine.UI.Image>().fillAmount = TerrainsUtils.defenceStars[terrain.TerrainIndex] / 10f;
        TerrainInformationNum.GetComponent<Text>().text = terrain.incomingFunds.ToString();

        TerrainMoveCostInf.GetComponent<Text>().text = TerrainsUtils.MoveCost[terrain.TerrainIndex, infantryIndex].ToString();
        TerrainMoveCostHorse.GetComponent<Text>().text = TerrainsUtils.MoveCost[terrain.TerrainIndex, HorseIndex].ToString();
        TerrainMoveCostTire.GetComponent<Text>().text = TerrainsUtils.MoveCost[terrain.TerrainIndex, TireIndex].ToString();
        TerrainMoveCostSea.GetComponent<Text>().text = TerrainsUtils.MoveCost[terrain.TerrainIndex, SeaIndex].ToString();
        TerrainMoveCostTShip.GetComponent<Text>().text = TerrainsUtils.MoveCost[terrain.TerrainIndex, TShipIndex].ToString();

        TerrainReport.GetComponent<Text>().text = TerrainsUtils.ReportTerrain[terrain.TerrainIndex];

        AnimateTheCardMouvement(mousePositionWhenClickOnTerrain);
    }



    void Start()
    {
        RightCardPosition = new Vector3(XCardRightPosition, YcardPosition, 0);
        LeftCardPosition = new Vector3(XCardLeftPosition, YcardPosition, 0);
        InitialRightPositionOfTheCard = new Vector3(initialXCardRightPosition, initialYCardPosition, 0);
        InitialLeftPositionOfTheCard = new Vector3(initialXCardLeftPosition, initialYCardPosition, 0);

        canvasRect = GetComponent<RectTransform>();
    }



    // Calculate the position of the card relative to the bottom-right corner of the Canvas
    public void AnimateTheCardMouvement(Vector3 mousePositionWhenClickOnTerrain)
    {

        // Convert the mouse position to Canvas local coordinates
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, mousePositionWhenClickOnTerrain, null, out _);

        // Determine the new side based on the mouse position
        IsTheCardWillDisplayedInRightSide = mousePositionWhenClickOnTerrain.x < Screen.width / 2;

        // StartCoroutine(AnimateCard());
        if (!IsAnimating)
        {
            StartCoroutine(AnimateCardWhenItAppears());
        }
    }



    private IEnumerator AnimateCardWhenItAppears()
    {
        IsAnimating = true;
        MiniIntelController.Instance.LockTheMiniCard();
        MiniIntelController.Instance.DesActivateCard();
        CoCardsController.Instance.LockTheCOCard();
        CoCardsController.Instance.DesActivateCard();


        card.transform.localPosition = IsTheCardWillDisplayedInRightSide ? InitialRightPositionOfTheCard : InitialLeftPositionOfTheCard;
        Vector3 initialPosition = card.transform.localPosition;
        Vector3 targetPosition = IsTheCardWillDisplayedInRightSide ? RightCardPosition : LeftCardPosition;

        float startTime = Time.time;
        float endTime = startTime + AppearAnimationDuration;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / AppearAnimationDuration;
            float easedT = 1f - Mathf.Exp(-5f * t); // Ease-out function: 1 - e^(-5t)
            card.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, easedT);
            yield return null;
        }

        card.transform.localPosition = targetPosition;
        IsAnimating = false;

    }


    private IEnumerator AnimateCardWhenItHides()
    {
        IsAnimating = true;
        MiniIntelController.Instance.UnLockTheMiniCard();
        MiniIntelController.Instance.ActivateCard();
        CoCardsController.Instance.UnLockTheCOCard();
        CoCardsController.Instance.ActivateCard();

        Vector3 initialPosition = card.transform.localPosition;
        bool CardIsInRightSide = card.transform.localPosition.x == RightCardPosition.x;
        Vector3 targetPosition = IsTheCardWillDisplayedInRightSide ? InitialRightPositionOfTheCard : InitialLeftPositionOfTheCard;

        float startTime = Time.time;
        float endTime = startTime + HideAnimationDuration;

        while (Time.time < endTime)
        {
            float t = (Time.time - startTime) / HideAnimationDuration;
            float easedT = t; // Cubic easing function
            card.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, easedT);
            yield return null;
        }

        IsAnimating = false;

    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(AnimateCardWhenItHides());
        }
    }


    public void ActivateCard()
    {
        IsTheCardActivated = true;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void DesActivateCard()
    {
        IsTheCardActivated = false;
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

}