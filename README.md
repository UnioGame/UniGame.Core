# Unity Tools

UniTools core package




# Game Logger

** Installing the https://github.com/Cysharp/ZString can be useful for better performance of the logger. **

if ZString is installed in the project on with the package manager, when add define ENABLE_ZSTRING to the project, the logger will use ZString for better performance.

```
    "versionDefines": [
        {
            "name": "com.cysharp.zstring",
            "expression": "",
            "define": "ENABLE_ZSTRING"
        }
    ],
```

 
```csharp
GameLog.Log("Some Log Message") //pring logs to unity console under Editor, this method is not used in build
    
GameLog.Log("Some Log Message",Color.blue) //print logs with color to unity console under Editor, this method is not used in build

GameLog.LogRuntime("Some Log Message") //print logs to unity console under Editor and in build

GameLog.LogError("Some Error Message") //print error logs to unity console and send to firebase crashlytics if define is enabled

GameLog.LogException(Exception e) //print exception logs to unity console and send to firebase crashlytics if define is enabled
```

## Firebase Crashlytics Support

```csharp
ENABLE_FIREBASE_CRASHLYTICS //define for enabling crashlytics support
```

```csharp
GameLog.LogError("Some Error Message") //send error logs to firebase crashlytics if define is enabled
```