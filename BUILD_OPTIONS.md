# Build Options for WebOS Scroll Fix Plugin

## Why .NET is Required

**Yes, Jellyfin plugins MUST be built with .NET SDK** because:
- Jellyfin server is built on .NET
- Plugins are .NET assemblies (DLLs) that load into the Jellyfin process
- This ensures compatibility and integration with Jellyfin's plugin system

## Build Options

### Option 1: GitHub Actions (Automatic - Recommended)

The repository includes a GitHub Actions workflow that automatically builds and creates releases when you push a tag.

**To trigger a build:**
```bash
git tag v1.0.0.0
git push --tags
```

The workflow will:
1. Build the plugin using .NET 6.0
2. Create a release package (ZIP file)
3. Create a GitHub release
4. Upload the package

**Then update manifest.json** with the checksum and push.

### Option 2: Build on Server (If .NET is Available)

If your server has .NET SDK installed:

```bash
# On server
cd /path/to/jellyfin-plugin-webos-scroll-fix
dotnet build -c Release
cp bin/Release/net6.0/Jellyfin.Plugin.WebOSScrollFix.dll /opt/media/config/jellyfin/plugins/
docker restart jellyfin
```

### Option 3: Build Locally (If You Have .NET SDK)

```bash
# Install .NET 6.0 SDK: https://dotnet.microsoft.com/download
cd jellyfin-plugin-webos-scroll-fix
dotnet build -c Release
# Then copy DLL to server or create release
```

### Option 4: Use Pre-built DLL (Manual Installation)

You can manually install the DLL without using the repository system:

1. Get a pre-built DLL (from GitHub Actions artifact or build)
2. Copy to: `/opt/media/config/jellyfin/plugins/`
3. Restart Jellyfin

## Current Status

The GitHub Actions workflow is set up and ready. To create the first release:

1. **Push a tag** (already done if you ran the tag command)
2. **Wait for GitHub Actions** to build and create release
3. **Download the ZIP** from the release
4. **Calculate checksum**: `md5sum Jellyfin.Plugin.WebOSScrollFix_1.0.0.0.zip`
5. **Update manifest.json** with checksum
6. **Push updated manifest**

## Alternative: Manual Installation (No Build Needed)

If you just want to use the fix without the plugin system, you can still use the direct file injection method (the original approach). The plugin method is better long-term, but the direct injection works immediately.

