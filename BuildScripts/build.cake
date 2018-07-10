
var target = Argument("target", "Default");
var configuration = Argument("Configuration", "Release");

//*****************************************************************************
// Constants

string artefacts = "./Artefacts";

// ****************************************************************************
var netCoreBuildSettings = new DotNetCoreBuildSettings()
{
    Configuration = configuration,
    NoDependencies = false,
    NoIncremental = true,
    NoRestore = false
};

var netCoreDotNetCoreTestSettings = new  DotNetCoreTestSettings()
{
    //TODO: lcov
    Configuration = configuration
};

var netCoreDotNetCorePackSettings = new  DotNetCorePackSettings ()
{
    Configuration = configuration,
    OutputDirectory = artefacts,
    IncludeSource = false,
    IncludeSymbols = false,
    NoBuild = false
};

string[] testProjectNetStandrd = new string[]
{
    "../src/Test/MassiveDynamicProxyGenerator.Tests.NetStandard/MassiveDynamicProxyGenerator.Tests.NetStandard.csproj",
    "../src/Test/MassiveDynamicProxyGenerator.SimpleInjector.Tests.NetCore/MassiveDynamicProxyGenerator.SimpleInjector.Tests.NetCore.csproj",
    "../src/Test/MassiveDynamicProxyGenerator.DependencyInjection.Test/MassiveDynamicProxyGenerator.DependencyInjection.Test.csproj"
};

string[] testProjectNetFull = new string[]
    {
     "../src/Test/MassiveDynamicProxyGenerator.SimpleInjector.Tests/MassiveDynamicProxyGenerator.SimpleInjector.Tests.csproj",
     "../src/Test/MassiveDynamicProxyGenerator.Tests/MassiveDynamicProxyGenerator.Tests.csproj",
    };

// ****************************************************************************
// Tasks

Task("Clean")
     .Does(()=>
     {
          foreach(var projFile in GetFiles("../src/Src/*/*.csproj"))
          {
              var projDirectory = projFile.GetDirectory();
              Information($"Clear {projDirectory}");
              CleanDirectory(projDirectory + Directory("/obj"));
              CleanDirectory(projDirectory + Directory("/bin"));
          }
     
          foreach(var projFile in GetFiles("../src/Test/*/*.csproj"))
          {
              var projDirectory = projFile.GetDirectory();
              Information($"Clear {projDirectory}");
              CleanDirectory(projDirectory + Directory("/obj"));
              CleanDirectory(projDirectory + Directory("/bin"));
          }
     
          CleanDirectory(artefacts);
     });

Task("Build")
    .IsDependentOn("Clean")
    .Does(()=>
    {
         foreach(var projFile in GetFiles("../src/Src/*/*.csproj"))
         {
           DotNetCoreBuild(projFile.ToString(), netCoreBuildSettings);
         }
    });

Task("BuildNetCoreTests")
    .IsDependentOn("Build")
    .Does(()=>
    {
        foreach(string projFile in testProjectNetStandrd)
        {
           DotNetCoreBuild(projFile, netCoreBuildSettings);
        }
    });

Task("RestoreNetFullNugets")
    .IsDependentOn("Clean")
    .Does(()=>
    {
        NuGetRestore("../src/MassiveDynamicProxyGenerator.sln");
    });

Task("BuildNetFullTests")
    .IsDependentOn("Build")
    .IsDependentOn("RestoreNetFullNugets")
    .Does(()=>
    {
        foreach(var projFile in testProjectNetFull)
        {
            MSBuild(projFile, settings =>
                settings.SetConfiguration(configuration));
        }
    });

Task("TestNetStandard")
    .IsDependentOn("Build")
    .IsDependentOn("BuildNetCoreTests")
    .Does(()=>
    {
        foreach(string projFile in testProjectNetStandrd)
        {
           DotNetCoreTest(projFile, netCoreDotNetCoreTestSettings);
        }
    });

Task("TestNetFull")
    .IsDependentOn("Build")
    .IsDependentOn("BuildNetFullTests")
    .Does(()=>
    {
        foreach(string projFile in testProjectNetFull)
        {
           Verbose($"Run tests in {projFile}");
           string dllName = System.IO.Path.ChangeExtension(System.IO.Path.GetFileName(projFile), ".dll");
           string folder = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(projFile), "bin", configuration);
           MSTest(System.IO.Path.Combine(folder, dllName), new MSTestSettings()
           {
               NoIsolation = false,
               WorkingDirectory = folder
           });
        }
    });

Task("Pack")
    .IsDependentOn("TestNetStandard")
    .IsDependentOn("TestNetFull")
    .Does(()=>
    {
        foreach(var projFile in GetFiles("../src/Src/*/*.csproj"))
         {
            DotNetCorePack(projFile.ToString(), netCoreDotNetCorePackSettings);
        }
    });

Task("Default")
    .IsDependentOn("Pack");

//*****************************************************************************
// EXECUTION

RunTarget(target);
