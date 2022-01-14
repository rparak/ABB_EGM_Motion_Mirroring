# System (Default)
import sys
# Pandas (Data analysis and manipulation) [pip3 install pandas]
import pandas as pd
# Numpy (Array computing) [pip3 install numpy]
import numpy as np
# Matplotlib (Visualization) [pip3 install matplotlib]
import matplotlib.pyplot as plt
# IO (Core tool for working with streams)
from io import StringIO
# OS (Operating system interfaces)
import os

def main():
    # Path Name: 
    #   Path_Experiment_1_T1: The path starts at eight different points P1-8 and ends at P1.
    #   Path_Experiment_1_T1_8: The path starts at P1.
    #   Folders for individual tests:
    #       Test_Path_Experiment_1_T1: The path starts at eight different points P1-8 and ends at P1.
    RESULT_PATH_NAME  = 'Path_Experiment_1_T1_8'
    # Speed: 
    #   v50
    SPEED = 'v50'
    # Zone:
    #   zFine, z0, z10
    ZONE  = 'zFine'

    # Path visibility
    visible_target, visible_desired_pos, visible_actual_pos = True, True, True

    # Read Data from the File (EGM_Results Folder)
    current_directory_name = os.getcwd()
    column_headings = ['Sequence','Time', 
                       'X_Desired', 'Y_Desired', 'Z_Desired',
                       'Q1_Desired', 'Q2_Desired', 'Q3_Desired', 'Q4_Desired',
                       'X_Actual', 'Y_Actual', 'Z_Actual',
                       'Q1_Actual', 'Q2_Actual', 'Q3_Actual', 'Q4_Actual',]
    egm_res_data = pd.read_csv(current_directory_name + '\\EGM_Results\\' + RESULT_PATH_NAME +'\\result_' + SPEED +'_' + ZONE +'.txt', names=column_headings)

    # Assign data to variables
    #   Sequence [-]
    sequence = egm_res_data['Sequence']
    #   Time [ms]
    time = egm_res_data['Time']
    #   Position [mm]
    desired_position = [egm_res_data['X_Desired'], egm_res_data['Y_Desired'], egm_res_data['Z_Desired']]
    actual_position  = [egm_res_data['X_Actual'], egm_res_data['Y_Actual'], egm_res_data['Z_Actual']]
    #   Orientation [-]
    desired_orientation = [egm_res_data['Q1_Desired'], egm_res_data['Q2_Desired'], egm_res_data['Q3_Desired'], egm_res_data['Q4_Desired']]
    actual_orientation  = [egm_res_data['Q1_Actual'], egm_res_data['Q2_Actual'], egm_res_data['Q3_Actual'], egm_res_data['Q4_Actual']]

    if visible_target == True:
        # Read Data from the File (Targets Folder)
        with open(current_directory_name + '\\Targets\\' + RESULT_PATH_NAME + '.txt') as file:
            target_data = file.read()

        # Remove unnecessary symbols
        for symbol in ['CONST robtarget ',':=[[', '],[', '9E+09,9E+09,9E+09,9E+09,9E+09,9E+09]];', ',,']:
            if symbol in target_data:
                target_data = target_data.replace(symbol,',')

        # Sort data
        target_data_sort = pd.read_csv(StringIO(target_data), names=['None1','Target Name', 'X', 'Y', 'Z', 'Q1', 'Q2', 'Q3', 'Q4', 'CF1', 'CF4', 'CF6', 'CFX', 'None2'])
        target_data_sort = target_data_sort.drop(columns=['None1', 'None2'])

        # Assign data to variables
        #   Position [mm]
        target_position = [target_data_sort['X'], target_data_sort['Y'], target_data_sort['Z']]

    # Create figure for 3d projection
    figure = plt.figure()
    axis   = figure.add_subplot(projection='3d')
    figure.suptitle(f'Path Name: {RESULT_PATH_NAME} (Position)\n[Speed: {SPEED} mm/s & {SPEED} °/s, Zone: {ZONE}]', fontsize = 15)
    # Display of the connected path with landmarks
    if visible_target == True: axis.scatter(target_position[0], target_position[1], target_position[2], s=50,color=[0.0,0.0,0.0,1.0],label=r'Targets')
    # Display Desired position (X, Y, Z)
    if visible_desired_pos == True: axis.plot(desired_position[0], desired_position[1], desired_position[2], color=[0.2,0.4,0.6], label=r'Desired Position')
    # Display Actual position (X, Y, Z)
    if visible_actual_pos == True: axis.plot(actual_position[0], actual_position[1], actual_position[2], color=[0.8,0.4,0], label=r'Actual Position')

    # Axis Parameters:
    #   Limits
    #axis.set_xlim(np.minimum.reduce(desired_position[0]) - 500, np.maximum.reduce(desired_position[0]) + 500)
    #axis.set_ylim(np.minimum.reduce(desired_position[1]) - 500, np.maximum.reduce(desired_position[1]) + 500)
    axis.set_zlim(np.minimum.reduce(desired_position[2]) - 500, np.maximum.reduce(desired_position[2]) + 500)
    #   Label
    axis.set_xlabel(r'X - Position (mm)')
    axis.set_ylabel(r'Y - Position (mm)')
    axis.set_zlabel(r'Z - Position (mm)')
    # Other
    axis.xaxis._axinfo["grid"].update({'linewidth': 0.75, 'linestyle': '--'})
    axis.w_xaxis.pane.set_color((1.0, 1.0, 1.0, 1.0))
    axis.yaxis._axinfo["grid"].update({'linewidth': 0.75, 'linestyle': '--'})
    axis.w_yaxis.pane.set_color((1.0, 1.0, 1.0, 1.0))
    axis.zaxis._axinfo["grid"].update({'linewidth': 0.75, 'linestyle': '--'})
    axis.w_zaxis.pane.set_color((1.0, 1.0, 1.0, 1.0))
    axis.legend()

    # Display the result
    plt.show()

if __name__ == '__main__':
    sys.exit(main())
