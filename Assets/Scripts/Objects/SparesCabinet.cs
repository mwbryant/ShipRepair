using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparesCabinet : Object {
    public Transform piston;

    public override bool Interact(Interaction interaction) {
        if (interaction == Interaction.RemovePiston) {
            //Give player piston
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            InteractionManager im = player.GetComponent<InteractionManager>();
            im.Equip(Instantiate(piston, player.gameObject.transform.position, Quaternion.identity).GetComponent<Tool>());
            return true;
        }
        return false;
    }
}
