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
namespace EGM_Control_Example
{
    public static class EGM_Stream_Data
    {
        // IP Port Number
        public static int port_number = 6511;
        // Sequence and Time
        public static List<uint> sequence = new List<uint>();
        public static List<uint> time     = new List<uint>();
        // Actual Position
        public static List<double> X = new List<double>();
        public static List<double> Y = new List<double>();
        public static List<double> Z = new List<double>();
        // Actual Orientation
        public static List<double> Q1 = new List<double>();
        public static List<double> Q2 = new List<double>();
        public static List<double> Q3 = new List<double>();
        public static List<double> Q4 = new List<double>();
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool save_data = true;

            // Keywords of the output file
            string path_name = "Test_Path_Experiment_1_T1";
            string speed_v   = "v50";
            string zone_z    = "z10";
            // IRB120_3_58_01 / IRB7600_325_310
            string robot = "IRB7600_325_310";

            // Start Stream {EGM}: Robot
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
                    Write_Data(EGM_Stream_Data.sequence, EGM_Stream_Data.time,
                               EGM_Stream_Data.X, EGM_Stream_Data.Y, EGM_Stream_Data.Z,
                               EGM_Stream_Data.Q1, EGM_Stream_Data.Q2, EGM_Stream_Data.Q3, EGM_Stream_Data.Q4,
                               "C:\\Users\\romanp\\Desktop\\LINZ_JKU\\ABB_EGM_Path_Streaming\\Data_Evaluation\\" + robot + "\\EGM_Results\\" + path_name + "\\Read\\result_" + speed_v + "_" + zone_z + ".txt");

                    Console.WriteLine("[INFO] File saved successfully!");
                }

                // Stop Stream {EGM}: Robot
                egm_stream.Stop();

                // Application quit
                Environment.Exit(0);
            }
        }

        public static void Write_Data(List<uint> sequence, List<uint> time,
                                      List<double> x_data, List<double> y_data, List<double> z_data,
                                      List<double> q1_data, List<double> q2_data, List<double> q3_data, List<double> q4_data,
                                      string file_path)
        {
            try
            {
                using(System.IO.StreamWriter file = new System.IO.StreamWriter(@file_path, true))
                {
                    for (int i = 0; i < time.Count; ++i)
                    {
                        file.WriteLine(sequence[i].ToString() + "," + time[i].ToString() + "," +
                                       x_data[i].ToString() + "," + y_data[i].ToString() + "," + z_data[i].ToString() + "," +
                                       q1_data[i].ToString() + "," + q2_data[i].ToString() + "," + q3_data[i].ToString() + "," + q4_data[i].ToString());
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
        private uint sequence_number = 0;
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
                    // Robot Parameters
                    EGM_Stream_Data.sequence.Add(sequence_number);
                    EGM_Stream_Data.time.Add(robot_msg.Header.Tm);
                    // Read Actual Cartesian Position
                    EGM_Stream_Data.X.Add(robot_msg.FeedBack.Cartesian.Pos.X);
                    EGM_Stream_Data.Y.Add(robot_msg.FeedBack.Cartesian.Pos.Y);
                    EGM_Stream_Data.Z.Add(robot_msg.FeedBack.Cartesian.Pos.Z);
                    // Read Actual Cartesian Orientation
                    EGM_Stream_Data.Q1.Add(robot_msg.FeedBack.Cartesian.Orient.U0);
                    EGM_Stream_Data.Q2.Add(robot_msg.FeedBack.Cartesian.Orient.U1);
                    EGM_Stream_Data.Q3.Add(robot_msg.FeedBack.Cartesian.Orient.U2);
                    EGM_Stream_Data.Q4.Add(robot_msg.FeedBack.Cartesian.Orient.U3);

                    // Increase the sequence number
                    sequence_number++;
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
}


