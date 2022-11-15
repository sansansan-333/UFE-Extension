using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFE3D;
using System;
using System.Linq;
using System.IO;

using FrameData = GameRecord.FrameData;
using GameState = GameRecord.GameState;
using CharacterState = GameRecord.CharacterState;
using Input = GameRecord.Input;

[DefaultExecutionOrder(300)] // after UFE.cs
public class GameRecorder : MonoBehaviour
{
    private GameRecord record;
    private string savePath;
    private bool isGameRecordable = false;
    private int frameRoundStarted;

    void FixedUpdate()
    {
        // since UFE.currentFrame is already counted up in Update, we need to use previous frame count
        if(isGameRecordable) RecordCurrentFrame((int)(UFE.currentFrame - 1));
    }

    public void Initialize(string savePath) {
        this.savePath = savePath;

        // only during a round, isGameRecordable is true
        UFE.OnRoundBegins += (round) => { InitializeRecord((int)(UFE.currentFrame - 1)); isGameRecordable = true; };
        UFE.OnRoundEnds += (winner, loser) => { isGameRecordable = false; };

        // if round is over, save record
        UFE.OnRoundEnds += (winner, loser) => { SaveRecord(); };
    }

    private void InitializeRecord(int frameRoundStarted) {
        this.frameRoundStarted = frameRoundStarted;

        record = new GameRecord();
        record.UUID = Guid.NewGuid().ToString();
        record.date = DateTime.Now.ToString();
        record.description = UFEExtension.Instance.ExtensionInfo.description;
        record.frames = new List<FrameData>();
    }

    private void RecordCurrentFrame(int currentFrame) {
        var history = UFE.fluxCapacitor.History;
        var pair = new KeyValuePair<FluxStates, FluxFrameInput>();

        if (!history.TryGetStateAndInput(currentFrame, out pair)) {
            Debug.LogError("Failed to get current game state.");
            return;
        }

        var p1CharacterStateHistory = pair.Key.allCharacterStates.First(elem => elem.playerNum == 1);
        var p2CharacterStateHistory = pair.Key.allCharacterStates.First(elem => elem.playerNum == 2);
        var p1InputHistory = pair.Value.Player1CurrentInput;
        var p2InputHistory = pair.Value.Player2CurrentInput;

        FrameData frame = new FrameData();
        frame.currentFrame = currentFrame - frameRoundStarted;
        frame.gameState = new GameState {
            normalizedDistance = (float)p1CharacterStateHistory.normalizedDistance
        };
        frame.p1GameState = new CharacterState {
            life = (int)p1CharacterStateHistory.life,
            isDown = p1CharacterStateHistory.currentState == PossibleStates.Down,
            isJumping = (p1CharacterStateHistory.currentState == PossibleStates.BackJump || p1CharacterStateHistory.currentState == PossibleStates.NeutralJump || p1CharacterStateHistory.currentState == PossibleStates.ForwardJump)
                                && p1CharacterStateHistory.currentSubState != SubStates.Stunned,
            isBlocking = p1CharacterStateHistory.isBlocking,
            frameAdvantage = UFEUtility.CalcFrameAdvantage(1),
        };
        frame.p2GameState = new CharacterState {
            life = (int)p2CharacterStateHistory.life,
            isDown = p2CharacterStateHistory.currentState == PossibleStates.Down,
            isJumping = (p2CharacterStateHistory.currentState == PossibleStates.BackJump || p2CharacterStateHistory.currentState == PossibleStates.NeutralJump || p2CharacterStateHistory.currentState == PossibleStates.ForwardJump)
                                && p2CharacterStateHistory.currentSubState != SubStates.Stunned,
            isBlocking = p2CharacterStateHistory.isBlocking,
            frameAdvantage = UFEUtility.CalcFrameAdvantage(2),
        };
        frame.p1Input = new Input {
            buttons = p1InputHistory.buttons.ToButtonPresses().Select(elem => elem.ToString()).ToArray()
        };
        frame.p2Input = new Input {
            buttons = p2InputHistory.buttons.ToButtonPresses().Select(elem => elem.ToString()).ToArray()
        };

        record.frames.Add(frame);
    }

    private void SaveRecord() {
        string json = JsonUtility.ToJson(record, true);
        string fileName = $"game_record_{record.UUID}.json";

        if (savePath[savePath.Length - 1] != '/') savePath += "/";

        if (Directory.Exists(savePath)) {
            File.WriteAllText(savePath + fileName, json);
        }
        else {
            Debug.LogError("Couldn't save a game recording because the folder does not exist.");
        }
    }
}
