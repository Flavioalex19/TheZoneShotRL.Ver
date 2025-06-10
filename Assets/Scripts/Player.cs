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
    private static readonly string[] names =
    {
        "Albert", "Joseph", "Jonh", "Philip", "George", "Isaac", "Alan", "Homer", "Devon", "Clark", "Peter", "William", "Alexandre", "Rick", "Dante", "Virgil", "Frank" ,
        "Caleb", "Rio", "Luke", "Morgan", "Anderson", "Richard", "Daniel", "David", "Jacob", "Arthur", "Lucas", "Walter", "Neil", "Steve", "Ash", "Sergio", "Dean", "Samuel",
        "Denis", "Scott", "Leon", "Nash", "Marc", "Kevin", "Tom", "Laurence", "Matt","Brian", "Travis", "Robert", "Bob", "Kevin", "Fabian", "Nelson", "Henry"
    };
    private static readonly string[] secondNames = 
    {
        "Smith","Wells", "Clark", "Parker", "Herbert", "Adams", "Taylor", "Walker", "Wilson", "Railey" , "Jonhson", "Brock", "Santos", "Gomes", "Castro", "Neves", "Owen",
        "Young", "Hill", "Armstrong", "Roger", "Mackmanus", "Salvi", "Free", "Freeman", "Strong", "Jager", "Cross", "Hunter", "Doyle", "Dezz", "Malone", "Gray", "Summers",
        "Miller", "Von", "Yorke", "Clancy", "Tyson", "Collins", "King"
    };
    [SerializeField] public string playerFirstName;
    [SerializeField] public string playerLastName;
    public int Age;
    [SerializeField] public float ovr;
    public int Shooting;
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
        Shooting = Random.Range(40,99);
        Inside = Random.Range(40, 99);
        Mid = Random.Range(40, 99);
        Outside = Random.Range(40, 99);
        Awareness = Random.Range(40, 99);
        ovr = (Inside + Mid + Outside + Awareness) / 4;
        Personality = Random.Range(1, 5);
        //firstName = ((PlayerNames)Random.Range(0, System.Enum.GetValues(typeof(PlayerNames)).Length)).ToString(); // Random name from enum
        // Randomly select a name from the array
        playerFirstName = names[Random.Range(0, names.Length)];
        playerLastName = secondNames[Random.Range(0,secondNames.Length)];
        Age = Random.Range(20, 30);
        //Debug.Log($"Generated Player: {firstName}, OVR: {ovr}");
    }
}
