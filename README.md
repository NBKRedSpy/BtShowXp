# BtShowXp

Optionally displays the Pilot's Total XP, XP Corruption, and/or minimum mission difficulty to earn full XP with BEX CE's XP cap.

![image](https://user-images.githubusercontent.com/54865934/167323249-daec91ee-8dbc-4da6-aad0-bd22ee932d7a.png)


# mod.json Settings

Setting | Description
---|---|
|```ShowPilotXp = true``` | Shows the pilot's XP on the pilot placard.
|```ShowPilotXpCorruption = true``` | Displays red "XP Mismatch" text on the pilot placard if the pilot's Total XP does not match the skills and unspent XP.  The number is the computed difference between expected and actual XP.
|```ShowPilotXpMinDifficulty = true``` | If Battletech Extended CE is installed and XP Caps is enabled, the minimum mission difficulty to get full XP will be shown.
|```ShowPilotXpMinDifficultyWorkAround = true``` | If ```ShowPilotXpMinDifficulty``` is enabled, adds .5 difficulty to the computed difficulty to work around BEX's XP Cap level bug.  Will be removed when the BEX issue is resolved.  See [XP Cap Workaround](#bex-xp-cap-workaround)] below.

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
Rather than using the ```ShowPilotXpMinDifficultyWorkAround``` option for BEX's XP Difficulty Caps, the BT_Extended_CE\mod.json could be modified to move the Caps up one level.  

Change this:
```
"XPDifficultyCaps" : [ 5200, 8000, 11600, 15600, 21600, 36000, 55600, 81200, 113600, 999999999 ],
```

To this:
```
"XPDifficultyCaps" : [ 5200, 5200, 8000, 11600, 15600, 21600, 36000, 55600, 81200, 999999999 ],
```
Make sure this mod's ```ShowPilotXpMinDifficultyWorkAround``` is set to false if the workaround is used.
