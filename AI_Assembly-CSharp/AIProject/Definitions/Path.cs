// Decompiled with JetBrains decompiler
// Type: AIProject.Definitions.Path
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace AIProject.Definitions
{
  public static class Path
  {
    public const string ProductName = "";
    public const string DefaultManifestName = "abdata";
    public const string SceneCommonBundleDirectory = "scene/common/";
    public const string SceneCommonBundleName = "scene/common/00.unity3d";

    public static string SaveDataDirectory { get; } = UserData.Path + "save/";

    public static string CharaFileDirectory { get; } = UserData.Path + "chara/";

    public static string GlobalSaveDataFileName { get; } = "global.ila";

    public static string WorldSaveDataFileName { get; } = "save.ila";

    public static string GlobalSaveDataFile { get; } = Path.SaveDataDirectory + Path.GlobalSaveDataFileName;

    public static string WorldSaveDataFile { get; } = Path.SaveDataDirectory + Path.WorldSaveDataFileName;
  }
}
