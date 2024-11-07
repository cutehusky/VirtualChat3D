# VirtualChat3D
## Project Structure 
```
README.md
|- src: used to store source code (Unity project)
	|- Assets
		|- Plugins: Additions third party plugins
		|- Scenes: Folder store scenes
		|- Scripts: Folder save C# source code
		|- Settings: Unity URP settings
		|- Models: Folder store 3D character models
		|- Animations: Folder store 3D character animations
		|- Prefabs: Folder store prefabs
	|- Package: Define Module install by Unity Package Manager
	|- ProjectSettings : Settings of Unity project
|- docs: used to store documentations, which has the following folders
	|- management: storing planning documents, reports (weekly report, project status report, etc.)
	|- requirements: storing all requirements, including vision document and use cases
	|- analysis and design: storing all analysis and design related documents, including software architecture document, UML models, UI design
	|- test: storing all test documents such as test plan, test cases, test reports
|- pa: including subfolders to store submissions. Each subfolder contains one PA submission.
```
## Requirement 
- Unity 2023.2.5f1 with Module: 
	- Android Build Support
	- iOS Build Support 
	- Windows Build Support IL2CPP
- JetBrains Rider (optional)
## Dependencies 
- Vroid SDK 0.5.2 (installed) (https://developer.vroid.com/en/sdk/)
- Firebase unity SDK 12.3.0 (**manually install**) module: (https://github.com/firebase/firebase-unity-sdk)
	- Firebase database
	- Firebase Auth
	- Firebase Storage
- ExecutionOrder (installed) (https://github.com/azixMcAze/Unity-ExecutionOrder)
	- Dependencies: com.dbrizov.naughtyattributes, com.jimmycushnie.jimmysunityutilities (will automatically install by Unity Package Manager)
- FancyTextRendering (installed) (https://github.com/JimmyCushnie/FancyTextRendering)
- TextMesh Pro (Unity Build-in)
- UGemini (https://github.com/Uralstech/UGemini) (will automatically install by Unity Package Manager):
	- Dependencies: com.uralstech.utils.singleton, com.uralstech.ucloud.operations (will automatically install by Unity Package Manager)
- Native File Picker (https://github.com/yasirkula/UnityNativeFilePicker) (will automatically install by Unity Package Manager)
- Native Gallery (https://github.com/yasirkula/UnityNativeGallery) (will automatically install by Unity Package Manager)
- QFramework (https://github.com/liangxiegame/QFramework) (installed)
- Native websocket (https://github.com/endel/NativeWebSocket) (will automatically install by Unity Package Manager)
