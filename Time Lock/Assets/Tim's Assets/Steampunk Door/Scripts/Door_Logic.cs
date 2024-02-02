using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Logic : MonoBehaviour
{
    //private Variables 
    private Animator DoorController;
    private bool open = false;                  
    private float clickdelay = 0.33f;           //delay between clicks
    private float timestamp;

    //public Variables
    public string InputAction = "Fire1";        //input Action that is used to interact with the Door
    public string Character_Tag = "Player";     //tag of the Player Character
    public bool locked = false;                 //should the door be locked


    //Doorstates if the peephole should be enabled
    public enum State
    {
        never, on_lock, on_unlock, on_open, always
    }
    public State Enable_Hole;
   

    //Initialization
    void Start()
    {
        DoorController = GetComponent<Animator>(); //Get the door Animation Controller

        if (State.always == Enable_Hole)          //Check if peephole always open
        {
            DoorController.SetBool("openhole ?", true); //play peephole animation
        }
    }

    //is Player inside Door Trigger Area
    void OnTriggerStay(Collider other)
    {

        //check if player is hitting the associated Input 
        if (Input.GetButtonDown(InputAction) && other.tag == Character_Tag && Time.time >= timestamp)
        {
            timestamp = Time.time + clickdelay; //set timestamp for click delay

            //check if Door is locked
            if (locked == true)
            {
                DoorController.SetBool("Is locked ?", true); //play lock animation

                if (State.on_lock == Enable_Hole)
                { 
                    DoorController.SetBool("openhole ?", true);
                }
            }
            else
            {
                //check if door can be unlocked
                if (DoorController.GetBool("unlock door"))
                {
                    //check if unlockanimation is still playing
                    if (DoorController.GetCurrentAnimatorStateInfo(0).IsName("unlock") || DoorController.GetCurrentAnimatorStateInfo(0).IsName("wait"))
                    {
                        DoorController.SetBool("opendoor ?", false);
                    }
                    else
                    {
                        //check if door is open
                        if (open == true)
                        {
                            DoorController.SetBool("opendoor ?", false);
                            open = false;
                        }
                        else
                        {
                            open = true;
                            DoorController.SetBool("opendoor ?", true); //play door open animation
                        }
                    }
                }
                else
                {
                    DoorController.SetBool("unlock door", true); //play door unlock animation

                    if (State.on_unlock == Enable_Hole)
                    {
                        DoorController.SetBool("openhole ?", true);
                    }
                }
            }
        }
    }
}


