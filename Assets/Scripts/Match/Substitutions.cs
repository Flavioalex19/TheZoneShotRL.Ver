using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Substitutions : MonoBehaviour
{
    MatchManager matchManager;
    public Player _StarterPLayerSelected;
    public Player _BenchPlayerSelected;
    [SerializeField]int indexStarter;
    [SerializeField]int indexBench;
    [SerializeField] List<TextMeshProUGUI> _text_StartersNames = new List<TextMeshProUGUI>();

    //ui teesting
    [SerializeField]TextMeshProUGUI text_playerStarter;
    [SerializeField] TextMeshProUGUI text_benchPlayer;

    Button confirmSwapBtn;

    private void Start()
    {
        matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
        _StarterPLayerSelected = null;
        _BenchPlayerSelected = null;
    }
   
    void TextNameUpdate()
    {
        for (int i = 0; i < _text_StartersNames.Count; i++)
        {
            _text_StartersNames[i].text = matchManager.HomeTeam.playersListRoster[i].playerFirstName + " " + matchManager.HomeTeam.playersListRoster[i].playerLastName;
        }
    }
    public void SwapPlayers()
    {
        // Find the index of the first element with IsActive == true
        indexStarter = matchManager.HomeTeam.playersListRoster.FindIndex(element => element == _StarterPLayerSelected);
        indexBench = matchManager.HomeTeam.playersListRoster.FindIndex(element => element == _BenchPlayerSelected);
        Player temp = matchManager.HomeTeam.playersListRoster[indexStarter];
        matchManager.HomeTeam.playersListRoster[indexStarter] = matchManager.HomeTeam.playersListRoster[indexBench];
        matchManager.HomeTeam.playersListRoster[indexBench] = temp;
        _StarterPLayerSelected = null;
        _BenchPlayerSelected = null;
        text_playerStarter.text = " ";
        text_benchPlayer.text = " ";


    }
    public void ChooseStarterToSwap(int index)
    {

        _StarterPLayerSelected = matchManager.HomeTeam.playersListRoster[index];
        indexStarter = index;
        text_playerStarter.text = _StarterPLayerSelected.playerLastName;

    }
    public void ChooseBenchToSwap(int index)
    {
        //_BenchPlayerSelected = matchManager.HomeTeam.playersListRoster[index];
        _BenchPlayerSelected = matchManager.HomeTeam.playersListRoster[index];
        indexBench = index;
        text_benchPlayer.text = _BenchPlayerSelected.playerLastName;
    }
    public void StarterSwap0() { ChooseStarterToSwap(0); }
    public void StarterSwap1() { ChooseStarterToSwap(1); }
    public void StarterSwap2() {  ChooseStarterToSwap(2); }
    public void StarterSwap3() {  ChooseStarterToSwap(3); }

    public void BenchSwap0() { ChooseBenchToSwap(4); }
    public void BenchSwap1() { ChooseBenchToSwap(5); }
    public void BenchSwap2() {  ChooseBenchToSwap(6); }
    public void BenchSwap3() {  ChooseBenchToSwap(7); }


}
