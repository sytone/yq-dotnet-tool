using System.Diagnostics;
using System.Runtime.InteropServices;

// Determine the Runtime Identifier (RID) for the current platform.
string rid;
try
{
    rid = GetRuntimeIdentifier();
}
catch (PlatformNotSupportedException ex)
{
    Console.Error.WriteLine($"yq.tool: {ex.Message}");
    return 1;
}

// The native binaries are placed next to this assembly under runtimes/<rid>/native/
string assemblyDir = AppContext.BaseDirectory;
string nativeName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "yq.exe" : "yq";
string nativePath = Path.Combine(assemblyDir, "runtimes", rid, "native", nativeName);

if (!File.Exists(nativePath))
{
    Console.Error.WriteLine(
        $"yq.tool: native binary not found for platform '{rid}' at '{nativePath}'.");
    Console.Error.WriteLine(
        "Supported platforms: win-x64, win-arm64, linux-x64, linux-arm64, osx-x64, osx-arm64.");
    return 1;
}

// On Unix ensure the binary is executable (best-effort).
EnsureExecutable(nativePath);

using var process = new Process();
process.StartInfo = new ProcessStartInfo
{
    FileName = nativePath,
    UseShellExecute = false,
    RedirectStandardInput = false,
    RedirectStandardOutput = false,
    RedirectStandardError = false,
};

foreach (string arg in args)
{
    process.StartInfo.ArgumentList.Add(arg);
}

process.Start();
await process.WaitForExitAsync();
return process.ExitCode;

// ---------------------------------------------------------------------------
// Helpers
// ---------------------------------------------------------------------------

static string GetRuntimeIdentifier()
{
    string os;
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        os = "win";
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        os = "osx";
    else
        os = "linux";

    string arch = RuntimeInformation.ProcessArchitecture switch
    {
        Architecture.X64 => "x64",
        Architecture.Arm64 => "arm64",
        _ => throw new PlatformNotSupportedException(
            $"Unsupported CPU architecture: {RuntimeInformation.ProcessArchitecture}. " +
            "Supported architectures are: x64, arm64."),
    };

    return $"{os}-{arch}";
}

static void EnsureExecutable(string path)
{
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        return;

    try
    {
        using var chmod = Process.Start(new ProcessStartInfo
        {
            FileName = "chmod",
            ArgumentList = { "+x", path },
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        });
        chmod?.WaitForExit();
    }
    catch
    {
        // Best-effort; ignore failures.
    }
}
