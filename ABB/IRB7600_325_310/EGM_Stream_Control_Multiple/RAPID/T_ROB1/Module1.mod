MODULE Module1
    ! Targets
    CONST robtarget P1 := [[1773.63,-287.31,92.37],[8.75873E-06,-1.19983E-06,-1,6.96016E-07],[-1,0,-1,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget P2 := [[1380.38,1119.81,64.77],[2.64893E-05,0.0181086,-0.999836,1.00795E-05],[0,-1,0,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget P3 := [[2043.85,1000.43,-65.59],[3.11237E-05,-2.11287E-06,-1,6.22279E-06],[0,-1,1,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget P4 := [[1359.12,-183.33,35.71],[1.11593E-05,-4.04865E-06,-1,-3.67682E-06],[-1,0,-1,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget P5 := [[1773.66,-287.32,35.72],[5.71756E-06,-4.42319E-06,-1,-3.51103E-07],[-1,0,-1,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget P6 := [[2046.76,-166.95,92.34],[3.47833E-05,1.92386E-06,-1,-6.87548E-06],[-1,0,0,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget P7 := [[2179.22,256.64,92.38],[1.67054E-05,3.20947E-05,-1,-5.605E-06],[0,-1,0,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    CONST robtarget P8 := [[1773.65,-287.31,92.39],[1.04985E-05,-1.1174E-06,-1,-1.91062E-07],[-1,0,-1,0],[9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];
    
    CONST robtarget P{8} := [P1, P2, P3, P4, P5, P6, P7, P8];
    
    ! Identifier for the EGM correction
    LOCAL VAR egmident egm_id;
    
    ! Actual State {Main State-Machine)
    VAR num actual_state := 0;
    ! Index of the Path
    VAR num p_index := 0;
    
    ! Initialization Parameters
    CONST num speed_tcp    := 50;
    CONST num speed_orient := 50;
    CONST speeddata speed_value := [speed_tcp,speed_orient,0,0];
    CONST zonedata zone_value := fine;
    
    ! Description:                                          !
    ! Simple movements with EGM Streaming Pos. - Main Cycle !
    PROC Main()
        TEST actual_state
            CASE 0:
                ! Register an EGM id
                EGMGetId egm_id;
                ! Setup the EGM communication
                EGMSetupUC ROB_1, egm_id, "default", "ROB_1";
                ! Start the EGM communication session
                EGMStreamStart egm_id \SampleRate:=4;
                
                ! Wait Time (2s)
                WaitTime 2;
                    
                actual_state := 1; 
            CASE 1:
                FOR i FROM 1 TO Dim(P, 1) DO
                    ! Move to the starting position for a point in index {n}
                    MoveJ P{i}, v200, fine, ABB_GRIPPER\WObj:=wobj0;
                    
                    ! Wait Time (2s)
                    WaitTime 2;
                    ! Instructions for multiple movements for a point in index {n}
                    Path_Experiment_1 P{i}, speed_value, zone_value;
                    ! Wait Time (2s)
                    WaitTime 2;
                    
                    ! Increase in value after a successfully executed path for a point in index {n}
                    p_index := p_index + 1;
                ENDFOR
                
                actual_state := 3;
            CASE 2:
                ! Instructions for multiple movements for a point in index {1}
                Path_Experiment_1 P{1}, speed_value, zone_value;
                ! Wait Time (2s)
                WaitTime 2;
                actual_state := 3;
            CASE 3:
                ! Move to the starting position
                MoveL P{1}, v200, fine, ABB_GRIPPER\WObj:=wobj0;
                
                actual_state := 4;
            CASE 4:
                ! Empty
        ENDTEST
                
        ! Stop and Release the EGM id
        !EGMStreamStop egm_id;
        !EGMReset egm_id;
    ENDPROC
    
    PROC Path_Experiment_1(robtarget rob_target, speeddata speed, zonedata zone)
        ! Block 1
        MoveL Offs (rob_target, 0, 300, 0), speed, zone, ABB_GRIPPER;
        MoveL rob_target, speed, zone, ABB_GRIPPER;
        MoveL Offs (rob_target, 0, 300, 0), v100, zone, ABB_GRIPPER;
        MoveL rob_target, v100, zone, ABB_GRIPPER;
        MoveL Offs (rob_target, 0, 300, 0), v500, zone, ABB_GRIPPER;
        MoveL rob_target, v500, zone, ABB_GRIPPER;
        ! Block 2      
        MoveL Offs (rob_target, 300, 0, 0), speed, zone, ABB_GRIPPER;
        MoveL rob_target, speed, zone, ABB_GRIPPER;
        MoveL Offs (rob_target, 300, 0, 0), v100, zone, ABB_GRIPPER;
        MoveL rob_target, v100, zone, ABB_GRIPPER;
        MoveL Offs (rob_target, 300, 0, 0), v500, zone, ABB_GRIPPER;
        MoveL rob_target, v500, zone, ABB_GRIPPER;
        ! Block 3
        MoveL Offs (rob_target, 300, 300, 0), speed, zone, ABB_GRIPPER;
        MoveL rob_target, speed, zone, ABB_GRIPPER;
        MoveL Offs (rob_target, 300, 300, 0), v100, zone, ABB_GRIPPER;
        MoveL rob_target, v100, zone, ABB_GRIPPER;
        MoveL Offs (rob_target, 300, 300, 0), v500, zone, ABB_GRIPPER;
        MoveL rob_target, v500, zone, ABB_GRIPPER;
    ENDPROC
ENDMODULE