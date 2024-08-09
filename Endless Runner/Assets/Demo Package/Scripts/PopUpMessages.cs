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
                DisplayMessage("Questioning Clients!");
                break;
            case "ObstackeRedTapeHurdle":
                DisplayMessage("Red Tape!");
                break;
            case "ObstacleWarningSign":
                DisplayMessage("Warning!");
                break;
            case "ObstaclePaperVortex":
                DisplayMessage("Paper Vortex!");
                break;
            case "ObstacleOilSlick":
                DisplayMessage("Oil Slick!");
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
