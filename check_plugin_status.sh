#!/bin/bash
# Diagnostic script to check if WebOS Scroll Fix plugin is working

echo "=== WebOS Scroll Fix Plugin Diagnostic ==="
echo ""

# Check if Jellyfin container exists
CONTAINER_NAME="${JELLYFIN_CONTAINER:-jellyfin}"
if ! docker ps --format '{{.Names}}' | grep -qi jellyfin; then
    echo "❌ Jellyfin container not found"
    exit 1
fi

echo "Using container: $CONTAINER_NAME"
echo ""

# 1. Check if plugin is installed
echo "1. Checking if plugin is installed..."
if docker exec "$CONTAINER_NAME" test -f /config/plugins/Jellyfin.Plugin.WebOSScrollFix.dll 2>/dev/null; then
    echo "   ✅ Plugin DLL found"
else
    echo "   ❌ Plugin DLL not found"
fi

# 2. Check if File Transformation plugin is installed
echo ""
echo "2. Checking File Transformation plugin..."
if docker exec "$CONTAINER_NAME" test -f /config/plugins/Jellyfin.Plugin.FileTransformation.dll 2>/dev/null; then
    echo "   ✅ File Transformation plugin found"
else
    echo "   ❌ File Transformation plugin NOT found (required!)"
fi

# 3. Check plugin registration in logs
echo ""
echo "3. Checking plugin registration in logs..."
REGISTRATION=$(docker logs "$CONTAINER_NAME" 2>&1 | grep -i "webos scroll fix.*registered" | tail -1)
if [ -n "$REGISTRATION" ]; then
    echo "   ✅ Plugin registered: $REGISTRATION"
else
    echo "   ❌ Plugin registration not found in logs"
fi

# 4. Check if transformation is being called
echo ""
echo "4. Checking if transformation is being called..."
TRANSFORM_CALLS=$(docker logs "$CONTAINER_NAME" 2>&1 | grep -i "transforming index.html" | wc -l)
if [ "$TRANSFORM_CALLS" -gt 0 ]; then
    echo "   ✅ Transformation called $TRANSFORM_CALLS time(s)"
    docker logs "$CONTAINER_NAME" 2>&1 | grep -i "transforming index.html" | tail -3
else
    echo "   ⚠️  No transformation calls found (may be normal if no requests made)"
    echo "   Try accessing Jellyfin in a web browser and check again"
fi

# 5. Check for errors
echo ""
echo "5. Checking for errors..."
ERRORS=$(docker logs "$CONTAINER_NAME" 2>&1 | grep -i "webos scroll fix.*error\|webos scroll fix.*failed" | tail -5)
if [ -n "$ERRORS" ]; then
    echo "   ⚠️  Errors found:"
    echo "$ERRORS"
else
    echo "   ✅ No errors found"
fi

# 6. Test in browser recommendation
echo ""
echo "6. Recommendations:"
echo "   - Test in web browser first (Chrome/Firefox)"
echo "   - Open browser console (F12) and look for:"
echo "     '[TV Guide Scroll Fix] Initializing...'"
echo "   - If it works in browser but not WebOS:"
echo "     → Use direct file injection: ./apply_webos_scroll_fix.sh"
echo "   - If it doesn't work in browser:"
echo "     → Check File Transformation plugin installation"

echo ""
echo "=== Diagnostic Complete ==="

