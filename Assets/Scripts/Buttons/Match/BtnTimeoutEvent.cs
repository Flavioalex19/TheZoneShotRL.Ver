using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnTimeoutEvent : MonoBehaviour
{
    [SerializeField] SO_TimeoutEvent _TimeoutEvent;
    [SerializeField] int _eventIndex;
    MatchManager _matchManager;
    Button _button;
    [SerializeField]bool _isLocked = false;
    bool _hasActionRemaining = true;
    // Start is called before the first frame update
    void Start()
    {
        _matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
        //Add the onclick by the index
        _button = gameObject.GetComponent<Button>();
        _isLocked = false;
        _button.onClick.AddListener(() => _TimeoutEvent.EventExc(_eventIndex, _matchManager.HomeTeam,ref _isLocked, ref _matchManager.HasActionOnTimeout));
        

    }

    // Update is called once per frame
    void Update()
    {
        //if (_matchManager.GetCanCallTimeout() == true) _matchManager.HasActionOnTimeout = true;
        if(_isLocked)
        {
            _button.interactable = false;
        }
        else
        {
            if(_matchManager.HasActionOnTimeout == false)
            {
                _button.interactable = false;
            }
            else
            {
                _button.interactable = true;
            }
            
            
        }
        
    }
}
