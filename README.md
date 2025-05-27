<p align="center">
  <img width="100%" alt="prostir" src="">
  </br>
</p>

## 🔴About
**ProjectStir** is a driving simulation game developed with the Unity engine, inspired by Maximum Tune. This application was developed only as a side project and for research purposes and fun, to be showcased by the BINUS Game Application & Technology. It features two modes: racing and driving simulation. 

In racing mode, players race against three competitors in a one-lap event. For an enhanced experience, the game is optimized for steering wheel use or the Steam Deck. The driving simulation mode takes a game-based approach to assess driving skills. Players navigate an urban city environment, collecting checkpoints within set time limits. Performance is scored based on checkpoint collection and vehicle damage.

The game’s Asian city setting includes narrow, one-way streets, and better performance—more checkpoints with minimal damage—leads to higher scores. The game accommodates multiple input devices, including keyboard, joystick, and steering wheel.

Within this gaming environment, an user data collection process is employed, employing a Leaderboard-based, game-oriented methodology. The feature for gathering performance data is initiated through data submission following a player's successful game completion. The variables collected encompass **Player Nickname**, **Checkpoint**, and **Damage** data. This performance data is seamlessly integrated with Looker Studio, a public-accessible data visualization tool. This data collection methodology is easily accessible for entertainment purposes or, if necessary, for research applications. Access to this data is available via https://lookerstudio.google.com/reporting/b00ab0ae-0caf-4b2b-855f-a3c7b88dc9f7
<br>
## ▶️ Video Gameplay
<img src="https://github.com/KXLVXN7/KXLVXN7/blob/main/gif/pstir.gif" alt="1" style="width:50%;height:auto;">
View Full Gameplay : https://youtu.be/UWn3G_PZ_fA

## 🕹️Download Game
Itch.io : https://binusgat.itch.io/project-stir

<br>

## 📋 Project Stir Info
This project using Unity 2021.3.11f1

| **Role** | **Name** | **Development Time** |
|:-|:-|:-|
| Game Programmer - Main Mechanic | Kelvin | 2 Day |
| Game Programmer - Game Mechanic | David Ang | 1 Day |
| Programmer Level Designer | Steven Putra Adicandra | 2 Day |
| Graphic Level Designer | Duns Scotus Aerotri T | 2 Day |
| Game Programmer & UI Designer | Andhika Suryanto | 2 Day |
| Game Designer | Steven Nugroho | 1 Day |
| 2D & 3D Artist | Adelya Sjafri | 2 Day |
| Project Lead | Fajar | 3 Day |
| Visual Designer | Thomas Galih | 1 Day |
| Game Designer & Sound | Galih Dea | 1 Day |

<br>

##  📜Scripts and Features

- In this game, we collect player performance data and store it in Google Sheets using Unity Networking WWW by submitting a form.
- The leaderboard is sorted A-Z and handled by Looker Studio for display on the screen.
- The Saturation Changer is used to control the environment’s tone and weather effects through color adjustments in the game.

|  Script       | Description                                                  |
| ------------------- | ------------------------------------------------------------ |
| `GameManager.cs` | Manages the game flow such as timers, difficulty levels, networking, etc. |
| `CheckPointManager.cs` | Handles the location and management of checkpoints in the game. |
| `SaturationChanger.cs`  | Controls the saturation levels of the 3D environment via post-processing. |
| `UIHandler.cs`  | Manages various UI elements and organizes them into sequences. |
| `etc`  | |


<br>


## 📂Files description

```
├── Project-Stir                      # In this Folder, containing all the Unity project files, to be opened by a Unity Editor
   ├── ...
   ├── Assets                         #  In this Folder, it contains all our code, assets, scenes, etcwas not automatically created by Unity
      ├── ...
      ├── 3rdParty                   # In this folder, there are several packages that you must add via Unity Package Manager
      ├── Scenes                     # In this folder, there are scenes. You can open these scenes to play the game via Unity
      ├── ....
   ├── ...
      
```
<br>

## 🕹️Game controls

The following controls are bound in-game, for gameplay and testing.

| Key Binding       | Function          |
| ----------------- | ----------------- |
| W,A,S,D           | Standard movement |
| F             | NOS              |
| Space             | Hand Break            |



<br>

## 🔥How to open up the project on Unity Editor
This game was developed using **Unity Editor 2021.3.11f1**, and we recommend that you download this specific version because using different ones, especially older versions, might result in problems

![image](https://github.com/fajarnadril/Project-Stir/assets/36891062/1d44502b-1dfb-424c-97d1-6b1a93616ffc)


You are **required to download several assets from the Unity Asset Store** to properly operate this game. All assets should be placed in the **3rdParty** folder. The assets that need to be downloaded are as follows:

**Download Here:**
- Japanese City Megapack (URP 2020) : https://assetstore.unity.com/packages/3d/environments/japanese-city-modular-pack-v1-4-239043
- Logitech SDK : https://assetstore.unity.com/packages/tools/integration/logitech-gaming-sdk-6630
- RainMaker :  https://assetstore.unity.com/packages/vfx/particles/environment/rain-maker-2d-and-3d-rain-particle-system-for-unity-34938
- RealisticCarControllerV3 : https://assetstore.unity.com/packages/tools/physics/realistic-car-controller-16296



