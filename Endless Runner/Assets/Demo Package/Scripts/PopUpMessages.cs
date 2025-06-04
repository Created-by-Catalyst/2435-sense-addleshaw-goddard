using System;
using UnityEngine;
using UnityEngine.UI;

public class PopUpMessages : MonoBehaviour
{
    [SerializeField] Image messageImage;

    [SerializeField] Animator anim;

    [SerializeField] public PopUpTextures popUpTextures;

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

            case "ObstacleQuestions":
                DisplayMessage(popUpTextures.questions);
                break;
            case "ObstackeRedTapeHurdle":
                DisplayMessage(popUpTextures.redTape);
                break;
            case "ObstacleWarningSign":
                DisplayMessage(popUpTextures.warningSign);
                break;
            case "ObstacleBananaCrates":
                DisplayMessage(popUpTextures.bananaCrates);
                break;
            case "ObstaclePaperVortex":
                DisplayMessage(popUpTextures.paperVortex);
                break;
            case "ObstaclePaperVortexPatrolling":
                DisplayMessage(popUpTextures.paperVortex);
                break;
            case "ObstacleOilSlick":
                DisplayMessage(popUpTextures.oilSlick);
                break;

            case "ExtraLife":
                DisplayMessage(popUpTextures.life);
                break;
            case "GoFurther-Invincibility":
                DisplayMessage(popUpTextures.ai);
                break;
            case "ScoreMultiplier":
                DisplayMessage(popUpTextures.multiplier);
                break;
            case "GoFaster-SpeedInc":
                DisplayMessage(popUpTextures.speed);
                break;
            case "Resource":
                DisplayMessage(popUpTextures.magnet);
                break;
            default:
                break;
        }
    }


    public void DisplayMessage(Sprite texture)
    {

        //print("attempt display");

        messageImage.sprite = texture;

        anim.SetTrigger("popup");
    }



    [Serializable]
    public class PopUpTextures
    {
        public Sprite redTape;
        public Sprite bananaCrates;
        public Sprite oilSlick;
        public Sprite paperVortex;
        public Sprite warningSign;
        public Sprite questions;


        public Sprite multiplier;
        public Sprite ai;
        public Sprite life;
        public Sprite magnet;
        public Sprite speed;

    }
}

