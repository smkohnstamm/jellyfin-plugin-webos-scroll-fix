# Build and Install WebOS Scroll Fix Plugin

## Quick Install

### Step 1: Install File Transformation Plugin

1. Go to Jellyfin Dashboard: `https://jellyfin.vubox.stream`
2. Navigate to: **Dashboard → Plugins → Repositories**
3. Click **Add Repository**
4. Enter:
   - **Name:** `IAmParadox Plugins`
   - **URL:** `https://www.iamparadox.dev/jellyfin/plugins/manifest.json`
5. Click **Save**
6. Go to: **Dashboard → Plugins → Catalog**
7. Search for: **File Transformation**
8. Click **Install**
9. **Restart Jellyfin**

### Step 2: Build WebOS Scroll Fix Plugin

```bash
cd jellyfin-plugin-webos-scroll-fix
dotnet build -c Release
```

### Step 3: Install Plugin

```bash
# Find Jellyfin plugins directory
# Docker: Usually /opt/media/config/jellyfin/plugins/
# Or check: docker exec jellyfin ls -la /config/plugins/

# Copy plugin DLL
cp bin/Release/net6.0/Jellyfin.Plugin.WebOSScrollFix.dll /opt/media/config/jellyfin/plugins/

# Set permissions
chmod 644 /opt/media/config/jellyfin/plugins/Jellyfin.Plugin.WebOSScrollFix.dll
```

### Step 4: Restart Jellyfin

```bash
docker restart jellyfin
```

### Step 5: Verify

1. Go to: **Dashboard → Plugins → Installed**
2. "WebOS Scroll Fix" should be listed
3. Check logs: `docker logs jellyfin | grep -i "webos\|scroll"`
4. Should see: "WebOS Scroll Fix transformation registered successfully"

## Automated Installation Script

```bash
#!/bin/bash
# Install WebOS Scroll Fix plugin

set -e

PLUGINS_DIR="/opt/media/config/jellyfin/plugins"
PLUGIN_DIR="jellyfin-plugin-webos-scroll-fix"

echo "Building plugin..."
cd "$PLUGIN_DIR"
dotnet build -c Release

echo "Copying plugin..."
mkdir -p "$PLUGINS_DIR"
cp bin/Release/net6.0/Jellyfin.Plugin.WebOSScrollFix.dll "$PLUGINS_DIR/"

echo "Restarting Jellyfin..."
docker restart jellyfin

echo "✓ Plugin installed! Check Dashboard → Plugins → Installed"
```

## Troubleshooting

### Plugin Not Appearing

1. Check File Transformation is installed first
2. Check plugin DLL is in correct directory
3. Check Jellyfin logs for errors
4. Verify .NET version matches (requires .NET 6.0)

### Transformation Not Working

1. Check File Transformation plugin is enabled
2. Check Jellyfin logs for "WebOS Scroll Fix transformation registered"
3. Clear browser cache
4. Restart Jellyfin app on WebOS TV

### Build Errors

- Ensure .NET 6.0 SDK is installed
- Check Jellyfin version matches plugin target (10.9.0)
- Update package versions if needed

