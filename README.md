# DPI Gonzales

This program mitigates an issue in Windows 10 with multi-display-DPI scenarios: perceived mouse speed differs by an uncomfortable amount traveling between high DPI displays and classic DPI displays.

It does this by automatically changing the pointer speed whenever the mouse travels to another monitor. The pointer speed is depending on the resolution, the DPI and Windows scaling settings of the display the mouse is on.

## Requirements

- Windows 10

## Installation

Currently, there is no installer. To make practical use of this program, you could for example:

  - build the solution
  - copy the contents of `src\DpiGonzales\bin\debug` to `C:\Program Files\DpiGonsales`
  - run `shell:startup` from the Windows Run dialog (<kbd>windows</kbd>+<kbd>R</kbd>)
  - make a shortcut in the Explorer window that just opened and make it point to `C:\Program Files\DpiGonsales\DpiGonsales.exe`
