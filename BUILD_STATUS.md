# Build Status

## Current Issue

The plugin requires **.NET 8.0** to build (Jellyfin 10.9.0 requirement), but there are compilation errors that need to be resolved.

## Summary

**Yes, Jellyfin plugins MUST be built with .NET SDK** because:
- Jellyfin server is built on .NET
- Plugins are .NET assemblies (DLLs) that load into the Jellyfin process
- This ensures compatibility and integration

## Build Options

### Option 1: GitHub Actions (Automatic - When Fixed)

The repository includes a GitHub Actions workflow that automatically builds when you push a tag. Currently there are compilation errors that need to be fixed first.

**To trigger a build:**
```bash
git tag v1.0.0.0
git push --tags
```

### Option 2: Build on Server (If .NET 8.0 SDK is Available)

If your server has .NET 8.0 SDK installed:

```bash
# On server
cd /path/to/jellyfin-plugin-webos-scroll-fix
dotnet build -c Release
cp bin/Release/net8.0/Jellyfin.Plugin.WebOSScrollFix.dll /opt/media/config/jellyfin/plugins/
docker restart jellyfin
```

### Option 3: Build Locally (If You Have .NET 8.0 SDK)

```bash
# Install .NET 8.0 SDK: https://dotnet.microsoft.com/download
cd jellyfin-plugin-webos-scroll-fix
dotnet build -c Release
# Then copy DLL to server or create release
```

### Option 4: Use Direct File Injection (No Build Needed)

If you just want the fix to work immediately without building a plugin, you can use the original direct file injection method:

```bash
./apply_webos_scroll_fix.sh
```

This modifies Jellyfin's web files directly on the server (the approach we used before creating the plugin).

## Current Compilation Errors

The plugin currently has compilation errors related to:
- `IJsonSerializer` namespace/package resolution
- Plugin API compatibility with Jellyfin 10.9.0

These need to be resolved before the plugin can be built and released.

## Next Steps

1. **Fix compilation errors** - Resolve the `IJsonSerializer` and API compatibility issues
2. **Test build locally** - Build the plugin locally to verify it compiles
3. **Create release** - Once building successfully, create a GitHub release
4. **Update manifest** - Add checksum to manifest.json
5. **Install via repository** - Add the repository URL to Jellyfin and install

## Alternative: Manual Installation

Until the plugin builds successfully, you can:
- Use the direct file injection method (`apply_webos_scroll_fix.sh`)
- Or wait for the plugin build issues to be resolved

