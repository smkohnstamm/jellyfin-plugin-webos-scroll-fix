# Plugin Repository Setup

## Repository URL

To install this plugin via Jellyfin's plugin repository system:

**Repository URL:**
```
https://raw.githubusercontent.com/smkohnstamm/jellyfin-plugin-webos-scroll-fix/master/manifest.json
```

## Installation Steps

1. **Go to Jellyfin Dashboard**: `https://jellyfin.vubox.stream` (or your Jellyfin URL)
2. **Navigate to**: Dashboard → Plugins → Repositories
3. **Click**: Add Repository
4. **Enter**:
   - **Name:** `WebOS Scroll Fix`
   - **URL:** `https://raw.githubusercontent.com/smkohnstamm/jellyfin-plugin-webos-scroll-fix/master/manifest.json`
5. **Click**: Save
6. **Go to**: Dashboard → Plugins → Catalog
7. **Search for**: "WebOS Scroll Fix"
8. **Click**: Install
9. **Restart Jellyfin**

## Prerequisites

**Important:** This plugin requires the **File Transformation** plugin to be installed first.

1. **Install File Transformation plugin:**
   - Add repository: `https://www.iamparadox.dev/jellyfin/plugins/manifest.json`
   - Install "File Transformation" plugin
   - Restart Jellyfin

2. **Then install WebOS Scroll Fix** using the steps above.

## Creating Releases

To create a new release that can be installed via the repository:

1. **Build the plugin:**
   ```bash
   dotnet build -c Release
   ```

2. **Create release package:**
   ```bash
   mkdir -p release
   cp bin/Release/net6.0/Jellyfin.Plugin.WebOSScrollFix.dll release/
   cd release
   zip -r ../Jellyfin.Plugin.WebOSScrollFix_1.0.0.0.zip .
   ```

3. **Create GitHub release:**
   ```bash
   gh release create v1.0.0.0 Jellyfin.Plugin.WebOSScrollFix_1.0.0.0.zip --title "v1.0.0.0" --notes "Initial release"
   ```

4. **Update manifest.json** with:
   - New version number
   - Release URL
   - Checksum (MD5 of the zip file)
   - Timestamp

5. **Commit and push** the updated manifest.json

## Manifest Format

The manifest.json follows Jellyfin's plugin manifest format:

```json
[
    {
        "category": "General",
        "guid": "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
        "name": "WebOS Scroll Fix",
        "description": "...",
        "owner": "smkohnstamm",
        "overview": "...",
        "versions": [
            {
                "version": "1.0.0.0",
                "targetAbi": "10.9.0.0",
                "sourceUrl": "https://github.com/smkohnstamm/jellyfin-plugin-webos-scroll-fix/releases/download/v1.0.0.0/Jellyfin.Plugin.WebOSScrollFix_1.0.0.0.zip",
                "checksum": "",
                "changelog": "...",
                "timestamp": "2025-01-27T00:00:00Z"
            }
        ]
    }
]
```

## GitHub Actions

A GitHub Actions workflow (`.github/workflows/release.yml`) is included to automatically:
- Build the plugin on tag pushes
- Create release packages
- Upload to GitHub Releases

To use:
```bash
git tag v1.0.0.0
git push --tags
```

The workflow will automatically build and create a release.

