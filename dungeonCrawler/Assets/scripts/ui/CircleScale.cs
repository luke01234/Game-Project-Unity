//==================//
//script to change the scale of the crosshair depending on the movement of the player
//plans to make the scale of this representative of weapon accuracy
//==================//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CircleScale : MonoBehaviour
{ 
    //get reference to player and playermove script
    public GameObject Player;
    private playermove moveScript;
    //reference to reticle/crosshair
    private RectTransform reticle;
    //float for size of crosshair
    private float size;

    private void Awake() 
    {
        //grabs the components from the rectTransform for the canvas
        reticle = GetComponent<RectTransform>();
        //grabs all the components from the playermove script
        moveScript = Player.GetComponent<playermove>();
    }

    // Update is called once per frame
    void Update()
    {

        /*due to gravity the lowest value for momentum we can hold is 4.5, so for this test we leave 1 for a scale of 1 */
        float currentMovement = moveScript.velocity.magnitude - 3.5f;

        size = currentMovement;

        /*this will edit the current scale in the reticle RectTransform declared earlier*/
        reticle.localScale = new Vector3(size, size, 1);

    }


}
