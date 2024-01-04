using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePicker : MonoBehaviour
{
    public bool lvl_1 = false;
    public bool lvl_2 = true;
    public bool lvl_3 = false;
    public bool lvl_4 = false;
    private GameObject Level_1;
    private GameObject Level_2;
    private GameObject Level_3;
    private GameObject Level_4;
    public bool canTravel = false;
    private GameObject CanTravelImg;
    private GameObject CanNotTravelImg;
    private GameObject leftButton;
    private GameObject rightButton;
    private GameObject DepatureButton;
    private ComputerPult_DepatureButton ComputerPult_DepatureButton_Script;
    private ComputerPult_LeftButton ComputerPult_LeftButton_Script;
    private ComputerPult_RightButton ComputerPult_RightButton_Script;
    void Start()
    {
        Level_1 = GameObject.Find("LVL_1");
        Level_2 = GameObject.Find("LVL_2");
        Level_3 = GameObject.Find("LVL_3");
        Level_4 = GameObject.Find("LVL_4");
        Level_1.SetActive(false);
        Level_2.SetActive(true);
        Level_3.SetActive(false);
        Level_4.SetActive(false);
        CanTravelImg = GameObject.Find("CanTravel_img");
        CanNotTravelImg = GameObject.Find("CanNotTravel_img");
        leftButton = GameObject.Find("leftButton");
        rightButton = GameObject.Find("rightButton");
        DepatureButton = GameObject.Find("DepatureButton");
        ComputerPult_LeftButton_Script = leftButton.GetComponent<ComputerPult_LeftButton>();
        ComputerPult_RightButton_Script = rightButton.GetComponent<ComputerPult_RightButton>();
        ComputerPult_DepatureButton_Script = DepatureButton.GetComponent<ComputerPult_DepatureButton>();
    }

    void Update()
    {
        travelChekker();
        Button_Input();
    }
  void Button_Input()
    {
        if (ComputerPult_LeftButton_Script.ButtonDown == true)
        {
            leftbuttonDown();
            print(" left button is down ");
            ComputerPult_LeftButton_Script.ButtonDown = false;
        }

        if (ComputerPult_RightButton_Script.ButtonDown == true)
        {
            rightbuttonDown();
            print(" right button is down ");
            ComputerPult_RightButton_Script.ButtonDown = false;
        }
    }

  void travelChekker()
    {
        if (canTravel == true && ComputerPult_DepatureButton_Script.ButtonDown == true)
        {
                if (lvl_1 == true)
                {
                    print(" JUMP to Scene 1 ");
                    SceneManager.LoadScene(0);
                }
                else if (lvl_2 == true)
                {
                    print(" JUMP to Scene 2 ");
                    SceneManager.LoadScene(1);
                }
                else if (lvl_3 == true)
                {
                    print(" JUMP to Scene 3 ");
                    SceneManager.LoadScene(2);
                }
                else if (lvl_4 == true)
                {
                    print(" JUMP to Scene 4 ");
                    SceneManager.LoadScene(3);
                
            }

        }

        // Skifter billedet på skærmen
        if (canTravel == false)
        {
            CanNotTravelImg.SetActive(true);
            CanTravelImg.SetActive(false);
        }
        else
        {
            CanNotTravelImg.SetActive(false);
            CanTravelImg.SetActive(true);  
        }
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        print("knappen er i bund");
    }
    */
    void rightbuttonDown()
    {
        if (lvl_1 == true)
        {
            lvl_1 = false;
            lvl_2 = true;
            Level_1.SetActive(false);
            Level_2.SetActive(true);
            canTravel = false;
        }
        else if (lvl_2 == true)
        {
            lvl_2 = false;
            lvl_3 = true;
            Level_2.SetActive(false);
            Level_3.SetActive(true);
            canTravel = true;
        }
        else if (lvl_3 == true)
        {
            lvl_3 = false;
            lvl_4 = true;
            Level_3.SetActive(false);
            Level_4.SetActive(true);
            canTravel = false;
        }
        else if (lvl_4 == true)
        {
            lvl_4 = false;
            lvl_1 = true;
            Level_4.SetActive(false);
            Level_1.SetActive(true);
            canTravel = false;

        }
    }

    void leftbuttonDown()
    {
        if (lvl_1 == true)
        {
            lvl_1 = false;
            lvl_4 = true;
            Level_1.SetActive(false);
            Level_4.SetActive(true);
            canTravel = false;
        }
        else if (lvl_4 == true)
        {
            lvl_4 = false;
            lvl_3 = true;
            Level_4.SetActive(false);
            Level_3.SetActive(true);
            canTravel = true;
        }
        else if (lvl_3 == true)
        {
            lvl_3 = false;
            lvl_2 = true;
            Level_3.SetActive(false);
            Level_2.SetActive(true);
            canTravel = false;
        }
        else if (lvl_2 == true)
        {
            lvl_2 = false;
            lvl_1 = true;
            Level_2.SetActive(false);
            Level_1.SetActive(true);
            canTravel = false;
        }
    }
}

