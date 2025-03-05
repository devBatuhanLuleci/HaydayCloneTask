# Unity Farm Simulation Project

## Project Overview

This project is a **Farm Simulation Test Case** developed using **Unity** and **C#**. The game simulates essential farm mechanics, including crop planting, building placement, and a simple economy system. The project is optimized for **mobile platforms** and follows **Hay Day**-style gameplay.

## APK Link

[Download APK](https://github.com/devBatuhanLuleci/HaydayCloneTask/raw/d6e83a0d72d6fa250afd6abbc297e42bca93ffb8/FarmSimulationTaskApk.zip)


## Screenshots

![FarmSimulatorTask](https://github.com/user-attachments/assets/031af986-88b3-42e0-8601-745b38435eb4)



## Features

- **Crop System**: Grid-based planting, growing, and harvesting system.
- **Building Placement**: Grid-based building placement with visual placement validation.
- **Zoom & Pan Camera**: Touch-based pinch zoom and panning camera system.
- **Shop System**: Buy buildings via a UI-based shop system.
- **Timer System**: Production timers with UnityEvents for callbacks.
- **Level System**: Experience-based leveling system with rewards.
- **Event System**: Custom EventManager for decoupled game events.
- **Tooltip System**: Display information during interactions.

## Technologies Used

- Unity **6.0.0.x**
- C#
- Unity Event System
- Unity ScriptableObjects
- LeanTween (Animation Library)
- DOTween (Animation Library)

## Installation & Setup

1. Clone the repository:
   ```bash
   git clone https://github.com/devBatuhanLuleci/HaydayCloneTask.git
   ```
2. Open the project with Unity **6.0.0.x** or higher.
3. Install dependencies via Unity Package Manager if necessary.
4. Add prefabs under `Resources` folder for dynamic loading.

## How to Play

--Necessary : Turn your Unity Editor to Simulator mode and select any mobile device (Project has been worked around Apple Iphone 12 mini)
1. Tap on the **Shop Button** to open the shop.
2. Drag and place buildings onto the grid.
3. Tap on fields to plant crops including cabbage and corn.
4. Wait for crops to grow using the **Timer System**.
5. Harvest crops and store them in the **Barn or Silo**.
6. Upgrade buildings by gathering necessary materials.
7. Drag an object wait for a few seconds to rearrange it's position.
8. Test the building allocation in tile, highlights will be appeared.
## Architecture Overview

- **EventManager**: Manages custom game events.
- **BuildingSystem**: Handles grid-based building placement.
- **StorageManager**: Manages player inventory.
- **LevelSystem**: Tracks player experience and levels.
- **Timer**: Manages production timers.
- **PanZoom**: Touch-based camera controller.
- **ShopManager**: Manages in-game shop interactions.
- **Tooltip System**: Context-based information display.


---

**License:** MIT License
