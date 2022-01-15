 /****************************************************************************
MIT License
Copyright(c) 2021 Roman Parak
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
*****************************************************************************
Author   : Roman Parak
Email    : Roman.Parak @outlook.com
Github   : https://github.com/rparak
File Name: Program.cs
****************************************************************************/

// System Lib.
using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
// ABB EGM Lib.
using abb.egm;

/*
    ABB EGM Notes: 
        - EGM (Externally Guided Motion) is an interface for ABB robots that allows smoothness control of the robot 
          arm from an external application. 
        - EGM can also be used to stream current positions from the robot in either joint/cartesian 
          space.  
        - The application communicates with the ABB robot via UDP (User Datagram Protocol)

    For more detailed information on EGMs, please refer to the application guide on the ABB website.
 */
namespace EGM_Stream_Control_Multiple
{
    public static class EGM_Stream_Data
    {
        // IP Port Number
        public static int port_number = 6512;
        // Position
        public static double X = 0;
        public static double Y = 0;
        public static double Z = 0;
        // Orientation {Quaternion}
        public static double Q1 = 0;
        public static double Q2 = 0;
        public static double Q3 = 0;
        public static double Q4 = 0;
    }

    public static class EGM_Control_Data
    {
        // IP Port Number
        public static int port_number = 6511;
        // Sequence and Time
        public static List<uint> sequence = new List<uint>();
        public static List<uint> time     = new List<uint>();
        // Actual Position
        public static List<double> X_Actual = new List<double>();
        public static List<double> Y_Actual = new List<double>();
        public static List<double> Z_Actual = new List<double>();
        // Actual Orientation
        public static List<double> Q1_Actual = new List<double>();
        public static List<double> Q2_Actual = new List<double>();
        public static List<double> Q3_Actual = new List<double>();
        public static List<double> Q4_Actual = new List<double>();
        // Actual Position
        public static List<double> X_Desired = new List<double>();
        public static List<double> Y_Desired = new List<double>();
        public static List<double> Z_Desired = new List<double>();
        // Actual Orientation
        public static List<double> Q1_Desired = new List<double>();
        public static List<double> Q2_Desired = new List<double>();
        public static List<double> Q3_Desired = new List<double>();
        public static List<double> Q4_Desired = new List<double>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool save_data = false;

            // Keywords of the output file
            string path_name = "Path_Experiment_1_T1";
            string speed_v   = "v50";
            string zone_z    = "zFine";
            // IRB120_3_58_01 / IRB7600_325_310
            string robot = "IRB7600_325_310";

            // Start Control {EGM}: Robot Slave
            Egm_Control egm_ctrl = new Egm_Control();
            egm_ctrl.Start();

            // Start Stream {EGM}: Robot Master
            Egm_Stream egm_stream = new Egm_Stream();
            egm_stream.Start();

            Console.WriteLine("[INFO] Stop (y):");
            // Stop communication and save the values to a file
            string stop_rs = Convert.ToString(Console.ReadLine());

            if (stop_rs == "y")
            {
                if (save_data == true)
                {
                    // Write Data to file (.txt)
                    Write_Data(EGM_Control_Data.sequence, EGM_Control_Data.time,
                               EGM_Control_Data.X_Desired, EGM_Control_Data.Y_Desired, EGM_Control_Data.Z_Desired,
                               EGM_Control_Data.Q1_Desired, EGM_Control_Data.Q2_Desired, EGM_Control_Data.Q3_Desired, EGM_Control_Data.Q4_Desired,
                               EGM_Control_Data.X_Actual, EGM_Control_Data.Y_Actual, EGM_Control_Data.Z_Actual,
                               EGM_Control_Data.Q1_Actual, EGM_Control_Data.Q2_Actual, EGM_Control_Data.Q3_Actual, EGM_Control_Data.Q4_Actual,
                               "C:\\Users\\romanp\\Desktop\\LINZ_JKU\\ABB_EGM_Path_Streaming\\Data_Evaluation\\" + robot + "\\EGM_Results\\" + path_name + "\\result_" + speed_v + "_" + zone_z + ".txt");

                    Console.WriteLine("[INFO] File saved successfully!");
                }

                // Stop EGM {Control / Stream}
                egm_ctrl.Stop();
                egm_stream.Stop();

                // Application quit
                Environment.Exit(0);
            }
        }

