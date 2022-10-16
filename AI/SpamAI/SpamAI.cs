using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFE3D;

public class SpamAI : BaseAI
{
    /// sample code: AI that spams Buttom5
    public override void DoUpdate() {
        this.inputs.Clear();
        foreach (InputReferences input in this.inputReferences) {
            this.inputs[input] = this.ReadInput(input);
        }
    }

    public override InputEvents ReadInput(InputReferences inputReference) {
        if (inputReference.inputType == InputType.Button && inputReference.engineRelatedButton == ButtonPress.Button5) {
            return new InputEvents(Random.Range(0f, 1f) < 0.5f);
        }
        else {
            return InputEvents.Default;
        }
    }
}
