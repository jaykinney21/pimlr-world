using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangeAndWeaponAssign : MonoBehaviour
{
    public MeshRenderer partOneMesh;

    public MeshRenderer partTwoMesh;

    public MeshRenderer partThreeMesh;

    public MeshRenderer[] wheelMesh;




    public GameObject[] HammerWeaponOnePrefabs;

    public GameObject[] HammerWeaponTwoPrefabs;

    public GameObject[] HammerWeaponThreePrefabs;





    public Transform weaponOneTransform, weaponTwoTransform, weaponThreeTransform;

    [SerializeField] int bodyMaterialPosition;
    [SerializeField] int moldingMaterialPosition;
    [SerializeField] int led_logoMaterialPosition;
    [SerializeField] int front_grilleMaterialPosition;
    [SerializeField] int accent_lightsMaterialPosition;
    [SerializeField] int cargo_coverMaterialPosition;
    [SerializeField] int warning_lightsMaterialPosition;
    [SerializeField] int wheel_lightsMaterialPosition;
    [SerializeField] int front_rear_fasciaMaterialPosition;
    [SerializeField] int frameMaterialPosition;
    [SerializeField] int bumpersMaterialPosition;
    [SerializeField] int spoilerMaterialPosition;
    [SerializeField] int led_lightsMaterialPosition;



    //[SerializeField] int warning_lightsMaterialPosition;




    private void Start () 
    {
        //partOneMesh.material.SetColor("_color", Random.ColorHSV());
    }

   

    public void ChangePartsColor_Hunter (Color body, Color frame, Color inner_frame, Color Lights, Color Logo, Color wheel_light) 
    {
        //WHEEL FRONT RIM
        partOneMesh.material.SetColor("_EmissionColor", wheel_light);

        //BODY LOGO
        partTwoMesh.materials[0].color = inner_frame;

        //BODY LOGO
        partTwoMesh.materials[1].color = body;

        //FRAME LOGO
        partTwoMesh.materials[1].SetColor("_EmissionColor", frame);

        //BIKE LOGO
        partTwoMesh.materials[2].SetColor("_EmissionColor", Logo);

        //WHEEL BACK RIM
        partThreeMesh.material.SetColor("_EmissionColor", wheel_light);

    }

    public void ChangePartsColor_Hacker (Color led_logo, Color upper_body, Color lower_body, Color accent_piece_one) 
    {
        //LED LOGO 
        partThreeMesh.materials[1].SetColor("_EmissionColor", led_logo);

        partThreeMesh.materials[1].color = upper_body;

        partThreeMesh.materials[0].color = lower_body;

        partThreeMesh.materials[2].SetColor("_EmissionColor", accent_piece_one);
    }

    public void ChangePartsColorAndAddWeapons_Hammer (Color body, Color molding, Color led_logo, Color cargo_cover, Color front_grille, Color accent_lights, Color warning_lights, Color molding_material, string WeaponOne, string WeaponTWo, string WeaponThree)
    {
        partOneMesh.materials[bodyMaterialPosition].color = body;

        partOneMesh.materials[moldingMaterialPosition].color = molding;

        partOneMesh.materials[led_logoMaterialPosition].color = led_logo;
        partOneMesh.materials[led_logoMaterialPosition].SetColor("_EmissionColor", led_logo);

        partOneMesh.materials[front_grilleMaterialPosition].color = front_grille;

        partOneMesh.materials[accent_lightsMaterialPosition].color = accent_lights;

        partOneMesh.materials[cargo_coverMaterialPosition].color = cargo_cover;

        partOneMesh.materials[warning_lightsMaterialPosition].color = warning_lights;
        partOneMesh.materials[warning_lightsMaterialPosition].SetColor("_EmissionColor", warning_lights);


        switch (WeaponOne)
        {
            case "Weapon 1":
                Instantiate(HammerWeaponTwoPrefabs[0], weaponTwoTransform);
                break;
            case "Weapon 2":
                Instantiate(HammerWeaponThreePrefabs[0], weaponThreeTransform);
                break;
            case "Weapon 3":
                Instantiate(HammerWeaponTwoPrefabs[1], weaponTwoTransform);
                break;
            case "Weapon 4":
                Instantiate(HammerWeaponThreePrefabs[1], weaponThreeTransform);
                break;
            case "Weapon 5":
                Instantiate(HammerWeaponTwoPrefabs[2], weaponTwoTransform);
                break;
            default:
                //Debug.Log("NONE SELECTED");
                break;
        }

        switch (WeaponTWo)
        {
            case "Weapon 1":
                Instantiate(HammerWeaponTwoPrefabs[0], weaponTwoTransform);
                break;
            case "Weapon 2":
                Instantiate(HammerWeaponThreePrefabs[0], weaponThreeTransform);
                break;
            case "Weapon 3":
                Instantiate(HammerWeaponTwoPrefabs[1], weaponTwoTransform);
                break;
            case "Weapon 4":
                Instantiate(HammerWeaponThreePrefabs[1], weaponThreeTransform);
                break;
            case "Weapon 5":
                Instantiate(HammerWeaponTwoPrefabs[2], weaponTwoTransform);
                break;
            default:
                //Debug.Log("NONE SELECTED");
                break;
        }

        switch (WeaponThree)
        {
            case "Weapon 1":
                Instantiate(HammerWeaponTwoPrefabs[0], weaponTwoTransform);
                break;
            case "Weapon 2":
                Instantiate(HammerWeaponThreePrefabs[0], weaponThreeTransform);
                break;
            case "Weapon 3":
                Instantiate(HammerWeaponTwoPrefabs[1], weaponTwoTransform);
                break;
            case "Weapon 4":
                Instantiate(HammerWeaponThreePrefabs[1], weaponThreeTransform);
                break;
            case "Weapon 5":
                Instantiate(HammerWeaponTwoPrefabs[2], weaponTwoTransform);
                break;
            default:
                Debug.Log("NONE SELECTED");
                break;
        }

    }

   
    
    
    public void ChangePartsColor_Hype (Color body,Color logo,  Color frame, Color bumpers,Color spoiler,Color led_lights,Color wheel_light) 
    {
        partOneMesh.materials[bodyMaterialPosition].color = body;

        partOneMesh.materials[led_logoMaterialPosition].color = logo;

        partOneMesh.materials[led_logoMaterialPosition].SetColor("_EmissionColor", logo);

        partOneMesh.materials[frameMaterialPosition].color = frame;

        partOneMesh.materials[bumpersMaterialPosition].color = bumpers;

        partOneMesh.materials[spoilerMaterialPosition].color = spoiler;

        partOneMesh.materials[led_lightsMaterialPosition].color = led_lights;

       


        for (int i = 0; i < wheelMesh.Length; i++)
        {
            wheelMesh[i].materials[wheel_lightsMaterialPosition].color = wheel_light;
        }

        
    }
    
    public void ChangePartsColor_Hero(Color body,Color logo,  Color front_rear_fascia, Color accent_lights) 
    {
        partOneMesh.materials[bodyMaterialPosition].color = body;

        partOneMesh.materials[led_logoMaterialPosition].color = logo;

        partOneMesh.materials[led_logoMaterialPosition].SetColor("_EmissionColor", logo);

        partOneMesh.materials[front_rear_fasciaMaterialPosition].color = front_rear_fascia;

        partOneMesh.materials[accent_lightsMaterialPosition].color = accent_lights;



        for (int i = 0; i < wheelMesh.Length; i++)
        {
            wheelMesh[i].materials[wheel_lightsMaterialPosition].color = accent_lights;
        }

        
    }
}
