# UFE Extension
![Screenshot 2022-03-29 180737](https://user-images.githubusercontent.com/61408011/160576464-eb8c0277-1dc4-4e3c-b2f8-376ee6a0fb2c.png)


## Install
1. Place this folder under UFE/Engine/Scripts/Core.

2. Place an empty Game Object in the scene and attach UFEExtension.cs to it.  

3. Open UFE/Engine/Scripts/Core/Manager/UFE.cs and edit as follows

Before
```cs
...
public class UFE : MonoBehaviour, UFEInterface
...
```
After
```cs
...
public partial class UFE : MonoBehaviour, UFEInterface
...
```

## How to use
First, a UFE Extension file must be created. It is like a configuration file for the extension.

It can be created by right-clicking on the Project view and selecting Create > U.F.E. > Extension File.

The created file can be edited from the Extension window, which can be opened by selecting Window > U.F.E. > Extension from the menu at the top of the screen.

Drag and drop the created UFE Extension file into the Extension Info of the UFEExtension.cs file attached in the second step of the installation procedure.

Translated with www.DeepL.com/Translator (free version)