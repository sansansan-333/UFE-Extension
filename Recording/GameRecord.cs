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
    public int version = 1; 
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
        public GameState gameState;
        public CharacterState p1GameState;
        public CharacterState p2GameState;
        public Input p1Input;
        public Input p2Input;
    }

    [Serializable]
    public class GameState {
        public float normalizedDistance;
    }

    [Serializable]
    public class CharacterState {
        public int life;
        public bool isDown;
        public bool isJumping;
        public bool isBlocking;
        public int frameAdvantage;
    }

    [Serializable]
    public class Input {
        public string[] buttons;
    }
}
