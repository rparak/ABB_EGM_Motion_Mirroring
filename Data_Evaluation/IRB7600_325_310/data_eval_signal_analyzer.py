# System (Default)
import sys
# Pandas (Data analysis and manipulation) [pip3 install pandas]
import pandas as pd
# Numpy (Array computing) [pip3 install numpy]
import numpy as np
# Matplotlib (Visualization) [pip3 install matplotlib]
import matplotlib.pyplot as plt
# OS (Operating system interfaces)
import os

def main():
    # Plot Data:
    #   'Position' or 'Orientation'
    PLOT_DATA = 'Position'
    # Path Name: 
    #   Path_Experiment_1_T1: The path starts at eight different points P1-8 and ends at P1.
    #   Path_Experiment_1_T1_8: The path starts at P1.
    RESULT_PATH_NAME  = 'Path_Experiment_1_T1'
    # Speed: 
    #   v50
    SPEED = 'v50'
    # Zone:
    #   zFine, z0, z10
    ZONE  = 'zFine'

    # Read Data from the File (Signal_Analyzer Folder)
    current_directory_name = os.getcwd()
    signal_analyzer_data = pd.read_excel(current_directory_name + '\\Signal_Analyzer\\' + RESULT_PATH_NAME +'\\result_' + SPEED +'_' + ZONE +'.xlsx', index_col=None, header=None)
    
    # Assign data to variables
    #   Time [ms]
    time = signal_analyzer_data[signal_analyzer_data.columns[0]]
    #   Orientation Speed in Current Wobj [°/s
    orientation_speed = signal_analyzer_data[signal_analyzer_data.columns[1]]
    #   Speed in Current Wobj [mm/s
    speed = signal_analyzer_data[signal_analyzer_data.columns[2]]

    # Create a data vector (Speed) for a plot
    speed_vector = [orientation_speed, speed]
    ax_vector    = [0]*len(speed_vector)

    # Create figure with multiple subplots
    figure, (ax_vector) = plt.subplots(len(ax_vector), 1)
    figure.suptitle(f'Path Name: {RESULT_PATH_NAME} (Signal Analyzer)\n[Speed: {SPEED} mm/s & {SPEED} deg/s, Zone: {ZONE}]', fontsize = 15)

    AXIS_NAME = ['Orientation Speed in Current Wobj (deg)', 'Speed in Current Wobj (mm)']
    for i, (ax, data) in enumerate(zip(ax_vector, speed_vector)):
        ax.set_ylim([np.minimum.reduce(data[1:-1]) - 10.0, 
                     np.maximum.reduce(data[1:-1]) + 10.0])
        ax.plot(time[1:-1], data[1:-1])
        ax.grid(linewidth = 0.75, linestyle = '--')
        ax.set_xlabel(r'Time (s)')
        ax.set_ylabel(f'{AXIS_NAME[i]}')

    # Display the result
    plt.show()
if __name__ == '__main__':
    sys.exit(main())
