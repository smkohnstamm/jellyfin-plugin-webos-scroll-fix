# Changelog

All notable changes to this project will be documented in this file.

## [1.0.0.0] - 2025-01-27

### Added
- Initial release
- Fixes WebOS TV Guide scroll issue using File Transformation plugin
- Injects CSS for proper overflow and scroll behavior
- Injects JavaScript to handle scrollIntoView() on focus/keyboard events
- Works with all Jellyfin clients including WebOS native app
- Survives Jellyfin updates (non-destructive plugin approach)

### Technical Details
- Uses File Transformation plugin for HTML transformation
- Registers transformation for index.html
- Injects CSS and JavaScript before </head> tag
- Handles focus events and keyboard navigation in TV Guide
- Supports dynamically loaded content via MutationObserver

