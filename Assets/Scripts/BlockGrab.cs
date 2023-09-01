using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BlockGrab : XRGrabInteractable
{
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Deshabilitem l'input del comandament
        args.interactorObject.transform.gameObject.
        GetComponent<UnityEngine.XR.Interaction.Toolkit.ActionBasedController>().enableInputActions = false;
        // Cridem la funció original
        base.OnSelectEntered(args);
        // Marquem una de les baquetes com a agafades al singleton, i cridem la funció CheckStart del singleton
        if (!Singleton.GetInstance().Baqueta1Grabbed) Singleton.GetInstance().Baqueta1Grabbed = true;
        else if (!Singleton.GetInstance().Baqueta2Grabbed) Singleton.GetInstance().Baqueta2Grabbed = true;
        Singleton.GetInstance().checkStart();
    }
}
