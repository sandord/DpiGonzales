# DPI Gonzales

This program mitigates an issue in Windows 10 with multi-display-DPI scenarios: perceived mouse speed differs by an uncomfortable amount traveling between high DPI displays and classic DPI displays.

It does this by automatically changing the pointer speed whenever the mouse travels to another monitor. The pointer speed is depending on the resolution, the DPI and Windows scaling settings of the display the mouse is on.

## Requirements

- Windows 10
- .NET Framework 4.6+
- Two or more displays with different DPIs

## Installation

Currently, there is no installer. To make practical use of this program, you could for example:

  - download the latest release from https://github.com/sandord/DpiGonzales/releases by clicking on the `DpiGonzales-portable.zip` file
  - extract the contents of the downloaded .zip file to `C:\Program Files\DpiGonzales` or any other place on your hard drive you might prefer.
  - run `shell:startup` from the Windows Run dialog (<kbd>windows</kbd>+<kbd>R</kbd>)
  - create a shortcut in the Explorer window that just opened and make it point to the `DpiGonzales.exe` file in the folder you extracted the downloaded .zip file to.

You can now either reboot your machine or manually start the `DpiGonzales.exe` file/shortcut.

The next time your machine boots, it should start DPI Gonzales automatically. Please keep in mind that Windows might wait a few seconds before starting DPI Gonzales.

You can tell DPI Gonzales is running if you see the DPI Gonzales mouse icon in your tray bar.
