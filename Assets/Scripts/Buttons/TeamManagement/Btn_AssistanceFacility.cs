using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Btn_AssistanceFacility : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Sprite sprite_offSprite;
    [SerializeField] Sprite sprite_onSprite;
    [SerializeField] Image image;
    Button btn_AssistanceInfo;
    GameObject panel_facilityInfo;
    [SerializeField] string assistanceDescription;
    [SerializeField] string facilityEffects;
    [SerializeField] string facilityEffect1;
    [SerializeField] string facilityEffect2;
    Animator animator;

    TeamManagerUI teamManagerUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //print("IN");
        image.sprite = sprite_onSprite;
        animator.SetBool("On", true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = sprite_offSprite;
        animator.SetBool("On", false);
    }


    // Start is called before the first frame update
    void Start()
    {
        teamManagerUI = GameObject.Find("TeamManagerUI").GetComponent<TeamManagerUI>();
        animator = GetComponent<Animator>();
        btn_AssistanceInfo = GetComponent<Button>();
        btn_AssistanceInfo.onClick.AddListener(() => teamManagerUI.FacilityPanelInfoUpdate(assistanceDescription, facilityEffects,facilityEffect1, facilityEffect2 ,sprite_offSprite));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
