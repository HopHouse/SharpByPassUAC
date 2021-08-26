# SharpByPassUAC

Tool to leverage a famous UAC bypass and execute the code located inside a DLL.

The tool leverages DLL hijacking vulnerabilities of Microsoft executables located into `C:\Windows\System32\`. It leverages the fact that it is possible to create a folder containing a space like `C:\Windows\System32 \` (note de space after `System32`). When Microsoft executable look for missing DLL, they will it our created folder before `System32` and then having our custom DLL loaded. 

If it is weaponized, the code inside will be executed. 

Custom DLL can be created will [SharpDLLProxy](https://github.com/HopHouse/SharpDLLProxy) in order to be directly used.

## Example
The executable `msdt.exe` is prone to a DLL Hijacking vulnerability with the DLL `cabinet.dll`

```
C:\> SharpByPassUAC.exe --binary C:\\Windows\\System32\\msdt.exe --dll  C:\\Users\\administrator\\Desktop\\cabinet.dll
```

## Help
```
SharpByPassUAC 1.0.0
Copyright (C) 2021 SharpByPassUAC

  --dll        Required. Input dll file to be processed.

  --binary     Required. Binary file to execute in order to trigger the UAC bypass.

  --help       Display this help screen.

  --version    Display version information.
```