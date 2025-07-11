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
        "Denis", "Scott", "Leon", "Nash", "Marc", "Han", "Tom", "Laurence", "Matt","Brian", "Travis", "Robert", "Bob", "Kevin", "Fabian", "Nelson", "Henry", "Austin",
        "Wagner", "Adrian", "Andy", "Eden", "Carl", "Martin", "Wallace", "Tim", "Douglas", "Harry", "Eric", "Ethan", "Max", "Afonso", "Oliver", "Dan", "Ian", "Ewan"
    };
    private static readonly string[] secondNames = 
    {
        "Smith","Wells", "Clark", "Parker", "Herbert", "Adams", "Taylor", "Walker", "Wilson", "Railey" , "Jonhson", "Brock", "Santos", "Gomes", "Castro", "Neves", "Owen",
        "Young", "Hill", "Armstrong", "Roger", "Macmanus", "Salvi", "Free", "Freeman", "Strong", "Jager", "Cross", "Hunter", "Doyle", "Howard", "Malone", "Gray", "Summers",
        "Miller", "Von", "Yorke", "Clancy", "Tyson", "Collins", "King","Dent", "Barry", "Chambers", "Cole", "Jones", "Langley", "Lee", "Ross", "Souza","Hart", "Kane", "Law",
        "Rogers", "Newton", "Lewis", "Mckay", "Barnes", "Vincent", "Enies", "Wayne", "Burke", "Falcon", "Lamb", "Allen", "Connor", "Fray", "White", "Kid", "Lane"
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
    public int Defending;
    public int Guarding;
    public int Stealing;
    #region Hidden Variables
    public int Personality;//1 to 5 , 1-calm and 5-Agressive 
    #endregion
    #endregion
    [SerializeField] public GameObject bt_DraftInfo;
    public int ImageCharacterPortrait;

    #region Match Variables
    public bool HasTheBall = false;
    public int CurrentZone = 0;
    public int PointsMatch = 0;
    public bool IsStun = false;
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
        Defending = Random.Range(40, 99);
        Guarding = Random.Range(40, 99);
        Stealing = Random.Range(40, 99);
        ovr = (Shooting +Inside + Mid + Outside + Awareness + Defending + Guarding + Stealing) / 8;
        Personality = Random.Range(1, 5);
        //firstName = ((PlayerNames)Random.Range(0, System.Enum.GetValues(typeof(PlayerNames)).Length)).ToString(); // Random name from enum
        // Randomly select a name from the array
        playerFirstName = names[Random.Range(0, names.Length)];
        playerLastName = secondNames[Random.Range(0,secondNames.Length)];
        Age = Random.Range(20, 30);
        ImageCharacterPortrait = Random.Range(0, 4);
        //Debug.Log($"Generated Player: {firstName}, OVR: {ovr}");
    }
}
