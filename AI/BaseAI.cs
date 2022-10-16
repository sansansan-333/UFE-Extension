using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFE3D;

/// <summary>
/// Base class of original AI
/// </summary>
public class BaseAI : RandomAI {
    public override void Initialize(IEnumerable<InputReferences> inputReferences) {
        base.Initialize(inputReferences);
    }

    public override void DoUpdate() {}

    public override void DoFixedUpdate() {}

    public override InputEvents ReadInput(InputReferences inputReference) {
        return InputEvents.Default;
    }
}