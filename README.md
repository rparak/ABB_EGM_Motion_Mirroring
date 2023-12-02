# Motion Mirroring of the ABB Robot Arm via EGM

## Requirements:

**Software:**
```bash
ABB RobotStudio
```

**Programming Language**

```bash
Python, C#
```

**Import Libraries**
```bash
More information can be found in the individual scripts (*.py, *.cs).
```

**Supported on the following operating systems:**
```bash
Windows
```

| Software/Package      | Link                                                                                  |
| --------------------- | ------------------------------------------------------------------------------------- |
| ABB RobotStudio       | https://new.abb.com/products/robotics/robotstudio/downloads                           |

## Project Description:

**WARNING: To control the robot in the real world, it is necessary to disable the firewall.**

**Notes:**

EGM (Externally Guided Motion) is an interface for ABB robots that allows smoothless control of the robotic arm from an external application. The EGM can be used to transfer positions to the robot controller in either Joint/ Cartesian space. In our case it is the control of the robot using Cartesian coordinates.

```bash
The file "egm.proto" can be found in the installation folder of RobotWare. For example on Windows with RobotWare 7.6.1:
C:\Users\<user_name>\AppData\Local\ABB Industrial IT\Robotics IT\RobotWare\RobotControl_7.6.1\utility\Template\EGM
```

The Protobuf code generator can be used to generate code from a *.proto file into individual programming languages.

Link: [Protobuf Code Generator and Parser](https://protogen.marcgravell.com)

**Unpacking a station (*.rspag):**
1. On the File tab, click Open and then browse to the folder and select the Pack&Go file, the Unpack & Work wizard opens.
2. In the Welcome to the Unpack & Work Wizard page, click Next.
3. In the Select package page, click Browse and then select the Pack & Go file to unpack and the Target folder. Click Next.
4. In the Library handling page select the target library. Two options are available, Load files from local PC or Load files from Pack & Go. Click the option to select the location for loading the required files, and click Next.
5. In the Virtual Controller page, select the RobotWare version and then click Locations to access the RobotWare Add-in and Media pool folders. Optionally, select the check box to automatically restore backup. Click Next.
6. In the Ready to unpack page, review the information and then click Finish.
   
## Project Hierarchy:

```bash
[/ABB_RS/]
Description:
  The main ABB RobotStudio project for robot control via EGM. 
  
[/PROTO/]
Description:
  The egm.proto -file.
```

## Contact Info
Roman.Parak@outlook.com

## Citation (BibTex)
```bash
@misc{RomanParak_EGM_MotionMirroring,
  author = {Roman Parak},
  title = {Motion mirroring of the abb robot arm via egm},
  year = {2022},
  publisher = {GitHub},
  journal = {GitHub repository},
  howpublished = {\url{https://https://github.com/rparak/Transformation}}
}
```

## License
[MIT](https://choosealicense.com/licenses/mit/)
