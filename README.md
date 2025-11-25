# WebOS Scroll Fix - Jellyfin Plugin

A Jellyfin plugin that fixes the TV Guide scroll issue on WebOS devices (LG TVs).

## The Problem

On LG WebOS TVs, when navigating down in the TV Guide using arrow keys, the highlighted item disappears off the bottom of the screen instead of scrolling to keep it visible.

**GitHub Issue**: https://github.com/jellyfin/jellyfin-web/issues/5705

## The Solution

This plugin uses the [File Transformation plugin](https://github.com/IAmParadox27/jellyfin-plugin-file-transformation) to inject CSS and JavaScript into Jellyfin's web interface. This approach:

- ✅ **Survives Jellyfin updates** - No file modifications needed
- ✅ **Non-destructive** - Uses plugin system
- ✅ **Proper way** - Follows Jellyfin plugin architecture
- ✅ **Multiple plugins** - Works alongside other File Transformation plugins

## Installation

### Prerequisites

1. **Install File Transformation plugin first:**
   - Go to: Dashboard → Plugins → Repositories
   - Add repository: `https://www.iamparadox.dev/jellyfin/plugins/manifest.json`
   - Install "File Transformation" plugin
   - Restart Jellyfin

### Install WebOS Scroll Fix

#### Method 1: Plugin Repository (Recommended)

1. **Add this repository to Jellyfin:**
   - Go to: Dashboard → Plugins → Repositories
   - Click: **Add Repository**
   - Enter:
     - **Name:** `WebOS Scroll Fix`
     - **URL:** `https://raw.githubusercontent.com/simonkohnstamm/media-server/main/jellyfin-plugin-webos-scroll-fix/manifest.json`
   - Click: **Save**

2. **Install the plugin:**
   - Go to: Dashboard → Plugins → Catalog
   - Search for: **WebOS Scroll Fix**
   - Click: **Install**
   - Restart Jellyfin

3. **Verify installation:**
   - Go to: Dashboard → Plugins → Installed
   - "WebOS Scroll Fix" should be listed and enabled

#### Method 2: Manual Installation

1. **Build the plugin:**
   ```bash
   cd jellyfin-plugin-webos-scroll-fix
   dotnet build -c Release
   ```

2. **Copy to Jellyfin plugins directory:**
   ```bash
   # Find your Jellyfin plugins directory
   # Usually: /opt/media/config/jellyfin/plugins/ (Docker)
   # Or: ~/.local/share/jellyfin/plugins/ (Linux)
   
   cp bin/Release/net6.0/Jellyfin.Plugin.WebOSScrollFix.dll /path/to/jellyfin/plugins/
   ```

3. **Restart Jellyfin**

4. **Verify installation:**
   - Go to: Dashboard → Plugins → Installed
   - "WebOS Scroll Fix" should be listed and enabled

## How It Works

1. Plugin registers with File Transformation on installation
2. File Transformation intercepts `index.html` requests
3. Our plugin transforms the HTML to inject:
   - CSS for proper overflow and scroll behavior
   - JavaScript that handles `scrollIntoView()` on focus/keyboard events
4. All clients (including WebOS app) receive the transformed content

## Testing

After installation:

1. **Restart Jellyfin app on WebOS TV** (not just refresh)
2. **Navigate to**: Live TV → Guide
3. **Use arrow keys** to navigate down
4. **Verify**: Items should scroll into view automatically

## Development

### Building

```bash
dotnet build -c Release
```

### Requirements

- .NET 6.0 SDK
- Jellyfin 10.9.0+ (adjust version in .csproj if needed)
- File Transformation plugin installed

## Files

- `Plugin.cs` - Main plugin class, registers with File Transformation
- `TransformationHandler.cs` - Handles HTML transformation
- `PluginConfiguration.cs` - Plugin configuration (currently empty)

## License

GPL-3.0 (same as Jellyfin)

## Credits

- Based on [File Transformation plugin](https://github.com/IAmParadox27/jellyfin-plugin-file-transformation) by IAmParadox27
- Fixes issue: https://github.com/jellyfin/jellyfin-web/issues/5705

