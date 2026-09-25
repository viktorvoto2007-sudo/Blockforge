using System.IO;
using UnityEditor;

public static class BuildScript
{
    public static void BuildAndroid()
    {
        Directory.CreateDirectory("build");

        var scenes = new[] { "Assets/Scenes/Main.unity" };
        var options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = "build/Blockforge.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildPipeline.BuildPlayer(options);
    }
}
