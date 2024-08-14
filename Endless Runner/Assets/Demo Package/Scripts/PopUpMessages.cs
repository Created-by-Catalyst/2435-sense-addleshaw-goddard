using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PopUpMessages : MonoBehaviour
{
    [SerializeField] TMP_Text messageText;

    [SerializeField] Animator anim;

    public static PopUpMessages Instance;

    void Awake()
    {
        Instance = this;
    }

    public void ConvertObjectToMessage(string obj)
    {

        string formattedObj = obj;


        if (obj.Contains(' '))
        {
            formattedObj = obj.Substring(0, obj.IndexOf(' '));
        }
        else if (obj.Contains('('))
        {
            formattedObj = obj.Substring(0, obj.IndexOf('('));
        }

        print("convert this: " + formattedObj);

        switch (formattedObj)
        {
            case "ObstacleCompromisedSmartHub":
                DisplayMessage("Smart Hub!");
                break;
            case "ObstackeFirewallBlockade":
                DisplayMessage("Firewall!");
                break;
            case "ObstacleAutonomousDroneSurveillance":
                DisplayMessage("Drone!");
                break;
            case "ObstacleSocialMediaFirestorm":
                DisplayMessage("Social Media Firestorm!");
                break;
            case "ObstacleDataCloud":
                DisplayMessage("Data Cloud!");
                break;
            case "ObstacleSocialMediaFirestormPatrolling":
                DisplayMessage("Social Media Firestorm!");
                break;


            case "ObstacleQuestions":
                DisplayMessage("Bombarded with Questions! Client queries are creating roadblocks.");
                break;
            case "ObstackeRedTapeHurdle":
                DisplayMessage("Caught in Red Tape! Bureaucratic delays are holding you back.");
                break;
            case "ObstacleWarningSign":
                DisplayMessage("Warning Ahead! Risk management issues are signalling trouble.");
                break;
            case "ObstaclePaperVortex":
                DisplayMessage("Overwhelmed by Paperwork! Excessive documentation is bogging you down.");
                break;
            case "ObstaclePaperVortexPatrolling":
                DisplayMessage("Overwhelmed by Paperwork! Excessive documentation is bogging you down.");
                break;
            case "ObstacleOilSlick":
                DisplayMessage("Oil Slick! Your business is facing reputational danger.");
                break;

            case "ExtraLife":
                DisplayMessage("Legal Backup! Extra support is keeping you in the game.");
                break;
            case "Invincibility":
                DisplayMessage("AI to the Rescue! Advanced technology is clearing the path ahead.");
                break;
            case "ScoreMultiplier":
                DisplayMessage("Cash Influx! Your business is securing more financial resources.");
                break;
            case "SpeedIncrease":
                DisplayMessage("Lightning Speed! You're accelerating to maximise your gains.");
                break;
            case "Resource":
                DisplayMessage("Reinforcements Arrived! Additional resources are boosting your efforts.");
                break;
            default:
                break;
        }
    }


    public void DisplayMessage(string message)
    {

        print("attempt display");

        messageText.text = message;

        anim.Play("PopUpAnim");
    }


}
