# Unity Custom Script Template
A simple tool to create your own script template.

:warning: *If you want to extend or customize this tool's functionalities, you might as well clone the [project repository](https://github.com/JoaoBorks/unity-customscripttemplate-project) to use as the Unity Project.*

:warning: *If you feel like any information is missing, [check the issues tab](https://github.com/JoaoBorks/unity-customscripttemplate/issues) for problems you might be having or [create a new one](https://github.com/JoaoBorks/unity-customscripttemplate/issues/new) if you couldn't find anything. Feel free to give suggestions and feedback.*

## What's this about?
This tool adds another Script Template to Unity that will serve as an alternative for the default "C# Script" that I personally always
modify before writing my scripts. In essence, it adds a signature header on every script with your name, email and creation date. Also,
it removes the `Start()` and `Update()` methods from the default template.

## Add to your project

### As an asset

Download the repository and copy the `CustomScriptTemplate` folder to your project. It will add a menu item in 
`Assets/Custom Script Template`. 

### As a package

You should add this to your `manifest.json` under the `Packages` folder on the root of your Unity Project:
```
{
  "dependencies": {
	"com.joaoborks.customscripttemplate": "https://github.com/JoaoBorks/unity-customscripttemplate"
  }
}
```

It will be installed on the next Unity Editor start. Later you will be able to update the tool from the Package Manager Window.

:link: [Unity Official Documentation](https://docs.unity3d.com/Manual/upm-git.html)

## Usage

![First image](https://user-images.githubusercontent.com/9505905/65558067-cab4ed80-df0b-11e9-9b96-7185618b4bc9.png)

From there you can open the **Custom Script Template Editor** Window.

![Custom Script Template Editor](https://user-images.githubusercontent.com/9505905/65558097-dbfdfa00-df0b-11e9-9a5c-fcd3355cbf30.png)

Here you can set your Author Name and Email to add to your script's signature header. You need to click save to store this data into
the [Editor Prefs](https://docs.unity3d.com/ScriptReference/EditorPrefs.html). You also have the option to clear the saved data.

Immediately below that the `Custom Template` field is assigned automatically to the loaded script template, and serves as a shortcut for
you to locate the file. You also have the options to `Edit` this file (that should open the file on your default 
[External Script Editor](https://docs.unity3d.com/Manual/Preferences.html#External-Tools)) and to `Generate` the script template, that
copies your custom template to the Editor's Script Tempalte folder and reloads the Editor Application.

## How it works

The custom Script Template has the keywords `#AUTHOR#` and `#CREATIONDATE#` that will be replaced by the data you entered on the Editor
Window and the present date. The keyword `#SCRIPTNAME#` is automatically replaced by Unity. All the keyword recognition and replacing is
done by the [ScriptKeywordProcessor](Assets/CustomScriptTemplate/Editor/ScriptKeywordProcessor.cs) class.

## Customization

You can customize your Script Template to suit your needs, for example adding default methods or even new keywords to process. Keep in
mind that if you ever want a feature that it does not have, I may want to include it on this package.

## But, why?

I like to have my scripts signed so everyone knows who to ask when they have doubts. Unfortunately with every new version of Unity, 
I have to redo the tedious process of updating my Script Template manually. I'd very much like to be able to [manage my Script Templates
over the Unity Hub](https://forum.unity.com/threads/feature-request-manage-script-templates.532962/), but looks like no one else
uses it.

Have a great time!
