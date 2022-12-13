MODULE Module1
    ! ## =========================================================================== ## 
    ! MIT License
    ! Copyright (c) 2021 Roman Parak
    ! Permission is hereby granted, free of charge, to any person obtaining a copy
    ! of this software and associated documentation files (the "Software"), to deal
    ! in the Software without restriction, including without limitation the rights
    ! to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    ! copies of the Software, and to permit persons to whom the Software is
    ! furnished to do so, subject to the following conditions:
    ! The above copyright notice and this permission notice shall be included in all
    ! copies or substantial portions of the Software.
    ! THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    ! IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    ! FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    ! AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    ! LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    ! OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
    ! SOFTWARE.
    ! ## =========================================================================== ## 
    ! Author   : Roman Parak
    ! Email    : Roman.Parak@outlook.com
    ! Github   : https://github.com/rparak
    ! File Name: T_ROB1/Module1.mod
    ! ## =========================================================================== ## 
    
    ! Identifier for the EGM correction
    LOCAL VAR egmident egm_id;
    ! EGM pose frames
    LOCAL CONST pose egm_correction_frame := [[0.0, 0.0, 0.0],[1.0, 0.0, 0.0, 0.0]];
    LOCAL CONST pose egm_sensor_frame     := [[0.0, 0.0, 0.0],[1.0, 0.0, 0.0, 0.0]];
    ! The work object. Base Frame
    LOCAL PERS wobjdata egm_wobj := [FALSE, TRUE, "", [[0.0, 0.0, 0.0],[1.0, 0.0, 0.0, 0.0]], [[0.0, 0.0, 0.0],[1.0, 0.0, 0.0, 0.0]]];
    ! Limits for convergence
    ! Cartesian: +- 0.1 mm
    LOCAL CONST egm_minmax egm_condition_cartesian := [-0.1, 0.1];
    ! Orientation: +- 0.1 degrees
    LOCAL CONST egm_minmax egm_condition_orient := [-0.1, 0.1];

    ! Initial targets of the Robot {Master}
    CONST robtarget Target_10:=[[263.321960449,-68.978512397,300],[0,-0.707106781,0.707106781,0],[-1,0,-2,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    
    ! Description:                                !
    ! Externally Guided motion (EGM) - Main Cycle !
    PROC main()
        ! Move to the starting position
        !MoveJ Target_10,v50,fine,tool0\WObj:=egm_wobj;
        
        ! Call -> Cartesian Move Procedure (EGM)
        EGM_CARTESIAN_MOVE;
    ENDPROC
    
    PROC EGM_CARTESIAN_MOVE()
        ! Description:                                       !
        ! Externally Guided motion (EGM) - Cartesian Control !
        
        ! Register an EGM id
        EGMGetId egm_id;
            
        ! Setup the EGM communication
        EGMSetupUC ROB_1, egm_id, "default", "ROB_1", \Pose; 
            
        ! EGM While {Cartesian}
        WHILE TRUE DO
            ! Prepare for an EGM communication session
            EGMActPose egm_id, 
                       \WObj:=egm_wobj,
                       egm_correction_frame,
                       EGM_FRAME_BASE,
                       egm_sensor_frame,
                       EGM_FRAME_BASE
                       \X:=egm_condition_cartesian
                       \Y:=egm_condition_cartesian
                       \Z:=egm_condition_cartesian
                       \Rx:=egm_condition_orient
                       \Ry:=egm_condition_orient
                       \Rz:=egm_condition_orient
                       \LpFilter:=100
                       \SampleRate:=4
                       \MaxPosDeviation:=1000
                       \MaxSpeedDeviation:=1000;
                        
            ! Start the EGM communication session
            EGMRunPose egm_id, EGM_STOP_HOLD, \X \Y \Z \Rx \Ry \Rz \CondTime:=0.1 \RampInTime:=0.1 \RampOutTime:=0.1 \PosCorrGain:=1.0;
            
            ! Wait 2 seconds {No data from EGM sensor}
            WaitTime 2;
        ENDWHILE
        
        ERROR
        IF ERRNO = ERR_UDPUC_COMM THEN
            TPWrite "Communication timedout";
            TRYNEXT;
        ENDIF
    ENDPROC
ENDMODULE
