#!/bin/bash
# Build plugin and create GitHub release

set -e

PLUGIN_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
VERSION="1.0.0.0"
INSTANCE_ID="${1:-i-0e4108534892040a4}"

echo "=== Building and Creating Release ==="
echo ""

# Check if we're on the server (has docker and .NET)
if command -v dotnet &> /dev/null && [ -d "/opt/media" ]; then
    echo "Building on server..."
    cd "$PLUGIN_DIR"
    
    # Build
    dotnet build -c Release
    
    # Create release package
    mkdir -p release
    cp bin/Release/net6.0/Jellyfin.Plugin.WebOSScrollFix.dll release/
    cd release
    zip -r ../Jellyfin.Plugin.WebOSScrollFix_${VERSION}.zip .
    cd ..
    
    # Calculate checksum
    if command -v md5sum &> /dev/null; then
        CHECKSUM=$(md5sum Jellyfin.Plugin.WebOSScrollFix_${VERSION}.zip | cut -d' ' -f1)
    elif command -v md5 &> /dev/null; then
        CHECKSUM=$(md5 -q Jellyfin.Plugin.WebOSScrollFix_${VERSION}.zip)
    else
        echo "Warning: Could not calculate checksum"
        CHECKSUM=""
    fi
    
    echo "Checksum: $CHECKSUM"
    
    # Update manifest.json
    if [ -n "$CHECKSUM" ]; then
        # Update checksum in manifest.json
        if [[ "$OSTYPE" == "darwin"* ]]; then
            sed -i '' "s/\"checksum\": \"\"/\"checksum\": \"$CHECKSUM\"/" manifest.json
        else
            sed -i "s/\"checksum\": \"\"/\"checksum\": \"$CHECKSUM\"/" manifest.json
        fi
    fi
    
    # Create GitHub release
    echo ""
    echo "Creating GitHub release..."
    gh release create v${VERSION} \
        Jellyfin.Plugin.WebOSScrollFix_${VERSION}.zip \
        --title "v${VERSION} - Initial Release" \
        --notes "Initial release of WebOS Scroll Fix plugin.

Fixes TV Guide scroll issue on WebOS devices (LG TVs). When navigating down in the TV Guide using arrow keys, items now automatically scroll into view instead of disappearing off the bottom of the screen.

**Requires File Transformation plugin to be installed first.**"
    
    # Commit and push updated manifest
    git add manifest.json
    git commit -m "Update manifest with release checksum for v${VERSION}" || true
    git push
    
    echo ""
    echo "✓ Release created successfully!"
    echo ""
    echo "Plugin can now be installed via repository:"
    echo "https://raw.githubusercontent.com/smkohnstamm/jellyfin-plugin-webos-scroll-fix/master/manifest.json"
    
else
    echo "Building on remote server via AWS SSM..."
    
    # Upload files to S3 first
    BUCKET="vubox-stream-movies-144638214161"
    echo "Uploading plugin source to S3..."
    cd "$PLUGIN_DIR"
    tar -czf /tmp/webos-plugin-source.tar.gz --exclude='.git' --exclude='bin' --exclude='obj' .
    aws s3 cp /tmp/webos-plugin-source.tar.gz "s3://$BUCKET/deployment/webos-plugin-source.tar.gz"
    
    # Build on server
    COMMAND_ID=$(aws ssm send-command \
        --instance-ids "$INSTANCE_ID" \
        --region us-east-1 \
        --document-name "AWS-RunShellScript" \
        --parameters "commands=[
            'cd /tmp',
            'aws s3 cp s3://$BUCKET/deployment/webos-plugin-source.tar.gz .',
            'tar -xzf webos-plugin-source.tar.gz',
            'cd jellyfin-plugin-webos-scroll-fix || cd webos-plugin-source',
            'dotnet build -c Release',
            'mkdir -p release',
            'cp bin/Release/net6.0/Jellyfin.Plugin.WebOSScrollFix.dll release/',
            'cd release',
            'zip -r ../Jellyfin.Plugin.WebOSScrollFix_${VERSION}.zip .',
            'cd ..',
            'md5sum Jellyfin.Plugin.WebOSScrollFix_${VERSION}.zip > checksum.txt',
            'cat checksum.txt'
        ]" \
        --output text \
        --query 'Command.CommandId')
    
    echo "Command ID: $COMMAND_ID"
    echo "Waiting for build..."
    sleep 20
    
    # Get checksum
    OUTPUT=$(aws ssm get-command-invocation \
        --command-id "$COMMAND_ID" \
        --instance-id "$INSTANCE_ID" \
        --region us-east-1 \
        --query 'StandardOutputContent' \
        --output text)
    
    CHECKSUM=$(echo "$OUTPUT" | grep -oE '[a-f0-9]{32}' | head -1)
    echo "Checksum: $CHECKSUM"
    
    # Download the zip file
    aws ssm send-command \
        --instance-ids "$INSTANCE_ID" \
        --region us-east-1 \
        --document-name "AWS-RunShellScript" \
        --parameters "commands=[
            'cd /tmp',
            'aws s3 cp jellyfin-plugin-webos-scroll-fix/Jellyfin.Plugin.WebOSScrollFix_${VERSION}.zip s3://$BUCKET/deployment/ || aws s3 cp webos-plugin-source/Jellyfin.Plugin.WebOSScrollFix_${VERSION}.zip s3://$BUCKET/deployment/'
        ]" \
        --output text \
        --query 'Command.CommandId' > /dev/null
    
    sleep 5
    aws s3 cp "s3://$BUCKET/deployment/Jellyfin.Plugin.WebOSScrollFix_${VERSION}.zip" /tmp/
    
    # Update manifest and create release
    cd "$PLUGIN_DIR"
    if [[ "$OSTYPE" == "darwin"* ]]; then
        sed -i '' "s/\"checksum\": \"\"/\"checksum\": \"$CHECKSUM\"/" manifest.json
    else
        sed -i "s/\"checksum\": \"\"/\"checksum\": \"$CHECKSUM\"/" manifest.json
    fi
    
    # Create GitHub release
    gh release create v${VERSION} \
        /tmp/Jellyfin.Plugin.WebOSScrollFix_${VERSION}.zip \
        --title "v${VERSION} - Initial Release" \
        --notes "Initial release of WebOS Scroll Fix plugin."
    
    # Commit and push
    git add manifest.json
    git commit -m "Update manifest with release checksum for v${VERSION}"
    git push
    
    echo "✓ Release created!"
fi

