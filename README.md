# Win32Host
Root implementation for hosting WIN32 elements.

## Usage

Extend Win32HwndHost and implement the following methods:
* InitializeHostedContent - Called to create the hosted elements.
* UninitializeHostedContent - Called to destroy hosted elements.
* ResizeHostedContent - Called when the size of the window changes.

## Samples

* Sdl2Example - Hosts a child window created and rendered using SDL2.
* Win32Listbox - Hosts a WIN32 Listbox control and demonstrates interaction using Windows messages.
 