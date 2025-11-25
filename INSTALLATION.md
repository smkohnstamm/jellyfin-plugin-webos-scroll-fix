# Installation Guide

## Quick Start

### Step 1: Install File Transformation Plugin (Required)

The WebOS Scroll Fix plugin requires the File Transformation plugin to work.

1. **Access Jellyfin Dashboard**: `https://jellyfin.vubox.stream` (or your Jellyfin URL)
2. **Navigate to**: Dashboard → Plugins → Repositories
3. **Click**: Add Repository
4. **Enter**:
   - **Name:** `IAmParadox Plugins`
   - **URL:** `https://www.iamparadox.dev/jellyfin/plugins/manifest.json`
5. **Click**: Save
6. **Go to**: Dashboard → Plugins → Catalog
7. **Search for**: "File Transformation"
8. **Click**: Install
9. **Restart Jellyfin**

### Step 2: Install WebOS Scroll Fix Plugin

#### Option A: Via Plugin Repository (Easiest)

1. **Add Repository**:
   - Go to: Dashboard → Plugins → Repositories
   - Click: Add Repository
   - Enter:
     - **Name:** `WebOS Scroll Fix`
     - **URL:** `https://raw.githubusercontent.com/smkohnstamm/jellyfin-plugin-webos-scroll-fix/master/manifest.json`
   - Click: Save

2. **Install Plugin**:
   - Go to: Dashboard → Plugins → Catalog
   - Search for: "WebOS Scroll Fix"
   - Click: Install
   - Restart Jellyfin

3. **Verify**:
   - Go to: Dashboard → Plugins → Installed
   - "WebOS Scroll Fix" should be listed and enabled

#### Option B: Manual Installation

If you prefer to build and install manually:

```bash
# 1. Build the plugin
cd jellyfin-plugin-webos-scroll-fix
dotnet build -c Release

# 2. Copy to plugins directory
# Docker: /opt/media/config/jellyfin/plugins/
# Linux: ~/.local/share/jellyfin/plugins/
cp bin/Release/net6.0/Jellyfin.Plugin.WebOSScrollFix.dll /path/to/jellyfin/plugins/

# 3. Restart Jellyfin
docker restart jellyfin  # or systemctl restart jellyfin
```

### Step 3: Test on WebOS

1. **Restart Jellyfin app on WebOS TV** (not just refresh)
2. **Navigate to**: Live TV → Guide
3. **Use arrow keys** to navigate down
4. **Verify**: Items should scroll into view automatically

## Troubleshooting

### Plugin Not Appearing in Catalog

- **Check repository URL** is correct (must be raw GitHub URL)
- **Refresh plugin catalog** or restart Jellyfin
- **Check Jellyfin logs** for errors: `docker logs jellyfin | grep -i plugin`

### Transformation Not Working

- **Verify File Transformation plugin is installed and enabled**
- **Check plugin is enabled**: Dashboard → Plugins → Installed
- **Check logs**: `docker logs jellyfin | grep -i "webos\|scroll"`
- **Clear browser cache** and restart Jellyfin app on WebOS TV

### Build Errors

- **Install .NET 6.0 SDK**: https://dotnet.microsoft.com/download
- **Check Jellyfin version** matches plugin target (10.9.0)
- **Update package versions** in .csproj if needed

## Uninstallation

1. **Go to**: Dashboard → Plugins → Installed
2. **Find**: "WebOS Scroll Fix"
3. **Click**: Uninstall
4. **Restart Jellyfin**

The fix will be removed and Jellyfin will return to default behavior.

