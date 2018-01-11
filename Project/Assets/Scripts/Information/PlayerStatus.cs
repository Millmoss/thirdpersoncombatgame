using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private int healthMax;
    private int healthCurrent;
    private float energy; //represents hunger and stamina
    private float energyMax;
    
	public PlayerStatus()
    {
        healthMax = 20;
        healthCurrent = 20;
        energyMax = 6;
        energy = 5;
	}

    public void changeHealth(int change)
    {
        healthCurrent += change;
        if (healthCurrent > healthMax)
            healthCurrent = healthMax;
    }

    public int getHealth()
    {
        return healthCurrent;
    }

    public string getHealthString()
    {
        string r = " Health:\t" + healthCurrent + " / " + healthMax;
        return r;
    }

    public void changeEnergy(float change) //attacking too often causes energy to decrease more rapidly
    {
        if (energy < 1)
            energy += change * 2;
        else
            energy += change;
        if (energy > energyMax)
            energy = energyMax;
    }

    public float getEnergy()
    {
        return energy;
    }

    public string getHungerString()
    {
        string r = "";
        int c = Mathf.CeilToInt(energy);
        switch (c)
        {
            case 0:
                r = "d";
                break;
            case 1:
                r = "Dying"; //all energy changes are doubled and denotes more negative status
                break;
            case 2:
                r = "Starving"; //denotes negative statuses
                break;
            case 3:
                r = "Very Hungry";
                break;
            case 4:
                r = "Hungry";
                break;
            case 5:
                r = "Good";
                break;
            case 6:
                r = "Full";
                break;
            default:
                r = "Energy Error";
                break;
        }
        r = " Hunger:\t" + r;
        return r;
    }
}