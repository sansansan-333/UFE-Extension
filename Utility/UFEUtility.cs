using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UFE3D;

public class UFEUtility
{
    /// <summary>
    /// Returns number of frame advantage on the side of the specified player.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    /// <remarks>
    /// Returns 0 if called outside of battle.
    /// </remarks>
    public static int CalcFrameAdvantage(int player) {
        var p1 = UFE.GetPlayer1ControlsScript();
        var p2 = UFE.GetPlayer2ControlsScript();

        if (p1 == null || p2 == null) return 0;

        
        float p1MinusTime = 0, p2MinusTime= 0;
        // p1
        // if stunned or blocking
        if (p1.currentSubState == SubStates.Stunned || p1.blockStunned) {
            p1MinusTime += (float)p1.stunTime;
        }
        
        // if casting move
        if(p1.currentMove != null) {
            p1MinusTime += (float)(p1.currentMove.totalFrames - p1.currentMove.currentTick) / UFE.config.fps;
        }

        // p2
        // if stunned or blocking
        if (p2.currentSubState == SubStates.Stunned || p2.blockStunned) {
            p2MinusTime += (float)p2.stunTime;
        }

        // if casting move
        if (p2.currentMove != null) {
            p2MinusTime += (float)(p2.currentMove.totalFrames - p2.currentMove.currentTick) / UFE.config.fps;
        }

        int frameAdvantage = (int)((p2MinusTime - p1MinusTime) * UFE.config.fps);

        if (player == 1) return frameAdvantage;
        else return frameAdvantage * -1;
    }
}
