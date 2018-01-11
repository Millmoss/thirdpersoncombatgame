using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    private int state;
    private bool[][] transitions; //if first index state equivalent can lead to the second index state equivalent

    public FSM(int s, bool[][] t)
    {
        state = s;
        transitions = t;
    }
    
    public int getState()
    {
        return state;
    }

    public int changeState(int s)
    {
        if (s >= transitions[state].Length || s < 0)
            return -1;
        if (transitions[state][s])
        {
            state = s;
            return state;
        }
        else
            return -1;
    }
}