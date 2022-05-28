# BtShowXp

Optionally displays the Pilot's Total XP, XP Corruption, minimum mission difficulty to earn full XP with BEX CE's XP cap, and/or BEX's Level cap percentage of XP award.

When configurating a lance for a contract, the Difficulty text will be Green if the pilot will get full XP from the current contract.  If the pilot is exactly at the XP cap level of the mission, the percentage of the max XP for the mission will be displayed.

Other features:
* Contains a temporary fix for BEX which restores the "percentage of XP awarded" logic.
* Can reset pilot's spent XP when the Spent XP is "corrupted" and does not match the skills purchased.

The rest of the document contains various customization and install options, which most users will not need.

![image](https://user-images.githubusercontent.com/54865934/170802733-f957724f-1dcf-44c6-9af8-6eeca049e158.png)

![image](https://user-images.githubusercontent.com/54865934/170802992-8b96bdd7-6bcb-48e2-84c4-6a414f0900b6.png)

# Upgrading
For users upgrading from versions prior to 1.1, it is recommended to overwrite the mod.json in the BTShowXp install directory.


## Difficulty Note
The actual difficulty of a mission can go up or down from the displayed difficulty.  The mod may show a green difficulty highlight even though the final difficulty will be under the minimum difficulty and only get 10% XP (default XP Cap setting).

# mod.json Settings

Setting | Description
|---|---|
|```ShowPilotXp = true``` | Shows the pilot's XP on the pilot placard.
|```ShowPilotXpCorruption = true``` | Displays red XP text on the pilot placard if the pilot's Total XP does not match the skills and unspent XP.  The number is the computed difference between expected and actual XP.
|```ShowPilotXpMinDifficulty = true``` | If Battletech Extended CE is installed and XP Caps is enabled, the minimum mission difficulty to get full XP will be shown.
|```ShowPilotXpMinDifficultyWorkAround = false``` | If ```ShowPilotXpMinDifficulty``` is enabled, adds .5 difficulty to the computed difficulty to work around BEX's XP Cap level bug.  Will be removed when the BEX issue is resolved.  If ```UseBexXpCapFix``` is enabled, set this to false.  For more info on the original bug, see [XP Cap Workaround](#bex-xp-cap-workaround) below.
|```UseBexXpCapFix = true```| If true, will fix the BEX  XP Cap Bug with a temporary patch.  If Extended_CE.dll's XP cap logic has been changed, then the Cap fix will not be enabled.
|```XpPercentageDisplay = "BasedOnPatchStatus"```| Controls when the XP Cap XP award percentage is displayed.  See the [XpPercentageDisplay](#xppercentagedisplay) section below.
|```DebugOutput = false```|If true, will output additional logging.

## XpPercentageDisplay
For most users, the default value will not need to be changed.

|Setting|Description|
|--|--|
|Always|Always show, regardless of other settings.|
|Off|Never show|
|BasedOnPatchStatus|Will be shown if the BEX visual workaround is disabled and the BEX XP Cap bug fix is enabled.|

When BEX fixes the XP cap bug in the BEX source code, use Always instead.


# Resetting Corrupt XP


## Dislaimers
It is recommend to make a backup of the game saves before using.  This has been tested locally and appears to work correctly, but there could be unexpected consequences.

The Battletech campaign save directory for a Steam install is at Steam\userdata\3847327\637090\remote\C0\SGS1 . 

I am unsure at this point why the Spent XP vs Unspent XP gets out of sync over time with various mods so user beware that there may be some reason for the mismatch that I am unaware of.  

## Usage
If a pilot has corrupt XP (as shown by red text), the pilot can be reset by going to the barracks by clicking the pilot's portrait while holding down control (ctrl+click).

Make sure there pilot is not currently selected or the click will not work.  This is a known issue.
Currently each pilot must be changed individually.

## Steam Backups
The Battletech carrear backups on a Steam install should be under Steam\userdata\3847327\637090\remote\C0\SGS1


# Installation
To install, download the BtShowXp.zip from the releases section and extract to the Battletech Mods folder.

This assumes ModTek has been installed and injected.


# Compatibility
This should be compatible with all mods.
Safe to add to and remove from existing saves.


# BEX XP Cap Workaround

**Update**:  The default settings of this mod includes a temporary fix for BEX's XP Cap bug.  This section describes the bug, but is effectively mitigated.

Please note that the XP Cap bug only affects the "full xp difficulty" by half a skull.  So if the pilot requires at least a 2 skull mission to get full XP, the bug would require a 2.5 skull mission to get the full XP.  The Battletech Extended team has confirmed the support ticket.

To work around the BT_Extended_CE XP Cap bug, the BT_Extended_CE\mod.json can be modified to move the Caps up one level.  
Once the pilots require a 5 skull mission to progress, disable XPCap or the pilots will only gain 10% XP.

This workaround will show a minimum 1.0 difficulty mission on the pilot placard for pilots under 5200 XP, but the pilot will not be limited to 10%.  All levels after 1.0 will be correct.

Set XPDifficultyCaps to:
```
"XPDifficultyCaps" : [ 5200, 5200, 8000, 11600, 15600, 21600, 36000, 55600, 81200, 999999999 ],
```

How to disable XPCaps for the last XP Cap (5 star missions)
```
"XPCap" : false,
```

