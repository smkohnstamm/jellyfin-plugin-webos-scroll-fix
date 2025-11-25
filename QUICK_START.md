# Quick Start Guide

## Install via Plugin Repository (Easiest)

### Step 1: Install File Transformation Plugin (Required)

1. **Dashboard → Plugins → Repositories → Add:**
   - Name: `IAmParadox Plugins`
   - URL: `https://www.iamparadox.dev/jellyfin/plugins/manifest.json`
2. **Dashboard → Plugins → Catalog → Install:** "File Transformation"
3. **Restart Jellyfin**

### Step 2: Install WebOS Scroll Fix

1. **Dashboard → Plugins → Repositories → Add:**
   - Name: `WebOS Scroll Fix`
   - URL: `https://raw.githubusercontent.com/smkohnstamm/jellyfin-plugin-webos-scroll-fix/master/manifest.json`
2. **Dashboard → Plugins → Catalog → Install:** "WebOS Scroll Fix"
3. **Restart Jellyfin**

### Step 3: Test

1. **Restart Jellyfin app on WebOS TV**
2. **Navigate to:** Live TV → Guide
3. **Use arrow keys** to navigate down
4. **Items should scroll into view automatically** ✅

## What It Does

- Fixes TV Guide scroll issue on WebOS (LG TVs)
- Injects CSS and JavaScript via File Transformation plugin
- Survives Jellyfin updates
- Works with all clients (web, mobile, TV apps)

## Troubleshooting

- **Plugin not appearing?** Check repository URL is correct (must be raw GitHub URL)
- **Not working?** Verify File Transformation plugin is installed and enabled
- **Need help?** Check `INSTALLATION.md` for detailed troubleshooting

## Repository

**GitHub:** https://github.com/smkohnstamm/jellyfin-plugin-webos-scroll-fix

