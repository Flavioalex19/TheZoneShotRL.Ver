using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
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
        "Michael", "LeBron", "Kobe", "Larry", "Magic", "Tim", "Shaquille", "Bill", "Dmoc", "Clark", "Peter", "Son Goku"
    };
    [SerializeField] public string playerFirstName;
    [SerializeField] public int ovr;
    [SerializeField] public GameObject bt_DraftInfo;

    private void Start()
    {
       // GenerateRandomPlayer();
    }

    // Function to randomly generate a player's OVR and name
    public void GenerateRandomPlayer()
    {
        ovr = Random.Range(60, 100); // Random OVR between 60 and 99
        //firstName = ((PlayerNames)Random.Range(0, System.Enum.GetValues(typeof(PlayerNames)).Length)).ToString(); // Random name from enum
        // Randomly select a name from the array
        playerFirstName = names[Random.Range(0, names.Length)];
        //Debug.Log($"Generated Player: {firstName}, OVR: {ovr}");
    }
}
