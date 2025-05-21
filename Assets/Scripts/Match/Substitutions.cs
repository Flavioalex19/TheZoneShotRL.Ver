using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Substitutions : MonoBehaviour
{
    MatchManager matchManager;
    Player _StarterPLayerSelected;
    Player _BenchPlayerSelected;
    [SerializeField]int indexStarter;
    [SerializeField]int indexBench;

    Button confirmSwapBtn;

    private void Start()
    {
        matchManager = GameObject.Find("MatchManager").GetComponent<MatchManager>();
        _StarterPLayerSelected = null;
        _BenchPlayerSelected = null;
    }
    private void Update()
    {
        if (GameObject.Find("btn_ConfirmSwap"))
        {
            
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

    }
    public void ChooseStarterToSwap(int index)
    {
        _StarterPLayerSelected = matchManager.HomeTeam.playersListRoster[index];
    }
    public void ChooseBenchToSwap(int index)
    {
        _BenchPlayerSelected = matchManager.HomeTeam.playersListRoster[index];
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
