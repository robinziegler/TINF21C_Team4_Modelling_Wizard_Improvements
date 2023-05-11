# Changelog
|Version|Date|Author|Comment|
|--|--|--|--|
|V0.1|20.09.2022|[Maximilian Trumpp](https://github.com/maximiliantrumpp)|Created first version|
|V0.2|23.09.2022|[Maximilian Trumpp](https://github.com/maximiliantrumpp)|Edited Use Cases|
|V0.3|30.09.2022|[Maximilian Trumpp](https://github.com/maximiliantrumpp)|Edited Features|
|V0.4|06.10.2022|[Maximilian Trumpp](https://github.com/maximiliantrumpp)|Added Prototype|
|V0.5|04.04.2023|[Michael Grote](https://github.com/michi3214)|Edited missing numbers of Features|
|V0.6|14.04.2023|[Maximilian Trumpp](https://github.com/maximiliantrumpp)|Revision|
|V1.0|11.05.2023|[Maximilian Trumpp](https://github.com/maximiliantrumpp)|Final version|

# 1. Introduction
This document is the basis of the Modelling Wizard project of the TINF21C team. The document contains all requirements of the customer for this project. The other documents use this document as a basis. 

# 2. Goal
The goal of this project is to revise the application. The focus should be on revising the user interface and improving the usability of the application. To improve the usability of the app, more parts of the simple mode should be moved to the advanced mode. The menu of the application should be revised. Menu items with similar functions are to be grouped together for a better overview. According to the requirements, the normal user would like to create or import only an existing device and change individual values and not create a complex device. For a better usability it is necessary to refactor the whole code, with the refactoring of the code the existing bugs have to be fixed. In case of an error, customers wished to be better informed and, if possible, to jump directly with the cursor to the field where the error was triggered.

# 3. Product Environment
AutomationML (AML) is the short form of Automation MarkUp Language and is used to describe parts of automation plants as objects. These objects can consist of multiple other objects and can be part of a larger assembly of objects. That way AML can be used to describe a single screw or an entire robot with the necessary level of detail. AML makes use of various standards to describe the following plant components:
1.	CAEX (Computer Aided Engineering Exchange) to describe attributes of objects and their relations in a hierarchical structure. This is called a system topology. In this respect, CAEX forms the overarching integration framework of AutomationML.
2.	COLLADA to describe the geometry and 3D Models of an object
3.	COLLADA also integrates motion planning. It describes the connections and relations of moveable objects, which is called Kinematics.
4.	PLCopen XML describes the logic. Internal behavior and states if objects, action-sequences and I/O connections are implemented via this format. An IODD (IO Device Description) file describes the sensor and actuator of a plant or component. It also contains information on identify, parameters, process data, communication and more. It is written in XML-format, same as AML, which ensures a conversion.

# 4. Product Usage
## .1 Business Process
All Business Processes are included in the old [wiki](https://github.com/H4CK3R-01/TINF20C_ModellingWizard_Devices/blob/04980943011ff843bdf6ded0b3f02ac33294d35e/PROJECT/CRS/TINF20C_CRS_ModellingDevices_Team_1_0v1-2.pdf) of the application. Due to the requirement to fix the bugs and improve usability, no new business processes have emerged.

## 4.2 Use Cases
The project should not receive any new use cases, as this project is only about usability improvements.
### 4.2.1 <UC.001> Create new device
|Use Cases Objective|User wants to create a device by inserting the data manually into the user interface of the application|
|--|--|
|System Boundary|The application itself|
|Precondition|The user needs to know the minimal required parameters for the device. The program needs to be opened on the user's system.|
|Postcondition on success|The entered data is displayed completely and correctly. The user needs to easily find where to edit which parameter.|
|Contributed user|Every end-user of the application.|
|Triggering event|When the user opens the application and uses the 'new device' button.|

![UC 001](https://github.com/robinziegler/TINF21C_Team4_Modelling_Wizard_Improvements/assets/96117277/105721f0-3f07-4ee5-8b9f-d48a4f7796a0)


### 4.2.2 <UC.002> Save device
|Use Cases Objective|The user wants to save a currently opened device, by using the "Save" button. |
|--|--|
|System Boundary|The application itself.|
|Precondition|The user created a new device or has loaded an existing device from a file.|
|Postcondition on success|The data from the program is saved correctly.|
|Contributed user|Every end-user of the application.|
|Triggering event|When the user clicks the "Save" button.|

![UC 002](https://github.com/robinziegler/TINF21C_Team4_Modelling_Wizard_Improvements/assets/96117277/a41a8076-4b03-46fd-9afa-ecd6f75192bc)


### 4.2.3 <UC.003> Add interface from existing library
|Use Cases Objective|User wants to add an existing interface from the loaded library.|
|--|--|
|System Boundary|The application itself.|
|Precondition|The user needs to know the minimal required data for the interface to be added.|
|Postcondition on success|The user has successfully added an interface from the loaded library to the loaded device.|
|Contributed user|Every end-user of the application.|
|Triggering event|When the user clicks on the "Add Interface" button.|

![UC 003](https://github.com/robinziegler/TINF21C_Team4_Modelling_Wizard_Improvements/assets/96117277/bf72b6c2-c84f-4c24-8d24-45f6f7384beb)


### 4.2.4 <UC.004> View and edit device data 
|Use Cases Objective|When a device is either opened or created, the user should see all device data. The user should also be able to edit them.|
|--|--|
|System Boundary|The application itself.|
|Precondition|Valid device informations were loaded.|
|Postcondition on success|The entered data is displayed completely and correctly. The user has one device opened.|
|Contributed user|Every end-user of the application.|
|Triggering event|A device was successfully opened or created from an existing file.|

![UC 004](https://github.com/robinziegler/TINF21C_Team4_Modelling_Wizard_Improvements/assets/96117277/e384c07e-e570-4e45-bcb2-947853a6e523)


### 4.2.5 <UC.005> Add attachments to device
|Use Cases Objective|The user wants to add an attachment to a device.|
|--|--|
|System Boundary|The application itself.|
|Precondition|The user has either loaded an existing device or created a new one.|
|Postcondition on success|The user has successfully added a system unit class to the loaded device.|
|Contributed user|Every end-user of the application.|
|Triggering event|When the user uses the "Add Attachment" button.|

![UC 005](https://github.com/robinziegler/TINF21C_Team4_Modelling_Wizard_Improvements/assets/96117277/46325630-00ad-4154-a061-98658cae392e)


### 4.2.6 <UC.006> Add system units classes to device
|Use Cases Objective|User wants to add an existing system unit class from the loaded library.|
|--|--|
|System Boundary|The application itself.|
|Precondition|The user needs to know the minimal required data for the system unit class to be added.|
|Postcondition on success|The user has successfully added a system unit class from the loaded library to the loaded device.|
|Contributed user|Every end-user of the application.|
|Triggering event|When the user clicks on the "Add System Unit Class" button.|

![UC 006](https://github.com/robinziegler/TINF21C_Team4_Modelling_Wizard_Improvements/assets/96117277/5b8d4a54-b36f-42d9-bf44-31c2fafac3c9)

### 4.2.7 <UC.007> Add role class to device
|Use Cases Objective|User wants to add an existing role class from the loaded library.|
|--|--|
|System Boundary|The application itself.|
|Precondition|The user needs to know the minimal required data for the role class to be added.|
|Postcondition on success|The user has successfully added a role class from the loaded library to the loaded device.|
|Contributed user|Every end-user of the application.|
|Triggering event|When the user clicks on the "Add Role Class" button.|

![UC 007](https://github.com/robinziegler/TINF21C_Team4_Modelling_Wizard_Improvements/assets/96117277/fe6da397-7db3-4a98-b0a2-5ce336b0acb2)

### 4.2.8 <UC.008> Load individual library
|Use Cases Objective|User wants to use an individual library file.|
|--|--|
|System Boundary|The application itself.|
|Precondition|The user needs a valid library file.|
|Postcondition on success|The individual library can be used to add classes.|
|Contributed user|Every end-user of the application.|
|Triggering event|When the user clicks on the "Add library" button in the menu.|

![UC 008](https://github.com/robinziegler/TINF21C_Team4_Modelling_Wizard_Improvements/assets/96117277/d469c1de-b309-4d4d-a498-84cf1b2302e9)

## 4.3 Functional Requirements
In this chapter, the definable features are described and illustrated with figures.

### 4.3.1 /LF11/Import
The application should be able to import a file by the absolute path to the file. Supported filetypes should be AMLX, AML, EDZ, IODD and GSD.

### 4.3.2 /LF12/File Validation
The system shall be able to detect wrongly formatted imported files and throw an error to the user.

### 4.3.3 /LF13/Error Handling
The system shall be able to handle errors (unexpected shut down, wrongly formatted files, ...) and throw an error to the user.

### 4.3.4 /LF14/Edit device
The system should display of a device after either load or open a device. The user should be able to edit displayed informations.

### 4.3.5 /LF15/Create device
When the user creates a new device, it should not have any values except for the System Unit class.

### 4.3.6 /LF16/ Export device
After the end of the editor the user should have the possibility to save the device in a file.

## 4.4 Non-functional Requirements
### 4.4.1 /NF11/GUI improvements
The application should present an appealing interface. The graphical user interface should have a very good usability for an easy operation.The user should be able to navigate easily and intuitively in the application.

### 4.4.2 /NF12/Display device in a readable way
The informations from the either opened or created device should be displayed in a readable way for the user. In expert mode the user can view more details of the device.

### 4.4.3 /NF13/Easy Mode
The user wants to be able to view and edit basic information of the attributes.

### 4.4.4 /NF14/Expert Mode
The user wants to be able to view and edit advanced information of the attributes.

### 4.4.5 /NF15/Portable
If possible, the application should be able to run as a portable, without an installation. 

### 4.4.6 /NF16/Performance
The application should respond instantly after a user's action. There should not be long loading times.

### 4.4.7 /NF17/Compatibility
The application should be executable on every current system such as Windows 10 or higher. Furthermore the application is only executable on the Windows platform.

# 5. Prototypes of the UI 
The first prototype includes the changes in the existing UI. The final prototype is based on a completely redesigned interface.
## 5.1 First Prototype
![AdvancedMode](https://user-images.githubusercontent.com/96117277/198146532-2bb52457-4921-46b4-a2c4-445847c0cce6.jpg)

## 5.2 Final Prototype
![](https://user-images.githubusercontent.com/96415701/237424296-6eb49083-359f-474c-8be9-26c54f8424a3.png)

# 6. References
[1][GitHub Repository from TINF20C](https://github.com/H4CK3R-01/TINF20C_ModellingWizard_Devices)



