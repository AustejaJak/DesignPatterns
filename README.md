# Design Patterns

Project for university module **T120B516 Objektinis programų projektavimas.**

## installation
1. Install **.NET 5.0.100 SDK** from https://dotnet.microsoft.com/en-us/download/dotnet/5.0
2. Install **SplashKit** from https://splashkit.io/installation/    *(Recommendation to install the MSYS2 way)*
3. Open the project with VS code and install plugins related to C#.
4. Launch the project.

**ISSUE** *System.DllNotFoundException unable to load DLL splashkit.dll* 

(Documentation: https://splashkit.io/troubleshoot/windows/list/win-issue-2/)
> **Solution 1:**

> Disable any antivirus software on your computer.

> **Solution 2:**

> Make sure your project isn’t called “SplashKit”.

> **Solution 3:**

> Make sure you are creating your project using the MINGW64 terminal (rather than the MSYS terminal) and create the project files in its own directory/folder.

> **Solution 4:** Add the folder containing splashkit.dll file to your path environment variable manually. Firstly, go through Steps 1 – 3 shown in the “Update your system “Path” variable” section here.

> Then come back here for the Next Step.

> Next Step: In the “Edit environment variable” window, you should have these two paths shown in the image below (Green Box) – on the next page-If you are missing one of the paths in the Green box, click “New” (Red box), then add the path you are missing. It will be similar to what is shown in the Green box - just using your own username.

> Once it is added, click “OK” on all the windows, open a new MINGW64 terminal and try running the program again.

> **Solution 5 (if all else fails):** Get it working by copying the SplashKit binaries to the build output: The SplashKit binaries are in

> C:\msys64\home\(your username)\.splashkit\lib\win64 Replace “(your username)” with your username. (See Note about “whoami” above to get your username.)

> The contents of the folder should look something like this:-Copy all of these into the build output of your SplashKit project. This will usually be:

> <Project folder name>\bin\Debug\net7.0

> eg, if the folder name is MyProject, and you are using .NET 7.0, it would be:

> MyProject\bin\Debug\net7.0
