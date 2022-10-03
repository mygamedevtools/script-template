![License](https://img.shields.io/github/license/myunitytools/script-template)
![Release](https://img.shields.io/github/v/release/myunitytools/script-template?sort=semver)
![Last Commit](https://img.shields.io/github/last-commit/myunitytools/script-template)

My Unity Tools - Script Template
===

A simple tool to add and manage more script templates.

What's this about?
---

This tool adds custom Script Templates to Unity that will serve as alternatives for the default \"C#Script\" that I personally always modify before writing my scripts. In essence, it allows customization of the script templates with dynamic data such as signature and namespace settings. Additionally, it adds templates for others types of scripts rather than just Mono Behaviours. You can customize the templates after they have been copied as well.

Installation
---

#### [Installing from a git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html) _(requires [Git](https://git-scm.com/) installed and added to the PATH)_
You can open the Package Manager and then click on the `+` button on the top left corner. From there select `Add package from git URL...`, type `https://github.com/myunitytools/script-template.git` and click `Add`. The package will be imported by the Package Manager.

:link: [Unity Official Documentation](https://docs.unity3d.com/Manual/upm-git.html)

Usage
---

![First Image](https://user-images.githubusercontent.com/9505905/111533338-6dd37580-8745-11eb-968d-37102b4b5e5c.png)

From `Assets/Script Templates Editor` you can open the **Script Templates Editor** Window.

![Custom Script Template Editor](https://user-images.githubusercontent.com/9505905/193695307-3b607ab4-9c31-4dab-b96b-176ac57589c2.png)

Here you can set your Author Name and Email to add to your script's signature header. Also, you can choose to use the **local date format**: _9/13/2022 (en-US)_, for example, or the standard ISO format: _2022-09-13_. You need to click `Save` to store this data into
the [Editor Prefs](https://docs.unity3d.com/ScriptReference/EditorPrefs.html). You also have the option to clear the saved data.

Immediately below that, we have a list of all included Script Templates and you can select them to locate the file.
You also have two buttons: `Copy to Project` and `Copy to Editor`. The first one will copy all script templates to the current project `Assets/ScriptTemplates` path,
limiting the script template usage to that project. The other will copy all script templates to your Editor folder, and you will be able to use the templates
in all projects. However if you don't have this package in a project, it will not replace the `#AUTHOR#` and `#CREATIONDATE#` keywords on the file.
Also, if you update the Unity Editor version, you will lose the script templates and require to copy them again. This is not necessary if you copy them to your project.
Both actions of copying to project and to editor will require an editor restart to take effect.

How it works
---

The custom Script Templates have the keywords `#AUTHOR#` and `#CREATIONDATE#` that will be replaced by the data you entered on the Editor
Window and the present date. The keyword `#SCRIPTNAME#` is automatically replaced by Unity. All other keyword recognition and replacing is
done by the [ScriptKeywordProcessor](Assets/CustomScriptTemplate/Editor/ScriptKeywordProcessor.cs) class.

Customization
---

You can customize your Script Templates to suit your needs, for example adding default methods or even new keywords to process. Keep in
mind that if you ever want a feature that it does not have, I may want to include it on this package.

But, why?
---

I like to have my scripts signed so everyone knows who to ask when they have doubts. Unfortunately with every new version of Unity, 
I have to redo the tedious process of updating my Script Templates manually. I'd very much like to be able to [manage my Script Templates
over the Unity Hub](https://forum.unity.com/threads/feature-request-manage-script-templates.532962/), but looks like no one else
uses it.

---

Don't hesitate to create [issues](https://github.com/myunitytools/script-template/issues) for suggestions and bugs. Have fun!
