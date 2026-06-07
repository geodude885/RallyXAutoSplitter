# Version 1.1.1
- Ensure all requests to live split are sent in the correct order
- Fix potential issue with Harmony patching patches from other assemblies

# Version 1.1.0
- Non-blocking TCP communication to prevent game lag
  - All LiveSplit commands run asynchronously on background threads
  - 2-second connection timeout prevents freezing if server is unreachable

# Version 1.0.0
- Initial release
- Automatic timer start when track 0 begins (3-lap races only)
- Automatic splits at the start of tracks 1, 2, 3, and 4
- Final split when track 4 completes
- Automatic reset on race cancellation or returning to track selection
- TCP integration with LiveSplit Server (127.0.0.1:16834)
- Comprehensive unit test coverage
