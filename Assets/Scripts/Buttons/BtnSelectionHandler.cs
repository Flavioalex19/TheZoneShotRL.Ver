using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class BtnSelectionHandler : MonoBehaviour
{

    [Header("References")]
    public List<Selectable> Selectables = new List<Selectable>();

    [Header("Text and color text")]
    [SerializeField] List<TextMeshProUGUI> list_text_highlight = new List<TextMeshProUGUI>();
    [ColorUsage(true, true)]
    [SerializeField] Color color0;
    [ColorUsage(true, true)]
    [SerializeField] Color color1;
    [ColorUsage(true, true)]
    [SerializeField] Color color2;

    [Header("Animations Variables")]
    [SerializeField] protected float _selectedAnimScale = 1.1f;
    [SerializeField] protected float _scaleDuration = .25f;

    protected Dictionary<Selectable, Vector3> _scales = new Dictionary<Selectable, Vector3>();//All selectables elements

    protected Tween _scaleUpTween;
    protected Tween _scaleDownTween;

    Selectable _lastHovered; // ADDED: store the currently hovered Selectable


    public virtual void Awake()
    {
        foreach (var selectable in Selectables)
        {
            AddSelectedListeners(selectable);
            _scales.Add(selectable, selectable.transform.localScale);
        }
    }

    public virtual void OnEnable()
    {
        //Ensure all selectables are reset to original scale
        for (int i = 0; i < Selectables.Count; i++)
        {
            //Selectables[i].transform.localScale = _scales[Selectables[i]];
            if (Selectables[i] != null)
            {
                Selectables[i].transform.DOKill();
                Selectables[i].transform.localScale = _scales[Selectables[i]];
                   
            }
        }
    }
    public virtual void OnDisable()
    {
        _scaleUpTween.Kill(true);
        _scaleDownTween.Kill(true);
    }

    protected virtual void AddSelectedListeners(Selectable selectable)
    {
        //Add listeners
        EventTrigger trigger = selectable.gameObject.GetComponent<EventTrigger>();
        /*
        if (trigger != null)
        {
            trigger = selectable.AddComponent<EventTrigger>();
        }
        */
        if (trigger == null) // Fix the logic
        {
            trigger = selectable.gameObject.AddComponent<EventTrigger>();
        }
        //Add SELECT event
        EventTrigger.Entry SelectEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Select
        };
        SelectEntry.callback.AddListener(OnSelect);
        trigger.triggers.Add(SelectEntry);

        //Add DESELECT event
        EventTrigger.Entry DeselectEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Deselect
        };
        DeselectEntry.callback.AddListener(OnDeselect);
        trigger.triggers.Add(DeselectEntry);


        //Add ONPOINTER
        EventTrigger.Entry PointEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        PointEnter.callback.AddListener(OnPointEnter);
        trigger.triggers.Add(PointEnter);

        //Add PointExit
        EventTrigger.Entry PointerExit = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerExit
        };
        PointerExit.callback.AddListener(OnPointExit);
        trigger.triggers.Add(PointerExit);

        EventTrigger.Entry clickEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
        clickEntry.callback.AddListener(OnPointerClick);
        trigger.triggers.Add(clickEntry);
    }

    public void OnSelect(BaseEventData eventData)
    {
        Vector3 newScale = eventData.selectedObject.transform.localScale * _selectedAnimScale;
        //_scaleUpTween = eventData.selectedObject.transform.DOScale(newScale, _scaleDuration);
        _scaleUpTween?.Kill();
        _scaleUpTween = eventData.selectedObject.transform.DOScale(newScale, _scaleDuration);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Selectable sel = eventData.selectedObject.GetComponent<Selectable>();
        //_scaleDownTween = eventData.selectedObject.transform.DOScale(_scales[sel], _scaleDuration);
        _scaleDownTween?.Kill(); //Kill existing tween
        _scaleDownTween = eventData.selectedObject.transform.DOScale(_scales[sel], _scaleDuration);
    }

    public void OnPointEnter(BaseEventData eventData)
    {
        PointerEventData pointerEventData = eventData as PointerEventData;
        if(pointerEventData != null)
        {
            pointerEventData.selectedObject = pointerEventData.pointerEnter;

            // ADDED: Store the hovered selectable.
            _lastHovered = pointerEventData.pointerEnter.GetComponent<Selectable>();

            if (_lastHovered != null)
            {
                _lastHovered.transform.DOKill();                    
                                                                    
                ChangeTextColor(pointerEventData.pointerEnter, color1);
            }

            //Change color text
            ChangeTextColor(pointerEventData.pointerEnter, color1);

            if (GameObject.Find("SoundTrack Buttons Source"))
            {
                AudioSource audioSource = GameObject.Find("SoundTrack Buttons Source").GetComponent<AudioSource>();
                audioSource.Play();
            }
            OnSelect(pointerEventData);//MDOD
        }
    }
    public void OnPointExit(BaseEventData eventData)
    {
        // Use the stored _lastHovered instead of pointerEventData.pointerEnter.
        if (_lastHovered != null && _scales.ContainsKey(_lastHovered))
        {
            _lastHovered.transform.DOKill();
            _lastHovered.transform.localScale = _scales[_lastHovered];

            _scaleDownTween?.Kill(); // Kill any running tween
            _scaleDownTween = _lastHovered.transform.DOScale(_scales[_lastHovered], _scaleDuration);

            //Change color text
            ChangeTextColor(_lastHovered.gameObject, color0);
        }
        if (_lastHovered.transition == Selectable.Transition.ColorTint)
        {
            // Opção mais confiável: força a transição de cor de volta para Normal
            _lastHovered.targetGraphic.CrossFadeColor(
                _lastHovered.colors.normalColor,
                0.1f,           // duração da transição
                true,
                true
            );
        }
        else
        {
            // Caso use outro tipo de transição (SpriteSwap ou Animation)
            _lastHovered.OnDeselect(null);
        }
        //_lastHovered = null; // Clear the reference
        _lastHovered = null;
    }
    public List<Selectable> GetSelectabes()
    {
        return Selectables;
    }
    private void ChangeTextColor(GameObject target, Color newColor)
    {
        if (target == null) return;

        TextMeshProUGUI[] texts = target.GetComponentsInChildren<TextMeshProUGUI>(true);

        foreach (TextMeshProUGUI text in texts)
        {
            text.color = newColor;
        }
    }
    public void OnPointerClick(BaseEventData eventData)
    {
        if (eventData.selectedObject == null) return;

        Transform target = eventData.selectedObject.transform;

        // Mata tweens anteriores para evitar conflito
        target.DOKill();

        // Animação de clique: Diminui - Volta ao normal
        target.DOScale(_scales[eventData.selectedObject.GetComponent<Selectable>()] * 0.30f, 0.08f)
              .OnComplete(() =>
              {
                  target.DOScale(_scales[eventData.selectedObject.GetComponent<Selectable>()], 0.15f);
              });
    }
}
