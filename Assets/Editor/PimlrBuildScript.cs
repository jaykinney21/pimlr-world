// PIMLR build tooling — invoke headless via:
//   Unity -batchmode -nographics -projectPath <proj> -buildTarget WebGL -executeMethod PimlrBuildScript.BuildWebGL -logFile <log>
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
using System.Linq;

public static class PimlrBuildScript
{
    public static void BuildWebGL()
    {
        try
        {
            string[] scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled)
                .Select(s => s.path)
                .ToArray();

            if (scenes.Length == 0)
            {
                Debug.LogError("PIMLR BUILD: no enabled scenes in EditorBuildSettings.");
                EditorApplication.Exit(2);
                return;
            }

            string outDir = "Build/WebGL";
            System.IO.Directory.CreateDirectory(outDir);

            Debug.Log("PIMLR BUILD: building " + scenes.Length + " enabled scene(s) to " + outDir);
            foreach (var s in scenes) Debug.Log("  scene: " + s);

            var opts = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = outDir,
                target = BuildTarget.WebGL,
                targetGroup = BuildTargetGroup.WebGL,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(opts);
            var summary = report.summary;
            Debug.Log("PIMLR BUILD RESULT: " + summary.result
                + " | totalSize=" + summary.totalSize
                + " | errors=" + summary.totalErrors
                + " | time=" + summary.totalTime);
            EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
        }
        catch (System.Exception e)
        {
            Debug.LogError("PIMLR BUILD EXCEPTION: " + e);
            EditorApplication.Exit(3);
        }
    }

    // Bake lighting (needs GPU -> run WITHOUT -nographics) for each enabled scene, then build.
    public static void BuildWebGLBaked()
    {
        try
        {
            Lightmapping.giWorkflowMode = Lightmapping.GIWorkflowMode.OnDemand;
            string[] scenes = EditorBuildSettings.scenes
                .Where(s => s.enabled).Select(s => s.path).ToArray();

            foreach (var sp in scenes)
            {
                var sc = UnityEditor.SceneManagement.EditorSceneManager.OpenScene(
                    sp, UnityEditor.SceneManagement.OpenSceneMode.Single);
                Debug.Log("PIMLR BAKE: baking " + sp + " ...");
                bool ok = Lightmapping.Bake();
                Debug.Log("PIMLR BAKE: " + sp + " baked=" + ok);
                UnityEditor.SceneManagement.EditorSceneManager.SaveScene(sc);
            }

            string outDir = "Build/WebGL";
            System.IO.Directory.CreateDirectory(outDir);
            var opts = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = outDir,
                target = BuildTarget.WebGL,
                targetGroup = BuildTargetGroup.WebGL,
                options = BuildOptions.None
            };
            BuildReport report = BuildPipeline.BuildPlayer(opts);
            var summary = report.summary;
            Debug.Log("PIMLR BAKED BUILD RESULT: " + summary.result + " | size=" + summary.totalSize + " | time=" + summary.totalTime);
            EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
        }
        catch (System.Exception e)
        {
            Debug.LogError("PIMLR BAKED BUILD EXCEPTION: " + e);
            EditorApplication.Exit(3);
        }
    }
}
