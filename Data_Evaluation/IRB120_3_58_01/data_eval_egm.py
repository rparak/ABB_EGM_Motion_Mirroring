"""
## =========================================================================== ## 
MIT License
Copyright (c) 2021 Roman Parak
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
## =========================================================================== ## 
Author   : Roman Parak
Email    : Roman.Parak@outlook.com
Github   : https://github.com/rparak
File Name: data_eval_egm.py
## =========================================================================== ## 
"""

# System (Default)
import sys
# Pandas (Data analysis and manipulation) [pip3 install pandas]
import pandas as pd
# Numpy (Array computing) [pip3 install numpy]
import numpy as np
# Matplotlib (Visualization) [pip3 install matplotlib]
import matplotlib.pyplot as plt
# SciencePlots (Matplotlib styles for scientific plotting) [pip3 install SciencePlots]
import scienceplots
# OS (Operating system interfaces)
import os

def main():
    # Plot Data:
    #   'Position' or 'Orientation'
    PLOT_DATA = 'Position'
    # Path Name: 
    #   Path_Experiment_1_T1: Path with the same orientation.
    #   Path_Experiment_1_T2: Path with different orientation in the Z axis.
    #   Path_Experiment_1_T3: Path with different orientation in X, Y, Z axes.
    RESULT_PATH_NAME  = 'Path_Experiment_1_T1'
    # Speed: 
    #   v40, v80, v150
    SPEED = 'v40'
    # Zone:
    #   z0, z10, z100
    ZONE  = 'z0'

    # Read Data from the File (EGM_Results Folder)
    current_directory_name = os.getcwd()
    column_headings = ['Sequence','Time', 
                       'X_Desired', 'Y_Desired', 'Z_Desired',
                       'Q1_Desired', 'Q2_Desired', 'Q3_Desired', 'Q4_Desired',
                       'X_Actual', 'Y_Actual', 'Z_Actual',
                       'Q1_Actual', 'Q2_Actual', 'Q3_Actual', 'Q4_Actual',]
    egm_res_data = pd.read_csv(current_directory_name + '\\EGM_Results\\' + RESULT_PATH_NAME +'\\result_' + SPEED +'_' + ZONE + '.txt', names=column_headings)

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

    # Set the parameters for the scientific style.
    plt.style.use(['science'])

    if PLOT_DATA == 'Position':
        ax_vector = [0]*len(desired_position)

        # Create figure with multiple subplots
        figure, (ax_vector) = plt.subplots(len(ax_vector), 1)
        figure.suptitle(f'Path Name: {RESULT_PATH_NAME} ({PLOT_DATA})\n[Speed: {SPEED} mm/s $\&$ {SPEED} °/s, Zone: {ZONE}]', fontsize = 15)

        AXIS_NAME = ['x', 'y', 'z']
        for i, (ax, desired, actual) in enumerate(zip(ax_vector, desired_position, actual_position)):
            ax.set_ylim([np.minimum.reduce([np.minimum.reduce(desired), np.minimum.reduce(actual)]) - 10.0, 
                         np.maximum.reduce([np.maximum.reduce(desired), np.maximum.reduce(actual)]) + 10.0])
            ax.plot(time, desired, color=[0.2,0.4,0.6], label=f'Desired {PLOT_DATA}')
            ax.plot(time, actual, color=[0.8,0.4,0], label=f'Actual {PLOT_DATA}')
            ax.grid(which='major', linewidth = 0.15, linestyle = '--')
            ax.set_xlabel(r'Time in milliseconds', fontsize=15, labelpad=10)
            ax.set_ylabel(f'{AXIS_NAME[i]}-pos. in mm', fontsize=15, labelpad=10)
            ax.legend()

    elif PLOT_DATA == 'Orientation':
        ax_vector = [0]*len(desired_orientation)

        # Create figure with multiple subplots
        figure, (ax_vector) = plt.subplots(len(ax_vector), 1)
        figure.suptitle(f'Path Name: {RESULT_PATH_NAME} ({PLOT_DATA})\n[Speed: {SPEED} mm/s $\&$ {SPEED} °/s, Zone: {ZONE}]', fontsize = 15)

        AXIS_NAME = ['q1', 'q2', 'q3', 'q4']

        for i, (ax, desired, actual) in enumerate(zip(ax_vector, desired_orientation, actual_orientation)):
            ax.set_ylim([-1.0 - 0.1, 1.0 + 0.1])
            ax.plot(time, desired, color=[0.2,0.4,0.6], label=f'Desired {PLOT_DATA}')
            ax.plot(time, actual, color=[0.8,0.4,0], label=f'Actual {PLOT_DATA}')
            ax.grid(which='major', linewidth = 0.15, linestyle = '--')
            ax.set_xlabel(r'Time in milliseconds', fontsize=15, labelpad=10)
            ax.set_ylabel(f'{AXIS_NAME[i]}-orient. in (-)', fontsize=15, labelpad=10)
            ax.legend()

    # Display the result
    plt.show()

if __name__ == '__main__':
    sys.exit(main())
