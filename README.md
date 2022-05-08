# BtShowXp

Displays the XP on the Pilot's display.  Useful for determining if a pilot will be over the Battletech Extended XP caps.

If the pilot's XP is corrupt, XP Mismatch text with the computed difference will be shown.

![image](https://user-images.githubusercontent.com/54865934/167276422-3e81da6f-3b16-43e1-a88f-74080dec0c9f.png)


# Settings

## Note - these options effectively duplicate the UI display, but can be used to export the pilots in a JSON format.

```ShowSkillSyncError = false``` If true, if the pilot's XP is corrupt (Total XP doesn't match skill and unspent XP), the pilot info will be written to the the Log.txt in the mod's folder.

```ShowPilotSummary = false``` If true, always writes the pilot information to the Log.txt

```ExportPilots = false``` If true, will export the pilots to the PilotExport.txt file in the mod folder.  The pilots are exported every time the barracks screen is opened.

```OnlyExportSyncErrorPilots = false``` If true, will only export pilots that have corrupt XP.

# Installation
To install, download the BtShowXp.zip from https://github.com/NBKRedSpy/BtShowXp/releases/ and extract to the Battletech Mods folder.

This assumes ModTek has been installed and injected.


# Compatibility
This should be compatible with all mods.
