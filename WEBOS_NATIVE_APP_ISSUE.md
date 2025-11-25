# WebOS Native App Issue - Plugin Not Working

## Problem

The plugin installed successfully but **didn't fix the scrolling issue on WebOS**. This suggests that the File Transformation plugin approach may not work correctly for WebOS native apps.

## Key Questions

1. **Does File Transformation plugin intercept requests from native apps?**
   - Web browsers make HTTP requests that can be intercepted
   - Native apps might use different mechanisms (bundled files, WebView cache, etc.)

2. **Does WebOS app load `index.html` from server?**
   - Native apps often bundle web files
   - Or they might cache files aggressively
   - Or use a different file structure

3. **Is the transformation being called?**
   - Check Jellyfin logs for transformation messages
   - Verify File Transformation plugin is working

## Research Findings

### How Jellyfin WebOS App Works

The Jellyfin WebOS app is a **native app** that likely:
- Uses a WebView component to render the web interface
- May bundle web files or cache them locally
- May not make HTTP requests for `index.html` that can be intercepted

### File Transformation Plugin Limitations

The File Transformation plugin intercepts HTTP requests for web files. However:
- **Web browsers**: Make HTTP requests → Can be intercepted ✅
- **Native apps**: May use bundled/cached files → May not be intercepted ❌

## Verification Steps

### 1. Check if Transformation is Being Called

Check Jellyfin logs:
```bash
docker logs jellyfin | grep -i "webos\|scroll\|transformation"
```

Look for:
- `[WebOS Scroll Fix] Transformation registered successfully`
- `[WebOS Scroll Fix] Transforming index.html...`
- `[WebOS Scroll Fix] Injected CSS`
- `[WebOS Scroll Fix] Injected JavaScript`

### 2. Test with Web Browser First

Before testing on WebOS, verify the plugin works in a web browser:
1. Open Jellyfin in a web browser (Chrome/Firefox)
2. Go to Live TV → Guide
3. Check browser console (F12) for:
   - `[TV Guide Scroll Fix] Initializing...`
   - CSS should be loaded
4. Test navigation with arrow keys

If it works in browser but not WebOS, the issue is WebOS-specific.

### 3. Check File Transformation Plugin

Verify File Transformation plugin is working:
```bash
docker logs jellyfin | grep -i "file transformation"
```

## Potential Solutions

### Solution 1: Direct File Injection (Original Method)

The original `apply_webos_scroll_fix.sh` script directly modifies files in the Jellyfin container. This approach:
- ✅ Works for all clients (web and native)
- ✅ Modifies files directly on disk
- ❌ Gets overwritten on Jellyfin updates

**Use this if the plugin doesn't work:**
```bash
./apply_webos_scroll_fix.sh
```

### Solution 2: Verify Plugin is Actually Transforming

The plugin might be working but WebOS app is:
- Using cached files
- Not reloading after plugin installation
- Using a different file path

**Try:**
1. Clear WebOS app cache (uninstall/reinstall app)
2. Restart Jellyfin server
3. Force WebOS app to reload (close and reopen)

### Solution 3: Check Transformation Registration

The plugin registers the transformation, but it might not be called. Check:
1. File Transformation plugin is installed and enabled
2. Plugin is registered correctly (check logs)
3. Transformation method signature matches File Transformation API

## Next Steps

1. **Check Jellyfin logs** to see if transformation is being called
2. **Test in web browser** to verify plugin works at all
3. **If plugin doesn't work**, use direct file injection method
4. **If direct injection works**, the issue is with File Transformation plugin + native apps

## Alternative: Server-Side CSS Injection

If File Transformation doesn't work for native apps, we might need to:
1. Use Jellyfin's built-in custom CSS feature (if available)
2. Or use direct file modification (the original approach)
3. Or create a different type of plugin that modifies files directly

