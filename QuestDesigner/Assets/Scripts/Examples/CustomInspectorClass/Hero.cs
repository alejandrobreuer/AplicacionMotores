using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour
{

    public string[] heroClasses = { "Mage", "Warrior", "Bowman" };
    public string heroClass;
    public string myName;
    public int healthPoints;
    public float speed;
    public Color playerColor;
    public string biography;
    public Vector3 respawnPosition;
    public Hero theOtherHero;
    public float ammountOfExperience;
    public int optionsNumber;
}
