# Repository Status

## ✅ Completed

- [x] Plugin source code created
- [x] Plugin manifest.json created
- [x] Documentation added (README, INSTALLATION, QUICK_START, etc.)
- [x] Git repository initialized
- [x] Pushed to GitHub: https://github.com/smkohnstamm/jellyfin-plugin-webos-scroll-fix
- [x] GitHub Actions workflow for releases added

## 📋 Next Steps (To Make Installable via Repository)

To make the plugin installable via Jellyfin's plugin repository system, you need to:

### 1. Build the Plugin

```bash
cd jellyfin-plugin-webos-scroll-fix
dotnet build -c Release
```

### 2. Create Release Package

```bash
mkdir -p release
cp bin/Release/net6.0/Jellyfin.Plugin.WebOSScrollFix.dll release/
cd release
zip -r ../Jellyfin.Plugin.WebOSScrollFix_1.0.0.0.zip .
cd ..
```

### 3. Calculate Checksum

```bash
md5 Jellyfin.Plugin.WebOSScrollFix_1.0.0.0.zip
# Or on Linux:
md5sum Jellyfin.Plugin.WebOSScrollFix_1.0.0.0.zip
```

### 4. Update manifest.json

Update the checksum field in manifest.json with the MD5 hash.

### 5. Create GitHub Release

```bash
gh release create v1.0.0.0 \
  Jellyfin.Plugin.WebOSScrollFix_1.0.0.0.zip \
  --title "v1.0.0.0 - Initial Release" \
  --notes "Initial release of WebOS Scroll Fix plugin. Fixes TV Guide scroll issue on WebOS devices."
```

### 6. Commit and Push Updated manifest.json

```bash
git add manifest.json
git commit -m "Update manifest with release checksum"
git push
```

## 🎯 Current Repository URL

**For Jellyfin Plugin Repository:**
```
https://raw.githubusercontent.com/smkohnstamm/jellyfin-plugin-webos-scroll-fix/master/manifest.json
```

## 📝 Installation Instructions (For Users)

Once the release is created, users can install via:

1. **Dashboard → Plugins → Repositories → Add:**
   - URL: `https://raw.githubusercontent.com/smkohnstamm/jellyfin-plugin-webos-scroll-fix/master/manifest.json`
2. **Dashboard → Plugins → Catalog → Install:** "WebOS Scroll Fix"

## ⚠️ Note

The manifest.json currently references a release that doesn't exist yet. The plugin won't be installable via the repository until:
1. A release is created on GitHub
2. The manifest.json checksum is updated
3. The manifest.json is committed and pushed