        public static void Write_Data(List<uint> sequence, List<uint> time,
                                      List<double> x_des_pos, List<double> y_des_pos, List<double> z_des_pos,
                                      List<double> q1_des_orient, List<double> q2_des_orient, List<double> q3_des_orient, List<double> q4_des_orient,
                                      List<double> x_act_pos, List<double> y_act_pos, List<double> z_act_pos,
                                      List<double> q1_act_orient, List<double> q2_act_orient, List<double> q3_act_orient, List<double> q4_act_orient,
                                      string file_path)
        {
            try
            {
                using(System.IO.StreamWriter file = new System.IO.StreamWriter(@file_path, true))
                {
                    for (int i = 0; i < sequence.Count; ++i)
                    {
                        file.WriteLine(sequence[i].ToString() + "," + time[i].ToString() + "," +
                                       x_des_pos[i].ToString() + "," + y_des_pos[i].ToString() + "," + z_des_pos[i].ToString() + "," +
                                       q1_des_orient[i].ToString() + "," + q2_des_orient[i].ToString() + "," + q3_des_orient[i].ToString() + "," + q4_des_orient[i].ToString() + "," +
                                       x_act_pos[i].ToString() + "," + y_act_pos[i].ToString() + "," + z_act_pos[i].ToString() + "," +
                                       q1_act_orient[i].ToString() + "," + q2_act_orient[i].ToString() + "," + q3_act_orient[i].ToString() + "," + q4_act_orient[i].ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Error: ", ex);
            }
        }
    }

    class Egm_Stream
    {
        private Thread sensor_thread = null;
        private UdpClient udp_client = null;
        private bool exit_thread = false;

        public void Egm_Stream_Thread()
        {
            // Create an udp server and listen on any address and the port
            // {ABB Robot Port is set from the RobotStudio ABB}
            udp_client = new UdpClient(EGM_Stream_Data.port_number);

            var end_point = new IPEndPoint(IPAddress.Any, EGM_Stream_Data.port_number);

            while (exit_thread == false)
            {
                // Get the data from the robot
                var data = udp_client.Receive(ref end_point);

                if (data != null)
                {
                    // Initialization ABB Robot {EGM READ data (position, rotation)}
                    EgmRobot robot_msg = EgmRobot.CreateBuilder().MergeFrom(data).Build();
                    // Read Desired Cartesian Position
                    EGM_Control_Data.X_Desired.Add(robot_msg.FeedBack.Cartesian.Pos.X);
                    EGM_Control_Data.Y_Desired.Add(robot_msg.FeedBack.Cartesian.Pos.Y);
                    EGM_Control_Data.Z_Desired.Add(robot_msg.FeedBack.Cartesian.Pos.Z);
                    // Read Desired Cartesian Orientation
                    EGM_Control_Data.Q1_Desired.Add(robot_msg.FeedBack.Cartesian.Orient.U0);
                    EGM_Control_Data.Q2_Desired.Add(robot_msg.FeedBack.Cartesian.Orient.U1);
                    EGM_Control_Data.Q3_Desired.Add(robot_msg.FeedBack.Cartesian.Orient.U2);
                    EGM_Control_Data.Q4_Desired.Add(robot_msg.FeedBack.Cartesian.Orient.U3);
                    // Read Actual Position 
                    EGM_Stream_Data.X = robot_msg.FeedBack.Cartesian.Pos.X;
                    EGM_Stream_Data.Y = robot_msg.FeedBack.Cartesian.Pos.Y;
                    EGM_Stream_Data.Z = robot_msg.FeedBack.Cartesian.Pos.Z;
                    // Read Actual Orientation
                    EGM_Stream_Data.Q1 = robot_msg.FeedBack.Cartesian.Orient.U0;
                    EGM_Stream_Data.Q2 = robot_msg.FeedBack.Cartesian.Orient.U1;
                    EGM_Stream_Data.Q3 = robot_msg.FeedBack.Cartesian.Orient.U2;
                    EGM_Stream_Data.Q4 = robot_msg.FeedBack.Cartesian.Orient.U3;
                }
            }
        }
        public void Start()
        {
            // Start a thread and listen to incoming messages
            sensor_thread = new Thread(new ThreadStart(Egm_Stream_Thread));
            sensor_thread.Start();
        }

        public void Stop()
        {
            // Stop and exit thread
            exit_thread = true;
            sensor_thread.Abort();
        }
    }

    class Egm_Control
    {
        private Thread sensor_thread = null;
        private UdpClient udp_client = null;
        private bool exit_thread = false;
        private uint sequence_number = 0;
        public void Egm_Control_Thread()
        {
            // Create an udp server and listen on any address and the port
            // {ABB Robot Port is set from the RobotStudio ABB}
            udp_client = new UdpClient(EGM_Control_Data.port_number);

            var end_point = new IPEndPoint(IPAddress.Any, EGM_Control_Data.port_number);

            while (exit_thread == false)
            {
                // Get the data from the robot
                var data = udp_client.Receive(ref end_point);

                if (data != null)
                {
                    // Initialization ABB Robot {EGM READ data (position, rotation)}
                    EgmRobot robot_msg = EgmRobot.CreateBuilder().MergeFrom(data).Build();
                    // Robot Parameters
                    EGM_Control_Data.sequence.Add(sequence_number);
                    EGM_Control_Data.time.Add(robot_msg.Header.Tm);
                    // Read Actual Cartesian Position
                    EGM_Control_Data.X_Actual.Add(robot_msg.FeedBack.Cartesian.Pos.X);
                    EGM_Control_Data.Y_Actual.Add(robot_msg.FeedBack.Cartesian.Pos.Y);
                    EGM_Control_Data.Z_Actual.Add(robot_msg.FeedBack.Cartesian.Pos.Z);
                    // Read Actual Cartesian Orientation
                    EGM_Control_Data.Q1_Actual.Add(robot_msg.FeedBack.Cartesian.Orient.U0);
                    EGM_Control_Data.Q2_Actual.Add(robot_msg.FeedBack.Cartesian.Orient.U1);
                    EGM_Control_Data.Q3_Actual.Add(robot_msg.FeedBack.Cartesian.Orient.U2);
                    EGM_Control_Data.Q4_Actual.Add(robot_msg.FeedBack.Cartesian.Orient.U3);

                    // Create a new EGM sensor message
                    EgmSensor.Builder egm_sensor = EgmSensor.CreateBuilder();
                    // Create a sensor message to send to the robot
                    EMG_Sensor_Message(egm_sensor);

                    using (MemoryStream memory_stream = new MemoryStream())
                    {
                        // Sensor Message
                        EgmSensor sensor_message = egm_sensor.BuildPartial();
                        sensor_message.WriteTo(memory_stream);

                        // Send message to the ABB ROBOT {UDP}
                        int bytes_sent = udp_client.Send(memory_stream.ToArray(), (int)memory_stream.Length, end_point);
                        // Check sent data
                        if (bytes_sent < 0)
                        {
                            Console.WriteLine("Error send to robot");
                        }
                    }
                }
            }
        }
        void EMG_Sensor_Message(EgmSensor.Builder egm_s)
        {
            // create a header
            EgmHeader.Builder egm_hdr = new EgmHeader.Builder();
            /*
             * SetTm: Timestamp in milliseconds (can be used for monitoring delays)
             * SetMtype: Sent by sensor, MSGTYPE_DATA if sent from robot controller
             */
            egm_hdr.SetSeqno(sequence_number++).SetTm((uint)DateTime.Now.Ticks)
                .SetMtype(EgmHeader.Types.MessageType.MSGTYPE_CORRECTION);

            egm_s.SetHeader(egm_hdr);

            // Create EGM Sensor Data
            EgmPlanned.Builder planned         = new EgmPlanned.Builder();
            EgmPose.Builder cartesian          = new EgmPose.Builder();
            EgmQuaternion.Builder orientation  = new EgmQuaternion.Builder();
            EgmCartesian.Builder position      = new EgmCartesian.Builder();

            // Set data {Position / Orientation}
            position.SetX(EGM_Stream_Data.X)
                    .SetY(EGM_Stream_Data.Y)
                    .SetZ(EGM_Stream_Data.Z);
            orientation.SetU0(EGM_Stream_Data.Q1)
                       .SetU1(EGM_Stream_Data.Q2)
                       .SetU2(EGM_Stream_Data.Q3)
                       .SetU3(EGM_Stream_Data.Q4);

            // Set data {Cartesian}
            cartesian.SetPos(position).SetOrient(orientation);

            // Bind position object to planned
            planned.SetCartesian(cartesian);
            // Bind planned to sensor object
            egm_s.SetPlanned(planned);

            return;
        }
        public void Start()
        {
            // Start a thread and listen to incoming messages
            sensor_thread = new Thread(new ThreadStart(Egm_Control_Thread));
            sensor_thread.Start();
        }
        public void Stop()
        {
            // Stop and exit thread
            exit_thread = true;
            sensor_thread.Abort();
        }
    }
}


