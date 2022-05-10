# BtShowXp

Optionally displays the Pilot's Total XP, XP Corruption, and/or minimum mission difficulty to earn full XP with BEX CE's XP cap.

Also has the option to reset the Spent XP for a pilot that has corrupt XP.

![image](https://user-images.githubusercontent.com/54865934/167340694-3a7b06e3-da51-44ce-b20d-23f89ca9f49f.png)



# Resetting Corrupt XP
It is recommend to make a backup of the game saves before using.  This has been tested locally and appears to work correctly, but there could be unexpected consequences.  An alterantive is to the the Retrainer mod and reset each pilot.

If a pilot has corrupt XP (as shown by red text), the pilot can be reset by going to the barracks by clicking the pilot's portrait while holding down control (ctrl+click).

Make sure there pilot is not currently selected or the click will not work.  This is a known issue.
Currently each pilot must be changed individually.

## Steam Backups
The Battletech carrear backups on a Steam install should be under Steam\userdata\3847327\637090\remote\C0\SGS1

# mod.json Settings

Setting | Description
---|---|
|```ShowPilotXp = true``` | Shows the pilot's XP on the pilot placard.
|```ShowPilotXpCorruption = true``` | Displays red XP text on the pilot placard if the pilot's Total XP does not match the skills and unspent XP.  The number is the computed difference between expected and actual XP.
|```ShowPilotXpMinDifficulty = true``` | If Battletech Extended CE is installed and XP Caps is enabled, the minimum mission difficulty to get full XP will be shown.
|```ShowPilotXpMinDifficultyWorkAround = true``` | If ```ShowPilotXpMinDifficulty``` is enabled, adds .5 difficulty to the computed difficulty to work around BEX's XP Cap level bug.  Will be removed when the BEX issue is resolved.  See [XP Cap Workaround](#bex-xp-cap-workaround) below.

### The options below are used for debugging

Setting | Description
---|---|
|```ShowSkillSyncError = false``` | If the pilot's XP is corrupt (Total XP doesn't match skill and unspent XP), the pilot info will be written to the the Log.txt in the mod's folder.
|```ShowPilotSummary = false``` | Always writes the pilot information to the Log.txt
|```ExportPilots = false```  |Will export the pilots to the PilotExport.txt file in the mod folder.  The pilots are exported every time the barracks screen is opened.
|```OnlyExportSyncErrorPilots = false``` | If true, will only export pilots that have corrupt XP.


# Installation
To install, download the BtShowXp.zip from https://github.com/NBKRedSpy/BtShowXp/releases/ and extract to the Battletech Mods folder.

This assumes ModTek has been installed and injected.


# Compatibility
This should be compatible with all mods.


# BEX XP Cap Workaround

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

