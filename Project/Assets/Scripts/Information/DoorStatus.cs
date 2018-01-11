using System.Collections;
using System.Collections.Generic;

public class DoorStatus
{
    private int healthMax;
    private int healthCurrent;

    public DoorStatus(char t)
    {
        if (t == 'w') //door is wooden
            healthMax = 10;
        else if (t == 'i') //door is iron reinforced
            healthMax = 20;
        else if (t == 's') //door is steel reinforced
            healthMax = 30;
        else
            healthMax = 0;
        healthCurrent = healthMax;
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
}
