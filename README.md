# Project Title: [Insert Project Name Here]

## Overview
This project focuses on estimating the impact of the size-weight illusion on performance, motivation and enjoyment in the context of a VR strength-training exergame. The bulk of the project is all handled internally in Unity and all assets needed to run the game successfully are included in this repository. Any additional packages required to run the game will be present below.

## Features
- Size-Weight Illusion Mechanism: Implements visual scaling of virtual weights to manipulate the user’s perception of heaviness without altering the physical force required.
- Dynamic Exercise System: Alternates between bicep curls (isotonic) and isometric holds, simulating the physical exertion of pushing and resisting a rock up a hill.
- Third-Person VR Perspective: The player controls an avatar in a third-person view, enhancing immersion by visually tying physical exertion to the game’s progress.
- Isometric Hold Challenge: Introduces a risk-reward dynamic where players must maintain a static hold to prevent progress loss, simulating a real-life isometric exercise.
- Progress Tracking: A simple progress bar and rock icon display the user’s advancement as they complete repetitions, motivating continued performance.
- Calibration-Based Exercise Tracking: Tracks user movements through initial calibration, ensuring accurate tracking of bicep curls and isometric holds.
- Minimalist Visual Design: Features a clean, distraction-free environment with simple background elements (e.g., mountains, skybox) to maintain user focus on core game mechanics.

## Installation
### Prerequisites
- [List of dependencies]
- Unity Editor Version 2022.3.22f1
- Meta XR All-in-One SDK (Package - Third Party)
- XR Interaction Toolkit (Package - Unity Registry)
- Meta Quest Link (Program)
- [Hardware Requirements]
- USB 3.0 to USB C Cord (for quest link)
- Alternatively you can configure wireless quest link
- Meta Quest Pro Headset (Other models may be compatible but cannot confirm)

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/Chafuyee/Funbells-Project-Group-1
   cd Funbells-Project-Group-1
2. Open the Project in Unity

    - Install Unity (if you haven't already): Make sure you have Unity Hub installed, and download the Unity version compatible with the project. This project was developed using Unity 2022.3.22f1
    - Open Unity Hub: Launch Unity Hub, and click on the "Open" button.
    - Select the Project: Navigate to the folder where the project was cloned and select it.
    - Wait for Unity to Load: The project will take a moment to load as Unity compiles assets and scripts.

3. Before running the project, you may need to ensure that all dependencies are correctly set up:

    - Install XR Plugins: If not already installed, ensure the Meta XR SDK (for Meta Quest Pro) is included in the project. You can do this by going to Window > Package - Manager and searching for Meta XR Plugin.
    - Setup VR Hardware: Connect your Meta Quest Pro headset and ensure that it’s configured for use with Unity. Verify that the tracking systems and motion controllers are properly paired with the device.

4. Preparing the Game Conditions:

    - Input conditions: Within the GameStateManager.cs script there is a function called 'generateExperimentList()' which stores a multidimensional array of csv data. In this array each list corresponds to a participant and each entry in said list represents a condition (weight size + visual representation). These correlate directly with the name of the dumbell visuals in the game scene hierarchy. The dumbbell visuals are attached to the XR wrist UI in the following hierarchy path ('HeadsetConfig/XR Interacion Hands Setup/XR Origin (XR Rig)/Dumbell Visualiser Tracking/Dumbell Visualiser/Follow GameObject/Wrist Button UI/{HERE ARE THE OBJECTS}). The 'Dumbell' object holds all components for game interaction, to change the visual dumbell it is simply make invisible whilst the other objects are shown. Whilst invisible it handles all interaction with the calibrated collision objects. 
    - Handling Conditions: It is very important that the strings in the multidimensional input array correspond to an object name under the aforementioned parent object. This is how the game knows which visual representation to change to for specific steps. In our case the conditions where shuffled using a latin square. To add new visual representations you simply have to add them as GameObjects to the hierarchy location and ensure they have the same transform coordinates as the original 'Dumbell' object.


5. Running the Project

    - Switch to Play Mode: In the Unity Editor, click on the "Play" button in the top toolbar.
    - Switch Through Game States: The game is organized based on a sequential game state, someone outside of the headset needs to control the progression of the game. There are four buttons on the laptop game scene view, these cannot be scene by the VR user. Progress State and Reverse State are used to cycle through the overarching states of the game, their order is as follows: NONE -> Curl Calibration -> Hold Calibration -> Player Start Screen -> Exercise Loop. When you get into the exercise loop state, you will no longer use the progress state button and will instead use the 'Next Exercise' button. This button will go through the exercise based on the input from the csv data and follow the format as shown: Curl Set X -> Pause -> Hold Set X -> Pause -> Curl Set X+1...
    - Maintain Game Integrity: The headset tracking is relatively fickle so be sure that calibration shadows are set at a reasonable distance to prevent infinite collisions. To handle any in-game movement bugs there is a 'Reset Movement' button that can be used to stop the player from moving forward if the collision breaks. 

6. Changing the Project

    - Alterations to the project's general structure will require a significant level of new code, particularly in the GameStateManager.cs. This is because the exercise loop is static, if exercise types are to be added to removed, or order to be changed, a decent amount of work would be required.

   