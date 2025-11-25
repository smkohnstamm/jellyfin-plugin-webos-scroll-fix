# Diagnosing WebOS Plugin Issue

## Problem

Plugin installed but scrolling fix doesn't work on WebOS native app.

## Root Cause Hypothesis

**File Transformation plugin intercepts HTTP requests**, but:
- ✅ **Web browsers**: Make HTTP requests → Can be intercepted
- ❌ **WebOS native app**: May bundle/cache files → May not make HTTP requests

## Diagnostic Steps

### 1. Check if Transformation is Being Called

```bash
# Check Jellyfin logs for transformation activity
docker logs jellyfin --tail 200 | grep -i "webos\|scroll\|transformation"

# Look for:
# - "[WebOS Scroll Fix] Transformation registered successfully"
# - "[WebOS Scroll Fix] Transforming index.html..."
# - "[WebOS Scroll Fix] Injected CSS"
# - "[WebOS Scroll Fix] Injected JavaScript"
```

### 2. Test in Web Browser First

**Before testing on WebOS**, verify the plugin works in a web browser:

1. Open Jellyfin in Chrome/Firefox: `https://jellyfin.vubox.stream`
2. Open browser console (F12)
3. Go to Live TV → Guide
4. Check console for:
   - `[TV Guide Scroll Fix] Initializing...`
   - CSS should be loaded (check Network tab)
5. Test navigation with arrow keys

**If it works in browser but not WebOS:**
- The plugin works, but WebOS app isn't using transformed files
- Use direct file injection instead

**If it doesn't work in browser:**
- Plugin isn't working at all
- Check File Transformation plugin is installed
- Check plugin registration in logs

### 3. Check File Transformation Plugin

```bash
# Check if File Transformation plugin is installed and working
docker logs jellyfin | grep -i "file transformation"

# Check plugin is enabled
# Dashboard → Plugins → Installed → "File Transformation" should be enabled
```

### 4. Verify Plugin Registration

```bash
# Check if our plugin registered successfully
docker logs jellyfin | grep -i "webos scroll fix"
```

## Solution: Use Direct File Injection

If File Transformation doesn't work for WebOS native apps, use the **direct file injection method**:

```bash
# This modifies files directly on disk (works for all clients)
./apply_webos_scroll_fix.sh
docker restart jellyfin
```

This approach:
- ✅ Works for web browsers
- ✅ Works for WebOS native app
- ✅ Works for all clients
- ❌ Gets overwritten on Jellyfin updates (but easy to re-apply)

## Why Direct Injection Works

The direct file injection method:
1. Modifies `index.html` directly on disk
2. All clients (web and native) read from the same files
3. No HTTP interception needed
4. Works immediately

File Transformation plugin:
1. Intercepts HTTP requests
2. Only works if client makes HTTP requests
3. Native apps might use bundled/cached files
4. May not work for WebOS app

## Recommendation

**For WebOS native app, use direct file injection:**
```bash
./apply_webos_scroll_fix.sh
```

**Keep the plugin for:**
- Web browser clients
- Future compatibility
- Other clients that make HTTP requests

## Next Steps

1. **Test in web browser** to verify plugin works
2. **If plugin works in browser but not WebOS**: Use direct injection
3. **If plugin doesn't work at all**: Check File Transformation plugin installation

