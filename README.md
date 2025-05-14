# Unity Tools

UniTools core package




# Game Logger
 
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