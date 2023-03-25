<h1 align=center>
Script Template
</h1>

<p align=center>  
  <a href="LICENSE">
    <img src="https://img.shields.io/github/license/mygamedevtools/script-template" />
  </a>
  <a href="https://github.com/mygamedevtools/script-template/actions/workflows/release.yml">
    <img src="https://github.com/mygamedevtools/script-template/actions/workflows/release.yml/badge.svg" />
  </a>
  <a href="https://github.com/mygamedevtools/script-template/commits/">
    <img src="https://img.shields.io/github/last-commit/mygamedevtools/script-template" />
  </a>
</p>

<p align=center>
  <a href="https://openupm.com/packages/com.mygamedevtools.script-template/">
    <img src="https://img.shields.io/npm/v/com.mygamedevtools.script-template?label=openupm&registry_uri=https://package.openupm.com" />
  </a>
  <a href="https://github.com/mygamedevtools/script-template/releases/latest">
    <img src="https://img.shields.io/github/v/release/mygamedevtools/script-template?sort=semver" />
  </a>
  <a href="https://github.com/semantic-release/semantic-release">
    <img src="https://img.shields.io/badge/semantic--release-angular-e10079?logo=semantic-release" />
  </a>
</p>

<p align=center><i>
A simple tool to add and manage more script templates.
</i></p>

What's this about?
---

This tool adds custom Script Templates to Unity that will serve as alternatives for the default \"C#Script\" that I personally always modify before writing my scripts. In essence, it allows customization of the script templates with dynamic data such as signature and namespace settings. Additionally, it adds templates for others types of scripts rather than just Mono Behaviours. You can customize the templates after they have been copied as well.

Installation
---

### OpenUPM

This package is available on the [OpenUPM](https://openupm.com/packages/com.mygamedevtools.script-template) registry. Add the package via the [openupm-cli](https://github.com/openupm/openupm-cli):

```
openupm add com.mygamedevtools.script-template
```

### [Installing from Git](https://docs.unity3d.com/Manual/upm-ui-giturl.html) _(requires [Git](https://git-scm.com/) installed and added to the PATH)_

1. Open `Edit/Project Settings/Package Manager`.
2. Click <kbd>+</kbd>.
3. Select `Add package from git URL...`.
4. Paste `com.mygamedevtools.script-template` into name.
5. Click `Add`.

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
