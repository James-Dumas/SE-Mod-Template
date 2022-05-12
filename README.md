# SE-Mod-Template
An automatically generated template for Space Engineers mods, designed for use with VS Code.

Credit to [gregretkowski's VSC-SE](https://github.com/gregretkowski/VSC-SE) which this project is based on.

## Dependencies
* Space Engineers (obviously)
* [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/install/windows?tabs=net60)

## VS Code Setup

If you plan on using VS Code, you will need the following extensions to get Intellisense working properly:
* [Microsoft's C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
* [NuGet Package Manager](https://marketplace.visualstudio.com/items?itemName=jmrog.vscode-nuget-package-manager)
## Usage
To setup a new mod, first clone this repository. Then edit the file `install.bat` and fill in at the top of the file **(1)** the mod name (no spaces or special characters), **(2)** the path to your Space Engineers AppData directory, and **(3)** the path to your Space Engineers installation directory in Steam (you only need to change this one if you have a non-standard Steam install path).

After filling those in, run `install.bat`. It should create the proper directory structure, and put a template cs file in `Data\Scripts\(Mod Name)`. It will also remove `.git` so that you can more easily set up a new repository for your mod in the directory.

After this, running `install.bat` will install the mod into the Space Engineers mod directory (without all the extraneous C# files), allowing you to test it or upload it to the workshop. If you run `install.bat` after uploading the mod, it will copy the `mod.sbmi` file (this links the mod with its workshop item) back into the working directory. So, you can just run `install.bat` on the game's title screen and then load a world with the mod in it to quickly and easily test changes to the mod.
