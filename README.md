# Betting game read me

### Author: 
Brandon Arevalo

## Theme:
I went for a retro yet futuristic theme for this game

![Image of right side UI](https://github.com/ArevaloBrandon115/Betting_Game/blob/master/Images/Full_Game_UI.png?raw=true)

## UI:

On the right going from top to bottom you will see:
- the amount won in the round
- four buttons to choose the Denomination
- increase or decrease buttons for Denomination

![Image of right side UI](https://github.com/ArevaloBrandon115/Betting_Game/blob/master/Images/Betting_Amount_UI.png?raw=true)

On the left you see the 
- chest there is to open once you press play

![Image of left side UI](https://github.com/ArevaloBrandon115/Betting_Game/blob/master/Images/Chest_UI.png?raw=true)

On the very bottom you will see your 
- current balance 
- play button

![Image of bottom side UI](https://github.com/ArevaloBrandon115/Betting_Game/blob/master/Images/Bottom_Play_Button_UI.png?raw=true)

After the user loses all their balance they will be presented to this UI
- able to restart
- quit application

![Image of left side UI](https://github.com/ArevaloBrandon115/Betting_Game/blob/master/Images/Restart_Quit_UI.png?raw=true)

## Gameplay

![Alt Text](https://github.com/ArevaloBrandon115/Betting_Game/blob/master/Videos/Gameplay_Gif.gif?raw=true)


## Versions 1.00 - 1.07:

## Version history
| Version | Date implement | Description |
| ------ | ------ | ------ |
| 1.00 | 6/23/2021 | - Set up UI and dynamic resizing - worked on the logic behind the game, desided on colors/theme |
| 1.01 | 6/23/2021 | added chest sprites, chest animations such as chest wiggle animation |
| 1.02 | 6/24/2021 | added the UI for the round winnings, added the buttons to change the Denomination |
| 1.03 | 6/24/2021 | added the current balance, added the play button |
| 1.04 | 6/24/2021 | added hover and coin visual animations to the chests, added four buttons to help user choose Denomination faster, addded hover and coin SFX  |
| 1.05 | 6/26/2021 | added explosion animation once the player chooses pooper, added background music and explostion SFX |
| 1.06 | 6/26/2021 | added try again screen once the player runs out of money, added the option to quit or to restart, added try again music |
<!---
Version 1.00
  - Set up UI and dynamic resizing 
  - worked on the logic behind the game
  - desided on colors/theme

Version 1.01
  - added chest sprites
  - chest animations such as chest wiggle animation

Version 1.02
  - added the UI for the round winnings
  - added the buttons to change the Denomination

Version 1.03
  - added the current balance
  - added the play button

Version 1.04
  - added hover and coin visual animations to the chests
  - added four buttons to help user choose Denomination faster
  - addded hover and coin SFX

Version 1.05
  - added explosion animation once the player chooses pooper
  - added background music and explostion SFX

Version 1.06
  - added try again screen once the player runs out of money
  - added the option to quit or to restart
  - added try again music
--->
## Main scripts (Assets/Scripts):

GameHandler
  - controls the game and its logic
AudioController
  - controls all sounds added
Sound
  - helper class for AudioController

## Assets used from Unity Asset store:
- 2D Flat Explosion by Osama Deep
[Unity Asset Store Link](https://assetstore.unity.com/packages/2d/textures-materials/2d-flat-explosion-66932)
- Flat Cartoon Chest and coins by EVILTORN
[Unity Asset Store Link](https://assetstore.unity.com/packages/tools/sprite-management/flat-cartoon-chests-and-coins-187033)
