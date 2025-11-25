using System.Text.Json;

namespace Jellyfin.Plugin.WebOSScrollFix;

/// <summary>
/// Handles file transformations for WebOS scroll fix.
/// </summary>
public static class TransformationHandler
{
    // Embedded CSS content
    private static readonly string CssContent = @"
/**
 * Enhanced TV Guide Navigation Fix for WebOS (LG TV)
 * Fixes: https://github.com/jellyfin/jellyfin-web/issues/5705
 */
<style id=""webos-scroll-fix-css"">
.liveTvContainer.guideTab .emby-scroller,
.liveTvContainer .guideTab .emby-scroller,
.liveTvContainer[class*=""guideTab""] .emby-scroller {
    overflow: auto !important;
    overflow-x: hidden !important;
    -webkit-overflow-scrolling: touch !important;
}
.liveTvContainer.guideTab,
.liveTvContainer[class*=""guideTab""] {
    overflow: auto !important;
    overflow-x: hidden !important;
    height: 100% !important;
    max-height: 100vh !important;
}
.guideTab .emby-scroller,
[class*=""guideTab""] .emby-scroller {
    overflow: auto !important;
    overflow-x: hidden !important;
}
.guideContainer,
[class*=""guideContainer""],
[class*=""GuideContainer""] {
    overflow-y: auto !important;
    overflow-x: hidden !important;
    -webkit-overflow-scrolling: touch !important;
}
.liveTvContainer.guideTab .guideItem,
.liveTvContainer.guideTab [class*=""guideItem""],
.liveTvContainer.guideTab [class*=""guide-item""],
.guideTab .guideItem,
.guideTab [class*=""guideItem""] {
    scroll-margin: 15vh 0 !important;
    scroll-margin-top: 20vh !important;
    scroll-margin-bottom: 10vh !important;
}
.liveTvContainer.guideTab .guideItem:focus,
.liveTvContainer.guideTab .guideItem:focus-visible,
.liveTvContainer.guideTab [class*=""guideItem""]:focus,
.liveTvContainer.guideTab [class*=""guideItem""]:focus-visible {
    scroll-margin: 20vh 0 !important;
    scroll-margin-top: 25vh !important;
    scroll-margin-bottom: 15vh !important;
    z-index: 10 !important;
    position: relative !important;
}
.liveTvContainer.guideTab .emby-scroller,
.guideTab .emby-scroller,
.guideContainer {
    scroll-padding: 20vh 0 !important;
    scroll-padding-top: 25vh !important;
    scroll-padding-bottom: 15vh !important;
}
@media (pointer: coarse) {
    .liveTvContainer.guideTab .guideItem,
    .liveTvContainer.guideTab [class*=""guideItem""] {
        scroll-margin: 25vh 0 !important;
        scroll-margin-top: 30vh !important;
        scroll-margin-bottom: 20vh !important;
    }
    .liveTvContainer.guideTab .guideItem:focus,
    .liveTvContainer.guideTab .guideItem:focus-visible {
        scroll-margin: 30vh 0 !important;
        scroll-margin-top: 35vh !important;
        scroll-margin-bottom: 25vh !important;
    }
    .liveTvContainer.guideTab .emby-scroller,
    .guideTab .emby-scroller {
        scroll-padding: 30vh 0 !important;
        scroll-padding-top: 35vh !important;
        scroll-padding-bottom: 25vh !important;
    }
}
.liveTvContainer.guideTab,
.guideTab,
.guideContainer,
.liveTvContainer.guideTab .emby-scroller,
.guideTab .emby-scroller {
    scroll-behavior: smooth !important;
}
.liveTvContainer.guideTab *:focus,
.guideTab *:focus {
    outline: 2px solid var(--accent-color, #00a4dc) !important;
    outline-offset: 2px !important;
}
[class*=""liveTv""][class*=""guide""] .emby-scroller,
[class*=""liveTv""][class*=""guide""] [class*=""scroller""],
[data-testid*=""guide""] .emby-scroller {
    overflow: auto !important;
    overflow-x: hidden !important;
    -webkit-overflow-scrolling: touch !important;
}
</style>";

    // Embedded JavaScript content
    private static readonly string JsContent = @"
<script id=""webos-scroll-fix-js"">
(function() {
    'use strict';
    console.log('[TV Guide Scroll Fix] Initializing...');
    function findScrollableContainer(element) {
        let current = element;
        let maxDepth = 10;
        let depth = 0;
        while (current && depth < maxDepth) {
            const style = window.getComputedStyle(current);
            const overflowY = style.overflowY || style.overflow;
            const hasScroll = overflowY === 'auto' || overflowY === 'scroll';
            if (hasScroll && current.scrollHeight > current.clientHeight) {
                return current;
            }
            if (current.classList && current.classList.contains('emby-scroller')) {
                return current;
            }
            if (current.classList && (
                current.classList.contains('liveTvContainer') ||
                current.classList.contains('guideTab') ||
                current.classList.contains('guideContainer')
            )) {
                return current;
            }
            current = current.parentElement;
            depth++;
        }
        current = element;
        depth = 0;
        while (current && depth < maxDepth) {
            const style = window.getComputedStyle(current);
            if (current.scrollHeight > current.clientHeight) {
                return current;
            }
            current = current.parentElement;
            depth++;
        }
        return null;
    }
    function scrollIntoViewSmooth(element) {
        if (!element) return;
        const container = findScrollableContainer(element);
        if (!container) {
            element.scrollIntoView({
                behavior: 'smooth',
                block: 'center',
                inline: 'nearest'
            });
            return;
        }
        const containerRect = container.getBoundingClientRect();
        const elementRect = element.getBoundingClientRect();
        const isAbove = elementRect.bottom < containerRect.top;
        const isBelow = elementRect.top > containerRect.bottom;
        const isLeft = elementRect.right < containerRect.left;
        const isRight = elementRect.left > containerRect.right;
        if (isAbove || isBelow || isLeft || isRight) {
            let elementTop = element.offsetTop;
            let elementLeft = element.offsetLeft;
            let parent = element.offsetParent;
            while (parent && parent !== container) {
                elementTop += parent.offsetTop;
                elementLeft += parent.offsetLeft;
                parent = parent.offsetParent;
            }
            const targetScrollTop = elementTop - (container.clientHeight / 2) + (element.offsetHeight / 2);
            const targetScrollLeft = elementLeft - (container.clientWidth / 2) + (element.offsetWidth / 2);
            container.scrollTo({
                top: Math.max(0, targetScrollTop),
                left: Math.max(0, targetScrollLeft),
                behavior: 'smooth'
            });
        }
    }
    function handleFocus(event) {
        const target = event.target;
        const isInGuide = target.closest('.liveTvContainer') || 
                         target.closest('.guideTab') ||
                         target.closest('.guideContainer') ||
                         target.closest('[class*=""guide""]');
        if (!isInGuide) return;
        setTimeout(() => {
            scrollIntoViewSmooth(target);
        }, 50);
    }
    function handleKeyDown(event) {
        if (!['ArrowUp', 'ArrowDown', 'ArrowLeft', 'ArrowRight'].includes(event.key)) {
            return;
        }
        const activeElement = document.activeElement;
        if (!activeElement) return;
        const isInGuide = activeElement.closest('.liveTvContainer') || 
                         activeElement.closest('.guideTab') ||
                         activeElement.closest('.guideContainer') ||
                         activeElement.closest('[class*=""guide""]');
        if (!isInGuide) return;
        setTimeout(() => {
            const newActiveElement = document.activeElement;
            if (newActiveElement && newActiveElement !== activeElement) {
                scrollIntoViewSmooth(newActiveElement);
            }
        }, 100);
    }
    function observeGuideChanges() {
        const observer = new MutationObserver((mutations) => {
            mutations.forEach((mutation) => {
                mutation.addedNodes.forEach((node) => {
                    if (node.nodeType === 1) {
                        if (node.classList && (
                            node.classList.contains('guideTab') ||
                            node.classList.contains('guideContainer') ||
                            node.classList.contains('liveTvContainer')
                        )) {
                            attachEventListeners(node);
                        }
                    }
                });
            });
        });
        observer.observe(document.body, {
            childList: true,
            subtree: true
        });
        return observer;
    }
    function attachEventListeners(root = document) {
        root.removeEventListener('focus', handleFocus, true);
        root.removeEventListener('keydown', handleKeyDown, true);
        root.addEventListener('focus', handleFocus, true);
        root.addEventListener('keydown', handleKeyDown, true);
    }
    function init() {
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', init);
            return;
        }
        console.log('[TV Guide Scroll Fix] Attaching event listeners...');
        attachEventListeners();
        observeGuideChanges();
        console.log('[TV Guide Scroll Fix] Initialized successfully');
    }
    init();
    let lastUrl = location.href;
    new MutationObserver(() => {
        const url = location.href;
        if (url !== lastUrl) {
            lastUrl = url;
            setTimeout(init, 500);
        }
    }).observe(document, { subtree: true, childList: true });
})();
</script>";

