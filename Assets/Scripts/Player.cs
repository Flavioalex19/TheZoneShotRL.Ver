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
        "Caleb", "Rio", "Luke", "Anderson", "Richard", "Daniel", "David", "Jacob", "Arthur", "Lucas", "Walter", "Neil", "Steve", "Ash", "Sergio", "Dean", "Samuel",
        "Denis", "Scott", "Leon", "Nash", "Marc", "Han", "Tom", "Laurence", "Matt","Brian", "Travis", "Robert", "Bob", "Kevin", "Fabian", "Nelson", "Henry", "Austin",
        "Wagner", "Adrian", "Andy", "Eden", "Carl", "Martin", "Wallace", "Tim", "Douglas", "Harry", "Eric", "Ethan", "Max", "Afonso", "Oliver", "Dan", "Ian", "Ewan", "Jack",
        "Felix", "Arnold", "Tommy", "Trevor", "Thomas", "Zack", "Isiah","Erick", "Josh", " Norman", "Marco", "Ed", "Fred", "Dennis", "Marshall", "Patrick", "Ty", "Jerry", "Alfred",
        "Andrew", "Austin", "Acer", "Lima"
    };
    private static readonly string[] secondNames = 
    {
        "Smith","Wells", "Clark", "Parker", "Herbert", "Adams", "Taylor", "Walker", "Wilson", "Railey" , "Jonhson", "Brock", "Santos", "Gomes", "Castro", "Nunes", "Owen",
        "Young", "Hill", "Armstrong", "Roger", "Macmanus", "Salvi", "Free", "Freeman", "Strong", "Jager", "Cross", "Hunter", "Doyle", "Howard", "Malone", "Gray", "Summers",
        "Miller", "Von", "Yorke", "Clancy", "Tyson", "Collins", "King","Dent", "Barry", "Chambers", "Cole", "Jones", "Langley", "Lee", "Ross", "Souza","Hart", "Kane", "Law",
        "Rogers", "Newton", "Lewis", "Mckay", "Barnes", "Vincent", "Enies", "Wayne", "Burke", "Falcon", "Lamb", "Allen", "Connor", "Fray", "White", "Lane", "Morgan",
        "Bollock", "Trent", "Brady", "Machado","Edwards", "Thompson", "Mitty", "Webster", "Web", "Watson", "Homes", "Aiken" , "Ainsworth", "Adler", "May"
    };
    [SerializeField] public string playerFirstName;
    [SerializeField] public string playerLastName;
    public int Age;
    [SerializeField] public int ovr;
    public int Shooting;
    public int Inside;
    public int Mid;
    public int Outside;
    public int Awareness;
    public int Defending;
    public int Guarding;
    public int Stealing;
    public int Juking;
    public int Consistency;
    public int Control;
    public int Positioning;
    public int ContractYears;
    public int Salary;
    public int J_Number;
    public int Zone;
    #region Hidden Variables
    public int Personality;//1 to 5 , 1-calm and 5-Agressive 
    //CareerStats
    public int CareerGamesPlayed = 0;
    public int CareerPoints = 0;
    public int CareerSteals = 0;
    public int CareerFieldGoalAttempted = 0;
    public int CareerFieldGoalMade = 0;
    //PlayerEvents/Team stats
    public Player bondPlayer = null;
    public int buff = 0;
    #endregion
    #endregion
    [SerializeField] public GameObject bt_DraftInfo;
    public int ImageCharacterPortrait;

    #region Match Variables
    public bool HasTheBall = false;
    public int CurrentZone = 0;
    public int PointsMatch = 0;
    public int StealsMatch = 0;
    public bool IsStun = false;
    public int MaxStamina = 100;
    public int CurrentStamina;
    public bool isInjured = false;
    public int statBuff = 0;
    public int statDefBuff = 0;
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
        Juking = Random.Range(40, 99);
        Consistency = Random.Range(40, 99);
        Control = Random.Range(40, 99);
        Positioning = Random.Range(40, 99);
        ovr = (Shooting +Inside + Mid + Outside + Awareness + Defending + Guarding + Stealing + Juking + Consistency + Control + Positioning) / 12;
        Personality = Random.Range(1, 5);
        Zone = Random.Range(0, 2);
        //firstName = ((PlayerNames)Random.Range(0, System.Enum.GetValues(typeof(PlayerNames)).Length)).ToString(); // Random name from enum
        // Randomly select a name from the array
        playerFirstName = names[Random.Range(0, names.Length)];
        playerLastName = secondNames[Random.Range(0,secondNames.Length)];
        Age = Random.Range(20, 34);
        //ImageCharacterPortrait = Random.Range(0, 7);
        ImageCharacterPortrait = Random.Range(0, 30);
        GenerateContract();
        buff = 0;
        bondPlayer = null;
        //Debug.Log($"Generated Player: {firstName}, OVR: {ovr}");
    }
    //Generate player with ovr from 50-75

    public void GenerateEarlyPlayer()
    {
        GeneratePlayerByTier(55, 65, 67, 73);
    }

    public void GenerateMidPlayer()
    {
        GeneratePlayerByTier(65, 70, 75, 80);
    }

    public void GenerateEndPlayer()
    {
        GeneratePlayerByTier(75, 89, 85, 90);
    }

    private void GeneratePlayerByTier(int minLow, int maxLow, int minHigh, int maxHigh)
    {
        // Primeiro: Portrait e Personality
        ImageCharacterPortrait = Random.Range(0, 36); // 0 a 35
        Personality = Random.Range(1, 6); // 1 a 5

        // Define atributos específicos baseado no portrait
        bool isShootingSpec = ImageCharacterPortrait <= 11;
        bool isJukingSpec = ImageCharacterPortrait >= 12 && ImageCharacterPortrait <= 20;
        bool isControlSpec = ImageCharacterPortrait >= 21 && ImageCharacterPortrait <= 32;
        bool isDefendingSpec = ImageCharacterPortrait >= 33 && ImageCharacterPortrait <= 35;

        // Gera atributos
        Shooting = isShootingSpec ? Random.Range(minHigh, maxHigh + 1) : Random.Range(minLow, maxLow + 1);
        Inside = isShootingSpec ? Random.Range(minHigh, maxHigh + 1) : Random.Range(minLow, maxLow + 1);
        Mid = isShootingSpec ? Random.Range(minHigh, maxHigh + 1) : Random.Range(minLow, maxLow + 1);
        Outside = isShootingSpec ? Random.Range(minHigh, maxHigh + 1) : Random.Range(minLow, maxLow + 1);

        Juking = isJukingSpec ? Random.Range(minHigh, maxHigh + 1) : Random.Range(minLow, maxLow + 1);
        Stealing = isJukingSpec ? Random.Range(minHigh, maxHigh + 1) : Random.Range(minLow, maxLow + 1);
        Consistency = isJukingSpec ? Random.Range(minHigh, maxHigh + 1) : Random.Range(minLow, maxLow + 1);

        Control = isControlSpec ? Random.Range(minHigh, maxHigh + 1) : Random.Range(minLow, maxLow + 1);
        Awareness = isControlSpec ? Random.Range(minHigh, maxHigh + 1) : Random.Range(minLow, maxLow + 1);

        Defending = isDefendingSpec ? Random.Range(minHigh, maxHigh + 1) : Random.Range(minLow, maxLow + 1);
        Guarding = isDefendingSpec ? Random.Range(minHigh, maxHigh + 1) : Random.Range(minLow, maxLow + 1);
        Positioning = isDefendingSpec ? Random.Range(minHigh, maxHigh + 1) : Random.Range(minLow, maxLow + 1);

        // Calcula OVR (média arredondada)
        int sum = Shooting + Inside + Mid + Outside + Awareness + Defending + Guarding + Stealing + Juking + Consistency + Control + Positioning;
        ovr = Mathf.RoundToInt(sum / 12f);

        // Outros campos (iguais ao original)
        playerFirstName = names[Random.Range(0, names.Length)];
        playerLastName = secondNames[Random.Range(0, secondNames.Length)];
        Age = Random.Range(20, 34);
        GenerateContract();
        buff = 0;
        bondPlayer = null;

        // Debug opcional
        // Debug.Log($"Gerado player (Portrait: {ImageCharacterPortrait}, OVR: {ovr})");
    }
    void GenerateContract()
    {
        if(ovr>= 40 && ovr <= 50)
        {
            ContractYears = 6;
            Salary = UnityEngine.Random.Range(2,5);
        }
        else if(ovr > 50 && ovr <= 60)
        {
            ContractYears = 6;
            Salary = UnityEngine.Random.Range(4, 8);
        }
        else if (ovr > 60 && ovr <= 70)
        {
            ContractYears = UnityEngine.Random.Range(5, 9);
            Salary = 6;
        }
        else if (ovr > 70 && ovr <= 80)
        {
            ContractYears = 4;
            Salary = UnityEngine.Random.Range(8, 12);
        }
        else if (ovr > 80 && ovr <= 90)
        {
            ContractYears = 3;
            Salary = 10;
        }
        else if (ovr > 90)
        {
            ContractYears = 2;
            Salary = 15;
        }
    }
    public void UpdateOVR()
    {
        ovr = (Shooting + Inside + Mid + Outside + Awareness + Defending + Guarding + Stealing + Juking + Consistency + Control + Positioning) / 12;
    }

}
