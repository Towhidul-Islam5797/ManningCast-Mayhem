# ManningCast Mayhem — Client Requirements Status

This implementation reconciles the supplied documents by treating the August 12 feedback as the latest direction whenever earlier notes conflict.

## Implemented in this project

- WebGL-oriented title, optional player-entry, overview, rules, character-select, gameplay, pause, win, and lose flow.
- Client-matched player-information screen using the supplied blue background, television, black input, submit artwork, and Eight-Bit Madness font.
- Peyton/Eli selection with corrected front/back movement art and the unselected brother reacting from the final couch.
- Seven alternating traversal lanes with a randomized mix of safe, dangerous, bonus, and penalty items.
- Two athlete variants and challenge flags remove a life; sandwiches remove eight seconds.
- Recliners, remotes, and standard quarter-zips are safe pickups. Golden quarter-zips award a larger bonus.
- Footballs are collectible power-ups; using one distracts the nearest athlete and creates an opening.
- Three lives, thirty-second per-life countdown, timeout life loss, scoring, aggressive progress/time-based speed increase, and start-position reset.
- Persistent HUD for score, time, lives, and football inventory; visible pause button and keyboard controls.
- Win/lose presentation, final score, replay/change-character/menu actions, and local top-score display.
- Supplied music and sound effects integrated for movement, hits, pickups, bonuses, and scoring.
- Obstacle sprites are cropped to their visible imported bounds and normalized by per-template hierarchy sizes, so transparent source padding no longer produces inconsistent gameplay scale or hitboxes.
- Existing ManningCast studio scene art retained while superseded pools/spawners and old gameplay UI are disabled.
- All production screens, HUD panels, seven lane roots, spawn boundaries, and obstacle templates are saved as clearly named hierarchy objects for direct editing; runtime builders remain fallback-only.
- Main Menu, Character Select, and Game Scene are the only enabled production build scenes.

## External production inputs still required

These items were requested in the documents but cannot be safely invented or activated without Omaha/client-owned values:

1. Approved official contest terms/privacy copy and its production URL.
2. Approved contest-entry and leaderboard API endpoint, authentication method, payload contract, and retention policy.
3. Analytics provider/project identifiers and the approved event taxonomy.
4. Final hosting domain, WebGL deployment settings, and Omaha's production device/browser acceptance matrix.

Until those are supplied, entry details and scores remain local to the browser/device and the UI clearly identifies the build as a prototype. The website defaults to `https://omahaproductions.com`; the terms and website URLs can be injected through the `Manning.Config.TermsUrl` and `Manning.Config.WebsiteUrl` player preferences.

## Verification

- Unity 6000.4.9f1 script compilation and scene serialization: passed.
- Hierarchy validation: passed (five front-end screens plus terms, three login fields, seven lanes, ten obstacle templates, and authored gameplay HUD).
- Serialized-scene reload validation: passed (no missing production scripts; controller references, button events, client assets, font, hitboxes, and sprites all persist).
- 1920×1080 login render checked against the supplied Game Master Deck reference.
- WebGL player build: see the latest validation handoff/report.
