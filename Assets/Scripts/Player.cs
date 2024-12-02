using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region SavedVariables
    public enum PlayerNames
    {
        John,
        Michael,
        David,
        Chris,
        Alex,
        Steve,
        James,
        Robert
    }
    // Array of 8 player names
    private static readonly string[] names =
    {
        "Albert", "Joseph", "Jonh", "Philip", "George", "Isaac", "Alan", "Homer", "Devon", "Clark", "Peter", "William", "Alexandre", "Rick", "Dante", "Virgil", "Frank"
    };
    [SerializeField] public string playerFirstName;
    [SerializeField] public float ovr;
    public int Inside;
    public int Mid;
    public int Outside;
    public int Awareness;
    #region Hidden Variables
    public int Personality;//1 to 5 , 1-calm and 5-Agressive 
    #endregion
    #endregion
    [SerializeField] public GameObject bt_DraftInfo;

    #region Match Variables
    public bool HasTheBall = false;
    public int CurrentZone = 0;
    public int PointsMatch = 0;
    #endregion
    private void Start()
    {
       // GenerateRandomPlayer();
    }

    // Function to randomly generate a player's OVR and name
    public void GenerateRandomPlayer()
    {
         // Random OVR between 60 and 99
        Inside = Random.Range(40, 99);
        Mid = Random.Range(40, 99);
        Outside = Random.Range(40, 99);
        Awareness = Random.Range(40, 99);
        ovr = (Inside + Mid + Outside + Awareness) / 4;
        Personality = Random.Range(1, 5);
        //firstName = ((PlayerNames)Random.Range(0, System.Enum.GetValues(typeof(PlayerNames)).Length)).ToString(); // Random name from enum
        // Randomly select a name from the array
        playerFirstName = names[Random.Range(0, names.Length)];
        //Debug.Log($"Generated Player: {firstName}, OVR: {ovr}");
    }
}
