#!/bin/bash
# Install WebOS Scroll Fix Jellyfin Plugin

set -e

PLUGINS_DIR="/opt/media/config/jellyfin/plugins"
PLUGIN_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "=== Installing WebOS Scroll Fix Plugin ==="
echo ""

# Check if .NET SDK is available
if ! command -v dotnet &> /dev/null; then
    echo "Error: .NET SDK not found. Please install .NET 8.0 SDK first."
    echo "Visit: https://dotnet.microsoft.com/download"
    exit 1
fi

# Check if File Transformation plugin is installed
echo "Checking for File Transformation plugin..."
if ! docker exec jellyfin test -f /config/plugins/Jellyfin.Plugin.FileTransformation.dll 2>/dev/null; then
    echo "⚠ Warning: File Transformation plugin not found!"
    echo ""
    echo "Please install File Transformation plugin first:"
    echo "1. Go to: Dashboard → Plugins → Repositories"
    echo "2. Add: https://www.iamparadox.dev/jellyfin/plugins/manifest.json"
    echo "3. Install 'File Transformation' plugin"
    echo "4. Restart Jellyfin"
    echo ""
    read -p "Continue anyway? (y/N): " -n 1 -r
    echo ""
    if [[ ! $REPLY =~ ^[Yy]$ ]]; then
        exit 1
    fi
fi

# Build plugin
echo "Building plugin..."
cd "$PLUGIN_DIR"
dotnet build -c Release

if [ ! -f "bin/Release/net8.0/Jellyfin.Plugin.WebOSScrollFix.dll" ]; then
    echo "Error: Build failed or DLL not found"
    exit 1
fi

# Copy to plugins directory
echo ""
echo "Installing plugin..."
mkdir -p "$PLUGINS_DIR"
cp bin/Release/net8.0/Jellyfin.Plugin.WebOSScrollFix.dll "$PLUGINS_DIR/"
chmod 644 "$PLUGINS_DIR/Jellyfin.Plugin.WebOSScrollFix.dll"

echo "✓ Plugin installed to: $PLUGINS_DIR"
echo ""

# Restart Jellyfin
echo "Restarting Jellyfin..."
docker restart jellyfin

echo ""
echo "=== Installation Complete ==="
echo ""
echo "Next steps:"
echo "1. Wait for Jellyfin to restart (30-60 seconds)"
echo "2. Go to: Dashboard → Plugins → Installed"
echo "3. Verify 'WebOS Scroll Fix' is listed"
echo "4. Restart Jellyfin app on WebOS TV"
echo "5. Test navigation in Live TV → Guide"
echo ""

