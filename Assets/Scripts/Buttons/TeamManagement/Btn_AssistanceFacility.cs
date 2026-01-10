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

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("IN");
        image.sprite = sprite_onSprite;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = sprite_offSprite;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
