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
File Name: data_eval_signal_analyzer.py
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

    # Set the parameters for the scientific style.
    plt.style.use(['science'])

    # Create figure with multiple subplots
    figure, (ax_vector) = plt.subplots(len(ax_vector), 1)
    figure.suptitle(f'Path Name: {RESULT_PATH_NAME} (Signal Analyzer)\n[Speed: {SPEED} mm/s $\&$ {SPEED} °/s, Zone: {ZONE}]', y=0.95, fontsize=20)

    AXIS_NAME = ['Orientation Speed in Current Wobj in deg', 'Speed in Current Wobj in mm']
    for i, (ax, data) in enumerate(zip(ax_vector, speed_vector)):
        ax.set_ylim([np.minimum.reduce(data[1:-1]) - 1.0, 
                     np.maximum.reduce(data[1:-1]) + 1.0])
        ax.plot(time[1:-1], data[1:-1], color='#8ca8c5')
        ax.grid(which='major', linewidth = 0.15, linestyle = '--')
        ax.set_xlabel(r'Time in milliseconds', fontsize=15, labelpad=10)
        ax.set_ylabel(f'{AXIS_NAME[i]}', fontsize=15, labelpad=10)

    # Display the result
    plt.show()
if __name__ == '__main__':
    sys.exit(main())
