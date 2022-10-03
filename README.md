![License](https://img.shields.io/github/license/mygamedevtools/script-template)
![Release](https://img.shields.io/github/v/release/mygamedevtools/script-template?sort=semver)
![Last Commit](https://img.shields.io/github/last-commit/mygamedevtools/script-template)

My Unity Tools - Script Template
===

A simple tool to add and manage more script templates.

What's this about?
---

This tool adds custom Script Templates to Unity that will serve as alternatives for the default \"C#Script\" that I personally always modify before writing my scripts. In essence, it allows customization of the script templates with dynamic data such as signature and namespace settings. Additionally, it adds templates for others types of scripts rather than just Mono Behaviours. You can customize the templates after they have been copied as well.

Installation
---

#### [Installing from a git URL](https://docs.unity3d.com/Manual/upm-ui-giturl.html) _(requires [Git](https://git-scm.com/) installed and added to the PATH)_
You can open the Package Manager and then click on the `+` button on the top left corner. From there select `Add package from git URL...`, type `https://github.com/mygamedevtools/script-template.git` and click `Add`. The package will be imported by the Package Manager.

:link: [Unity Official Documentation](https://docs.unity3d.com/Manual/upm-git.html)

Usage
---

![First Image](https://user-images.githubusercontent.com/9505905/111533338-6dd37580-8745-11eb-968d-37102b4b5e5c.png)

From `Assets/Script Templates Editor` you can open the **Script Templates Editor** Window.

![Custom Script Template Editor](https://user-images.githubusercontent.com/9505905/193695307-3b607ab4-9c31-4dab-b96b-176ac57589c2.png)

Here you can preview in realtime what your script will look like, customize the values and copy the templates. 

### Settings

You can decide to enable both the Signature and the Namespace modules, each with their own set of custom settings.

#### Signature

The Signature module adds a simple signature header on your script following the template:

```cs
/**
 * #SCRIPTNAME#.cs
 * Created by: #AUTHOR#
 * Created on: #CREATIONDATE#
 */
```

The `#SCRIPTNAME#` is no stranger to custom script templates, as it is replaced by the default Unity asset processor.
However, both the `#AUTHOR#` and `#CREATIONDATE#` will be replaced by the values you set on the `Author`, `Email(optional)` and `Use Local Date` fields under the Namespace module group.

#### Namespace

The Namespace module adds a namespace to your file, indenting the template content under it.
For that to work, you need to have a template such as:

```
#NAMESPACE#public class #SCRIPTNAME# : MonoBehaviour
{
    public void Start()
    {

    }
}
```

Then, if enabled, the namespace will be replaced according to the values you set.
If you enable `Use Assembly Definition`, make sure the script is in the correct location to be linked to an assembly definition, or the `Default Namespace` will be applied.
Otherwise, if you decide to just use the `Default Namespace`, then it will be applied to all scripts.
Additionally, you can choose whether the auto indent use spaces or tabs and their amounts.

Finally, you should `Save` your changes once you're satisfied with your settings, to make sure they will be used whenever creating a new script.
You also have the option to `Clear` those settings at any time, if you wish.
This data is saved to the [Editor Prefs](https://docs.unity3d.com/ScriptReference/EditorPrefs.html) cache.

### Templates

This section shows a list of all included Script Templates and you can select them to locate the file.
You also have two buttons: `Copy to Project` and `Copy to Editor`.
The first one will copy all script templates to the current project `Assets/ScriptTemplates` path, limiting the script template usage to that project.
The other will copy all script templates to your Editor folder, and you will be able to use the templates
in all projects.
However if you don't have this package in a project, it will not replace the custom keywords on the file and might lead to compilation issues.
Also, if you update the Unity Editor version, you will lose the script templates and require to copy them again. This is not necessary if you copy them to your project.
Both actions of copying to project and to editor will require an editor restart to take effect.

How it works
---

The custom Script Templates have custom keywords that will be replaced by the data you entered on the Editor Window and the present date.
The keyword recognition and replacing is done by the [ScriptKeywordReplacer](Assets/CustomScriptTemplate/Editor/ScriptKeywordReplacer.cs) class.

Customization
---

You can customize your Script Templates to suit your needs, for example adding default methods or even new keywords to process.
Just make sure you add them to the correct folder and add the custom keywords following the same rules as the built-in templates.
Keep in mind that if you ever want a feature that it does not have, I may want to include it on this package.

But, why?
---

It all started when I wanted to have my scripts signed so everyone knows who to ask when they have doubts.
Unfortunately with every new version of Unity, I had to redo the tedious process of updating my Script Templates manually.
I'd very much like to be able to [manage my Script Templates over the Unity Hub](https://forum.unity.com/threads/feature-request-manage-script-templates.532962/), but it's a very minor feature.

---

Don't hesitate to create [issues](https://github.com/mygamedevtools/script-template/issues) for suggestions and bugs. Have fun!
