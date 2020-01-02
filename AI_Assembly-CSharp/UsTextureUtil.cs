// Decompiled with JetBrains decompiler
// Type: UsTextureUtil
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class UsTextureUtil
{
  public static int GetBitsPerPixel(TextureFormat format)
  {
    switch (format - 1)
    {
      case 0:
        return 8;
      case 1:
        return 16;
      case 2:
        return 24;
      case 3:
        return 32;
      case 4:
        return 32;
      case 6:
        return 16;
      case 9:
        return 4;
      case 11:
        return 8;
      case 12:
        return 16;
      case 13:
        return 32;
      default:
        switch (format - 30)
        {
          case 0:
            return 2;
          case 1:
            return 2;
          case 2:
            return 4;
          case 3:
            return 4;
          case 4:
            return 4;
          default:
            return format == 47 ? 8 : 0;
        }
    }
  }

  public static int CalculateTextureSizeBytes(Texture tTexture)
  {
    int width = tTexture.get_width();
    int height = tTexture.get_height();
    switch (tTexture)
    {
      case Texture2D _:
        Texture2D texture2D = tTexture as Texture2D;
        int bitsPerPixel1 = UsTextureUtil.GetBitsPerPixel(texture2D.get_format());
        int mipmapCount = texture2D.get_mipmapCount();
        int num1 = 1;
        int num2 = 0;
        for (; num1 <= mipmapCount; ++num1)
        {
          num2 += width * height * bitsPerPixel1 / 8;
          width /= 2;
          height /= 2;
        }
        return num2;
      case Cubemap _:
        int bitsPerPixel2 = UsTextureUtil.GetBitsPerPixel((tTexture as Cubemap).get_format());
        return width * height * 6 * bitsPerPixel2 / 8;
      default:
        return 0;
    }
  }

  public static string FormatSizeString(int memSizeKB)
  {
    return string.Empty + (object) memSizeKB + "k";
  }
}
