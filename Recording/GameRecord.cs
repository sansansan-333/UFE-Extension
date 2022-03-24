using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Data that represents one round of a gameplay
/// </summary>
[Serializable]
public class GameRecord
{
    // integer to distinguish different data format
    public int version = 0; 
    // unique id
    public string UUID;
    // date
    public string date;
    // description
    public string description;
    // frame data
    public List<FrameData> frames;


    [Serializable]
    public class FrameData {
        public int currentFrame;
        public CharacterState p1GameState;
        public CharacterState p2GameState;
        public Input p1Input;
        public Input p2Input;
    }

    [Serializable]
    public class CharacterState {
        public int life;
    }

    [Serializable]
    public class Input {
        public int buttons;
    }
}
