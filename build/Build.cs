using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;

using Nuke.Common.Tools.Git;

using Nuke.Common.Git;
using Nuke.Common.Tools.Docker;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Build : NukeBuild
{

    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode


    private List<string> projectAFiles = new List<string>{
        @"^src/projectA/.*\.cs$"
    };

    private List<string> projectBFiles = new List<string>{
        @"^src/projectB/.*\.cs$"
    };

    private bool isAnyFileModified(List<string> pathPatterns) {
        var gitShowOutput = GitTasks.Git("show --name-only");

        foreach (var output in gitShowOutput) {
            foreach (string pathPattern in pathPatterns) {
                if (Regex.Match(output.Text, pathPattern).Success) return true;
            }
        }

        return false;
    }
    
    public static int Main () => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
        });

    Target Restore => _ => _
        .Executes(() =>
        {
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
        });


    // readonly AbsolutePath ApiProject = "C:/Arafath/Dev/dotnet/src/projectA/Dockerfile";
    // readonly AbsolutePath ApiProject2 = "C:/Arafath/Dev/dotnet/src/projectA";
    Target ProjectA => _ => _
    .OnlyWhenDynamic(() => {
        return isAnyFileModified(projectAFiles);
    })
    .Executes(() =>
    {
        DockerTasks.DockerBuild(x => x
            .SetPath(RootDirectory)
            .SetFile(RootDirectory / "src/projectA/Dockerfile")
            .SetTag("docker-image-a")
        );
    });


    Target ProjectB => _ => _
    .OnlyWhenDynamic(() => {
        return isAnyFileModified(projectBFiles);
    })
    .Executes(() =>
    {
        DockerTasks.DockerBuild(x => x
            .SetPath(RootDirectory)
            .SetFile(RootDirectory / "src/projectA/Dockerfile")
            .SetTag("docker-image-b")
        );
    });

}
