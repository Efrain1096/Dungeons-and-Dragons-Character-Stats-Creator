using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//Create a new Dropdown GameObject by going to the Hierarchy and clicking Create>UI>Dropdown. Attach this script to the Dropdown GameObject.
//Set your own Text in the Inspector window

public class CharacterAttributes : MonoBehaviour
{
    [Serializable]
    public class player 
    {
        public string name;
        public int currentHp;
        public int maxHp;
        public int currentXp;
        public int maxXp;
        public int armorClass;
        public int walkingSpeed;
        public int runningSpeed;
        public int jumpHeight;

       public float strength;
       public float dexterity;
       public float constitution;
       public float intelligence;
       public float wisdom;
       public float charisma;

       public string Class;
       public string race;
       public string alignment;

       
    };


    public GameObject nameInput;//Name
    public InputField currentHpInput;//Current HP
    public InputField maxHpInput;//Max HP
    public InputField jumpHeightInput;//Jump Height
    public InputField walkingSpeedInput;//Walking Speed
    public InputField armorRatingInput;//Armor Rating
    public InputField runningSpeedInput;//Running Speed
    public InputField currentExperienceInput;//Current Experience
    public InputField maxExperienceInput;//Max Experience

    public InputField jsonOutputField;

    //The bottom six dropdowns are for the abilities
    public Dropdown strengthDropDown;
    public Dropdown dexterityDropdown;
    public Dropdown constitutionDropdown;
    public Dropdown intelligenceDropdown;
    public Dropdown wisdomDropdown;
    public Dropdown charismaDropdown;
    //The bottom three are for race, class, and alignment
    public Dropdown raceDropdown;
    public Dropdown classDropdown;
    public Dropdown alignmentDropdown;

    public bool characterCreated = false;//Use as a flag for allowing the play button to work only if a character has beem 

    int diceRolls = 0;
    int abilityRolls = 0;
    int abilitiesCalculated = 0; //Have as flag if the ability values have been rolled

    List<string> itemList = new List<string>(); //Empty for this assignment

    List<string> races = new List<string>() 
    {
        "Dragonborn",
        "Dwarf",
        "Elf",
        "Gnome",
        "Half-Elf",
        "Half-Orc",
        "Halfling",
        "Human",
        "Tiefling" 
    };

    List<string> classes = new List<string>()
    {
        "Barbarian",
        "Bard",
        "Cleric",
        "Druid",
        "Fighter",
        "Monk",
        "Paladin",
        "Ranger",
        "Rogue",
        "Sorcerer",
        "Warlock",
        "Wizard"
    };

    List<string> alignments = new List<string>()
    {
        "Lawful Good",
        "Neutral Good",
        "Chaotic Good",
        "Lawful Neutral",
        "True Neutral",
        "Chaotic Neutral",
        "Lawful Evil",
        "Neutral Evil",
        "Chaotic Evil",
    };
    //Store the sum of the 3 highest rolls for the 6 abilities
    public List<string> highestDiceRolls = new List<string>();
    //Store the generated simulated dice rolls to then find the highest 3 and add them.
    List<int> diceRollValues = new List<int>();


    //Character object has been made with the appropiate variables
    public player character = new player();


    public void Start()
    {

        PopulateDropdowns();
    }

    public void characterScene()
    {
        SceneManager.LoadScene("CharacterCreation");
    }

    public void menuScene()
    {
        SceneManager.LoadScene("Menu");
    }

    public void playScene()
    {
        if(characterCreated)//Only load the play scene if a character has been rolled
        {
            SceneManager.LoadScene("Play");
        }
    }

    public void quitGame()
    {
        //Only works when built and run as an application
        Application.Quit();
    }

    public void PopulateDropdowns() //Populate th non-ability dropdowns
    {
        classDropdown.AddOptions(classes);
        raceDropdown.AddOptions(races);
        alignmentDropdown.AddOptions(alignments);
    }


