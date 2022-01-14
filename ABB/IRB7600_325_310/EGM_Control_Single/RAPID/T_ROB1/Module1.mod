MODULE Module1
    ! Identifier for the EGM correction
    LOCAL VAR egmident egm_id;
    ! EGM pose frames
    LOCAL CONST pose egm_correction_frame := [[0.0, 0.0, 0.0],[1.0, 0.0, 0.0, 0.0]];
    LOCAL CONST pose egm_sensor_frame     := [[0.0, 0.0, 0.0],[1.0, 0.0, 0.0, 0.0]];
    ! The work object. Base Frame
    LOCAL PERS wobjdata egm_wobj   := [FALSE,TRUE,"",[[0,0,0],[1,0,0,0]],[[0,0,0],[1,0,0,0]]];
    ! Limits for convergence
    ! Cartesian: +- 0.1 mm
    LOCAL CONST egm_minmax egm_condition_cartesian := [-0.1, 0.1];
    ! Orientation: +- 0.1 degrees
    LOCAL CONST egm_minmax egm_condition_orient := [-0.1, 0.1];

    ! Initial targets of the Robot {Master}
    CONST robtarget P1 := [[1773.63,-287.31,92.37],[8.75873E-06,-1.19983E-06,-1,6.96016E-07],[-1,0,-1,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget P2 := [[1380.38,1119.81,64.77],[2.64893E-05,0.0181086,-0.999836,1.00795E-05],[0,-1,0,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget P3 := [[2043.85,1000.43,-65.59],[3.11237E-05,-2.11287E-06,-1,6.22279E-06],[0,-1,1,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget P4 := [[1359.12,-183.33,35.71],[1.11593E-05,-4.04865E-06,-1,-3.67682E-06],[-1,0,-1,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget P5 := [[1773.66,-287.32,35.72],[5.71756E-06,-4.42319E-06,-1,-3.51103E-07],[-1,0,-1,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget P6 := [[2046.76,-166.95,92.34],[3.47833E-05,1.92386E-06,-1,-6.87548E-06],[-1,0,0,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget P7 := [[2179.22,256.64,92.38],[1.67054E-05,3.20947E-05,-1,-5.605E-06],[0,-1,0,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget P8 := [[1773.65,-287.31,92.39],[1.04985E-05,-1.1174E-06,-1,-1.91062E-07],[-1,0,-1,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    
    ! Description:                                !
    ! Externally Guided motion (EGM) - Main Cycle !
    PROC main()
        ! Move to the starting position
        !MoveJ P1,v500,fine,ABB_GRIPPER\WObj:=egm_wobj;
        
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
                       \Tool:=ABB_GRIPPER,
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
                       \MaxSpeedDeviation:=100;
                        
            ! Start the EGM communication session
            EGMRunPose egm_id, EGM_STOP_RAMP_DOWN, \X \Y \Z \Rx \Ry \Rz \CondTime:=2500 \RampInTime:=0.1 \RampOutTime:=0.1 \PosCorrGain:=1.0;
            ! Release the EGM id
            !EGMReset egm_id;
            ! Wait 2 seconds {No data from EGM sensor}
            !WaitTime 2;
        ENDWHILE
        
        ERROR
        IF ERRNO = ERR_UDPUC_COMM THEN
            TPWrite "Communication timedout";
            TRYNEXT;
        ENDIF
    ENDPROC
ENDMODULE