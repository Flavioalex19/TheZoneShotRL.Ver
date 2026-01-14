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

    TeamManagerUI teamManagerUI;

    public void OnPointerEnter(PointerEventData eventData)
    {
        //print("IN");
        image.sprite = sprite_onSprite;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = sprite_offSprite;
    }


    // Start is called before the first frame update
    void Start()
    {
        teamManagerUI = GameObject.Find("TeamManagerUI").GetComponent<TeamManagerUI>();
        btn_AssistanceInfo = GetComponent<Button>();
        btn_AssistanceInfo.onClick.AddListener(() => teamManagerUI.FacilityPanelInfoUpdate(assistanceDescription, facilityEffects, sprite_offSprite));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