    /// <summary>
    /// Transforms index.html to inject CSS and JavaScript.
    /// </summary>
    /// <param name="payloadJson">JSON payload containing file contents.</param>
    /// <returns>Transformed file contents.</returns>
    public static string TransformIndexHtml(string payloadJson)
    {
        try
        {
            var payload = JsonSerializer.Deserialize<TransformationPayload>(payloadJson);
            if (payload == null || string.IsNullOrEmpty(payload.Contents))
            {
                return payloadJson;
            }

            string contents = payload.Contents;

            // Check if already injected (avoid duplicates)
            if (contents.Contains("webos-scroll-fix-css") || contents.Contains("webos-scroll-fix-js"))
            {
                return payloadJson; // Already injected
            }

            // Inject CSS and JS before </head>
            if (contents.Contains("</head>"))
            {
                contents = contents.Replace("</head>", $"{CssContent}\n{JsContent}\n</head>");
            }
            else if (contents.Contains("</body>"))
            {
                // Fallback: inject before </body> if </head> not found
                contents = contents.Replace("</body>", $"{CssContent}\n{JsContent}\n</body>");
            }

            // Return transformed payload
            var transformedPayload = new TransformationPayload
            {
                Contents = contents
            };

            return JsonSerializer.Serialize(transformedPayload);
        }
        catch (Exception ex)
        {
            // Log error but return original content
            System.Diagnostics.Debug.WriteLine($"Error transforming index.html: {ex.Message}");
            return payloadJson;
        }
    }

    private class TransformationPayload
    {
        public string Contents { get; set; } = string.Empty;
    }
}