    public void PopulateCharacterData()
    {
        character.strength = float.Parse(strengthDropDown.options[strengthDropDown.value].text, CultureInfo.InvariantCulture.NumberFormat);
        character.dexterity = float.Parse(dexterityDropdown.options[dexterityDropdown.value].text, CultureInfo.InvariantCulture.NumberFormat);
        character.constitution = float.Parse(constitutionDropdown.options[constitutionDropdown.value].text, CultureInfo.InvariantCulture.NumberFormat);
        character.intelligence = float.Parse(intelligenceDropdown.options[intelligenceDropdown.value].text, CultureInfo.InvariantCulture.NumberFormat);
        character.wisdom = float.Parse(wisdomDropdown.options[wisdomDropdown.value].text, CultureInfo.InvariantCulture.NumberFormat);
        character.charisma = float.Parse(charismaDropdown.options[charismaDropdown.value].text, CultureInfo.InvariantCulture.NumberFormat);

        character.Class = classDropdown.options[classDropdown.value].text;
        character.race = raceDropdown.options[raceDropdown.value].text;
        character.alignment = alignmentDropdown.options[alignmentDropdown.value].text;

        character.name = nameInput.GetComponent<Text>().text;
        character.currentHp = Convert.ToInt32(currentHpInput.text);
        character.maxHp = Convert.ToInt32(maxHpInput.text);
        character.currentXp = Convert.ToInt32(currentExperienceInput.text);
        character.maxXp = Convert.ToInt32(maxExperienceInput.text);
        character.armorClass = Convert.ToInt32(armorRatingInput.text);
        character.walkingSpeed = Convert.ToInt32(walkingSpeedInput.text);
        character.runningSpeed = Convert.ToInt32(runningSpeedInput.text);
        character.jumpHeight = Convert.ToInt32(jumpHeightInput.text);
    }

    public void GenerateJson()
    {

        PopulateCharacterData();
        jsonOutputField.text = JsonUtility.ToJson(character);
        characterCreated = true;
    }


    //The diceRoll() will be called with each click of the button to generate the pseudorandom dice values
    //Expect to have the user click the button 30 times, Price said so in class lecture.
    public void DiceRoll()
    {

        diceRolls++;
        diceRollValues.Add(UnityEngine.Random.Range(1, 7));
        
        string abilityToString;
        int abilityValue;
        //If the numbers of throws is divisible by 5, then we take the highest three
        if (diceRolls % 5 == 0 && abilityRolls < 6)
        {
            abilityValue = HighestThreeRollsSum();
            abilityToString = abilityValue.ToString();
            highestDiceRolls.Add(abilityToString);
            diceRollValues.Clear();
            abilityRolls++;
        }

        if (diceRolls == 30 && abilitiesCalculated < 1) //Once the user clicks 30 times, populate the dropdowns with sums of generated values.
        {
            strengthDropDown.AddOptions(highestDiceRolls);
            dexterityDropdown.AddOptions(highestDiceRolls);
            constitutionDropdown.AddOptions(highestDiceRolls);
            intelligenceDropdown.AddOptions(highestDiceRolls);
            wisdomDropdown.AddOptions(highestDiceRolls);
            charismaDropdown.AddOptions(highestDiceRolls);
            abilitiesCalculated = 1;
        }

    }
    public int HighestThreeRollsSum()
    {
        int highestRollSum = 0;
        int maxValueFound = 0;
        int removalIndex = 0;

        for(int i = 0; i < 3; i++)//Get the three highest rolls
        {
            for(int j = 0; j < diceRollValues.Count; j++)//Get the highest value present in the diceRollValues
            {
                if(maxValueFound < diceRollValues[j])
                {
                    removalIndex = j;
                    maxValueFound = diceRollValues[j];
                }
            }
            diceRollValues.RemoveAt(removalIndex);
            highestRollSum += maxValueFound;
            maxValueFound = 0;

        }
        return highestRollSum;

    }


    


}
