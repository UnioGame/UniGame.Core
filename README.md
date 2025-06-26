# Unity Tools

UniTools core package

# LifeTime

LifeTime is a powerful resource management system that allows you to track and clean up resources automatically when they are no longer needed.

## Basic Usage

```csharp
// Create a lifetime
var lifeTime = new LifeTimeDefinition();

// Add cleanup actions
lifeTime.AddCleanUpAction(() => Debug.Log("Cleaning up!"));

// Add disposable objects
lifeTime.AddDispose(someDisposableObject);

// Keep object references from garbage collection
lifeTime.AddRef(someObject);

// Terminate lifetime (triggers all cleanup actions)
lifeTime.Terminate();
```

## Features

- **Automatic Resource Management**: Automatically dispose of objects and execute cleanup actions
- **CancellationToken Support**: Convert to CancellationToken for async operations
- **Child Lifetimes**: Support for hierarchical lifetime management
- **Thread-Safe**: Safe to use across multiple threads

```csharp
// Using with cancellation token
var lifeTime = new LifeTimeDefinition();
var token = lifeTime.Token;

await SomeAsyncOperation(token);

// Using composed lifetimes
var mergedLifeTime = lifeTime1.MergeLifeTime(lifeTime2, lifeTime3);
```

# UrlChecker

UrlChecker provides functionality to test multiple endpoints and find the fastest available one.

## Usage

```csharp
// Test multiple URLs and get the fastest one
string[] urls = { "https://api1.example.com", "https://api2.example.com" };

// Select fastest endpoint
var fastestUrl = await UrlChecker.SelectFastestEndPoint(urls, timeout: 5);
Debug.Log($"Fastest URL: {fastestUrl.url}, Time: {fastestUrl.time}ms");

// Check all endpoints
var results = await UrlChecker.CheckEndPoints(urls, timeout: 5);
Debug.Log($"Best endpoint: {results.bestResult.url}");

// Test single URL
var result = await UrlChecker.TestUrlAsync("https://api.example.com", timeout: 5);
Debug.Log($"URL status: {result.success}, Time: {result.time}ms");
```

## Features

- **Performance Testing**: Measure response times for different endpoints
- **Automatic Selection**: Find the fastest available endpoint
- **Timeout Support**: Configure request timeouts
- **Batch Testing**: Test multiple URLs simultaneously

# WebRequestBuilder

WebRequestBuilder is a comprehensive HTTP client for Unity with support for GET, POST, PATCH requests, authentication, and media downloads.

## Basic Usage

```csharp
var builder = new WebRequestBuilder();

// Simple GET request
var response = await builder.GetAsync("https://api.example.com/data");
if (response.success)
{
    Debug.Log(response.data);
}

// POST request with JSON data
var jsonData = "{\"name\":\"test\"}";
var postResponse = await builder.PostAsync(
    url: "https://api.example.com/users",
    data: jsonData,
    headers: new Dictionary<string, string> { ["Content-Type"] = "application/json" }
);

// PATCH request
var patchResponse = await builder.PatchAsync(
    url: "https://api.example.com/users/123",
    data: jsonData
);
```

## Advanced Features

### Authentication

```csharp
// Set bearer token for authentication
builder.SetToken("your-auth-token");

// Now all requests will include Authorization header
var response = await builder.GetAsync("https://api.example.com/protected");
```

### Parameters and Headers

```csharp
var parameters = new Dictionary<string, string>
{
    ["page"] = "1",
    ["limit"] = "10"
};

var headers = new Dictionary<string, string>
{
    ["Accept"] = "application/json",
    ["User-Agent"] = "MyApp/1.0"
};

var response = await builder.GetAsync(
    url: "https://api.example.com/data",
    parameters: parameters,
    headers: headers,
    timeout: 30
);
```

### Media Downloads

```csharp
// Download texture
var textureResult = await builder.GetTextureAsync("https://example.com/image.jpg");
if (textureResult.success)
{
    myRenderer.material.mainTexture = textureResult.texture;
}

// Download sprite
var spriteResult = await builder.GetSpriteAsync("https://example.com/icon.png");
if (spriteResult.success)
{
    myImage.sprite = spriteResult.sprite;
}
```

### Form Data and File Uploads

```csharp
// POST with form data
var form = new WWWForm();
form.AddField("username", "john");
form.AddField("email", "john@example.com");

var response = await builder.PostAsync("https://api.example.com/register", form);

// POST with binary data
byte[] fileData = File.ReadAllBytes("path/to/file.png");
var uploadResponse = await builder.PostAsync(
    url: "https://api.example.com/upload",
    headers: new Dictionary<string, string> { ["Content-Type"] = "application/octet-stream" },
    data: fileData
);
```

## Configuration

```csharp
var builder = new WebRequestBuilder();

// Enable/disable automatic version parameter
builder.addVersion = true; // Adds "v" parameter with Application.version

// Set default authentication token
builder.SetToken("default-token");

// Build custom requests
var request = builder.BuildGetRequest(
    url: "https://api.example.com",
    parameters: parameters,
    headers: headers,
    timeout: 30
);
```

## Response Handling

```csharp
var response = await builder.GetAsync("https://api.example.com/data");

// Check response status
if (response.success)
{
    Debug.Log($"Success: {response.data}");
}
else
{
    Debug.LogError($"Error: {response.error}");
    Debug.LogError($"HTTP Code: {response.responseCode}");
    
    if (response.networkError)
        Debug.LogError("Network error occurred");
    
    if (response.httpError)
        Debug.LogError("HTTP protocol error");
}
```

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