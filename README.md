# SE-Mod-Template
An automatically generated template for Space Engineers mods, designed for use with VS Code.

Credit to [gregretkowski's VSC-SE](https://github.com/gregretkowski/VSC-SE) which this project is based on.

## Dependencies
* Space Engineers (obviously)
* [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/install/windows?tabs=net60) (Only if you plan to include C# scripts in your mod)

## VS Code Setup

If you plan on using VS Code and including C# scripts in your mod, you will need the following extensions installed for Intellisense to work properly:
* [Microsoft's C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
* [NuGet Package Manager](https://marketplace.visualstudio.com/items?itemName=jmrog.vscode-nuget-package-manager)
## Usage
To setup a new mod, first clone this repository. Then edit the file `install.bat` and fill in at the top of the file:

1. The mod name. Don't include spaces or special characters. I'm not sure if that will actually break anything, but, better safe than sorry.
2. The path to your Space Engineers AppData directory. You should just have to fill in your Windows username unless you've moved your AppData directory or something.
3. The path to your Space Engineers installation directory in Steam. You should only need to change this one if you have a non-standard Steam library path. You can find the installation directory easily by right-clicking on the game in your Steam library, and clicking **Manage > Browse Local Files**.
4. Whether or not your mod will include C# scripts.

After filling those in, run `install.bat`. It will create the proper directory structure, and, if you specified that the mod includes scripts, will put a template C# script in `Data\Scripts\(Mod Name)`, along with some extra files required for Intellisense. It will also remove `.git` so that you can more easily set up a new repository for your mod in the directory.

After this first run, running `install.bat` again will install the mod into the Space Engineers mod directory (without all the extraneous C# files), allowing you to test it or upload it to the workshop. If you run `install.bat` after uploading the mod, it will copy the `mod.sbmi` file (this is what links the mod files on your computer with the workshop item) back into the working directory, so you should always make sure to do that after uploading to keep everything in sync.

After the initial setup, you can delete the `setup_files` directory if you like, but if you ran `install.bat` with the option `HAS_SCRIPTS=false`, and wish to change it to `true`, you will need the files in this directory.

## Helper Scripts

The `helper_scripts` directory contains some helper scripts I've written that I use in almost all of my mods. Feel free to use them in your own!
