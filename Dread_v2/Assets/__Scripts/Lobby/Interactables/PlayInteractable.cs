using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DreadInc
{
    public class PlayInteractable : InteractableController
    {
        public Launcher launcher;

        public override void Interact()
        {
            if (launcher == null)
            {
                print("Launcher reference not set on " + this.name);
                return;
            }
            launcher.Connect();
        }
    }
}
