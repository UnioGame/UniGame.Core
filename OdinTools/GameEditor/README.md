# Game Editor

# Categories

## Auto Categories

If you want to automatically create editor categories based on your custom classes, you can use the `IAutoEditorCategory` interface.

As an Example, if you use Asset as a category view, you can inherit from `AutoAssetCategoryView<TAsset>` where `TAsset` is the type of the asset you want to show.

```csharp
    [Serializable]
    public class AutoAssetCategoryView<TAsset> : AssetViewCategory<TAsset>, IAutoEditorCategory
        where TAsset : Object
    {
    }
```