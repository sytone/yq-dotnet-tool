# yq.tool — yq as a .NET Global Tool

[![NuGet](https://img.shields.io/nuget/v/yq.tool.svg)](https://www.nuget.org/packages/yq.tool)
[![Release](https://github.com/sytone/yq-dotnet-tool/actions/workflows/release.yml/badge.svg)](https://github.com/sytone/yq-dotnet-tool/actions/workflows/release.yml)

> **Unofficial repackaging** of [mikefarah/yq](https://github.com/mikefarah/yq) as a [.NET global tool](https://learn.microsoft.com/en-us/dotnet/core/tools/global-tools).  
> All credit for the underlying `yq` tool goes to [Mike Farah](https://github.com/mikefarah).

---

## What is this?

`yq` is a portable command-line YAML/JSON/TOML/XML processor.  
This package allows you to install `yq` via the .NET tooling ecosystem without having to manually download or manage platform-specific binaries.

```bash
dotnet tool install -g yq.tool
```

After installation the `yq` command is available in your terminal, exactly as if you had installed the native binary directly.

---

## Requirements

- [.NET 8 SDK or Runtime](https://dotnet.microsoft.com/download) (or later).

---

## Install

### As a global tool

```bash
dotnet tool install -g yq.tool
```

### As a local tool

```bash
dotnet new tool-manifest   # if no manifest exists
dotnet tool install yq.tool
dotnet tool run yq --version
```

---

## Upgrade

```bash
dotnet tool update -g yq.tool
```

---

## Uninstall

```bash
dotnet tool uninstall -g yq.tool
```

---

## Usage

Once installed the command `yq` is available exactly as documented by upstream:

```bash
yq --version
yq eval '.name' file.yaml
echo '{"foo": "bar"}' | yq -P   # pretty-print
```

See the [mikefarah/yq documentation](https://mikefarah.gitbook.io/yq/) for full usage.

---

## Supported platforms

| .NET RID    | OS      | Architecture |
|-------------|---------|-------------|
| `win-x64`   | Windows | x86-64      |
| `win-arm64` | Windows | ARM64       |
| `linux-x64` | Linux   | x86-64      |
| `linux-arm64`| Linux  | ARM64       |
| `osx-x64`   | macOS   | x86-64      |
| `osx-arm64` | macOS   | ARM64 (Apple Silicon) |

---

## Versioning

Package versions mirror the upstream `mikefarah/yq` releases exactly.  
A tag `vX.Y.Z` upstream becomes NuGet version `X.Y.Z`.

---

## Security

- Native binaries are downloaded directly from the [official mikefarah/yq GitHub releases](https://github.com/mikefarah/yq/releases).
- Checksums are verified against the upstream-provided checksum file during the release workflow.
- If the checksum file is unavailable for a release, the pipeline **fails** rather than packaging unverified binaries.
- No modifications are made to the upstream binaries.

---

## How it works

The package contains:

1. A small .NET launcher (`Yq.Tool`) that detects the current OS and CPU architecture at runtime, resolves the path to the bundled native `yq` binary, ensures it is executable (on Unix), and executes it — forwarding all arguments and inheriting stdin/stdout/stderr.
2. The native `yq` binaries for each supported platform, embedded under `tools/net8.0/any/runtimes/<rid>/native/`.

---

## Disclaimer

This is an **unofficial** repackaging.  
It is not affiliated with, endorsed by, or supported by Mike Farah or the `yq` project.

If you encounter issues with `yq` behaviour, please report them [upstream](https://github.com/mikefarah/yq/issues).  
If you encounter issues with the .NET packaging or installation, please open an issue here.

---

## License

The launcher code in this repository is released under the [MIT License](LICENSE).

The upstream `yq` binary is licensed under the [MIT License](https://github.com/mikefarah/yq/blob/master/LICENSE) by Mike Farah. A copy is included in the distributed package for attribution.