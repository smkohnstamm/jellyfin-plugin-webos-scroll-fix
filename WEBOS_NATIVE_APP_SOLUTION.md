# WebOS Native App - Solution

## The Problem

The File Transformation plugin approach **may not work for WebOS native apps** because:

1. **File Transformation intercepts HTTP requests** - Works for web browsers ✅
2. **WebOS native app** may:
   - Bundle web files locally
   - Cache files aggressively  
   - Not make HTTP requests for `index.html`
   - Use a different file loading mechanism

## The Solution

**Use direct file injection for WebOS native app:**

```bash
# This modifies files directly on disk (works for ALL clients)
cd /Users/simonkohnstamm/Documents/Projects/media-server
./apply_webos_scroll_fix.sh
docker restart jellyfin
```

## Why This Works

### Direct File Injection ✅
- Modifies `index.html` directly on disk
- All clients (web, native, mobile) read from same files
- Works immediately
- No HTTP interception needed

### File Transformation Plugin ❌ (for native apps)
- Only intercepts HTTP requests
- Native apps may not make HTTP requests
- May use bundled/cached files
- May not work for WebOS

## Verification

### Step 1: Test in Web Browser

Before testing on WebOS, verify the fix works in a web browser:

1. Open Jellyfin: `https://jellyfin.vubox.stream`
2. Open browser console (F12)
3. Go to Live TV → Guide
4. Check console for: `[TV Guide Scroll Fix] Initializing...`
5. Test navigation with arrow keys

**If it works in browser:**
- The fix is working
- If it doesn't work on WebOS, the app may be caching files
- Try clearing WebOS app cache or reinstalling app

**If it doesn't work in browser:**
- Check Jellyfin logs for errors
- Verify files were injected correctly

### Step 2: Check Jellyfin Logs

```bash
# Check if files were injected
docker exec jellyfin grep -q "tv-guide-fix.css" /usr/share/jellyfin/web/index.html && echo "✅ CSS injected" || echo "❌ CSS not found"
docker exec jellyfin grep -q "tv-guide-scroll-fix.js" /usr/share/jellyfin/web/index.html && echo "✅ JS injected" || echo "❌ JS not found"
```

### Step 3: Clear WebOS App Cache

The WebOS app may be using cached files:

1. **Uninstall Jellyfin app** on WebOS TV
2. **Reinstall Jellyfin app** from app store
3. **Restart TV**
4. **Test again**

## Recommended Approach

**For WebOS native app, use direct file injection:**

1. Run `./apply_webos_scroll_fix.sh`
2. Restart Jellyfin
3. Clear WebOS app cache (uninstall/reinstall)
4. Test

**Keep the plugin for:**
- Web browser clients (if it works)
- Future compatibility
- Other clients that make HTTP requests

## Alternative: Hybrid Approach

You can use **both** methods:

1. **Plugin** for web browsers (if it works)
2. **Direct injection** for WebOS native app

They won't conflict - direct injection modifies files, plugin intercepts requests.

## Summary

- ✅ **Direct file injection** works for all clients including WebOS
- ❌ **File Transformation plugin** may not work for WebOS native app
- 🔍 **Test in browser first** to verify fix works
- 🧹 **Clear WebOS cache** if fix doesn't appear

