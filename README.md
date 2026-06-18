# PIMLR World

Standalone **PIMLR** game, isolated from the original multi-game "YannickThurz" project.
**Unity 2022.3.12f1**, URP, WebGL target.

## What this is
PIMLR is a single-player third-person action/story game: a living-room hub that leads into
downtown gameplay (wash the "haters" with a water gun, music-driven attribute boosts, a
drivable vehicle, a police-car chase). It was unbundled from a project that contained three
games — PIMLR, Helix, and Humanity Rocks. **PIMLR is the only game in this repo;** ~11 GB of
unused / other-game content was stripped.

> A few shared asset packs remain because PIMLR references assets inside them — notably
> **Heavy Station Kit** (PIMLR's environment art) and a small number of shared assets in `ARC/`.
> PIMLR's vehicle uses **RealisticCarControllerV3** plus a car from **CarControllerwithShooting**
> (the shared "Helix car"). These are intentional dependencies, not leftover games.

## What was done (unbundling + QA fixes)
Isolated PIMLR into this repo and addressed the following from the two QA passes
(Polish Pass + Playtest 1):

**Fixed (code, compiles clean):**
- **Skip login → boot straight into the living room** — `AuthManager` (loads `SceneStaticEU` by name)
- **Sirihanna = baked scripted dialogue** that unlocks the water gun — `ChatbotUIController` (removes the live AI/Flowise dependency)
- **Attribute boosts clear on track skip / stop / pause** — `MusicSystem`
- **Water gun stays equipped when entering the vehicle** — `DriveVehicles`
- **Punch = 5 damage, player-only** — `JUCharacterControllerCore`
- **Gamepad music controls** (LB/RB = prev/next, Start = play/pause) — new `MusicInputHandler`
- **Sirihanna hologram material**
- **Fixed a pre-existing compile error** in `ARC/Assets/Scripts/TS/UI/QualityPage/QualityUIManager.cs`

**Not done — needs Editor/scene work:**
- #5 movement leash before vehicle unlock · #6 parallel-park the truck · #7 remove excess cars past block 1 ·
  #9 full-motion videos between levels (code present, off by default) · #10 map boundary · #11 stamina-bar UI ·
  #15 Sirihanna outfit recolor (texture work)

## ⚠️ Known issue — WebGL lighting / rendering
Headless/batchmode WebGL builds render incorrectly (blown-out white; pink/rainbow garbage on surfaces).
The production GUI-build pipeline renders correctly. To get a correct build:
1. **Build from the Unity Editor** (File → Build Settings → WebGL → Build) — **not** headless `-batchmode -nographics`.
2. Keep WebGL on the **forward** URP (quality level 3). The **deferred** URP (quality level 2) produces the
   pink/rainbow garbage on WebGL.
3. If surfaces still look wrong, set `Assets/UniversalRenderPipelineGlobalSettings → Strip Unused Variants = off`
   (variant stripping can drop shader variants the runtime needs), then rebuild.

## Build & run
- Unity **2022.3.12f1** (URP) with the **WebGL Build Support** module.
- Open the project; first import builds `Library/`.
- Build: **File → Build Settings → WebGL → Build**.
  (A headless helper exists at `Assets/Editor/PimlrBuildScript.cs` — `PimlrBuildScript.BuildWebGL` — but for
  correct rendering use the GUI build per the issue above.)
- Live build scenes: `Assets/_Game/Scene/Pimlr/00_MainMenu` → `SceneStaticEU` (living room) → `YannicksWorld` (downtown).

## External dependencies (verify before shipping)
- **Ending video** streams from `idea-nfts.com` — consider bundling locally for reliability.
- **Login API**: `idea-labs.xyz`.
- **Ready Player Me**: avatar loading over the network (CORS/connectivity sensitive).
- Legacy Sirihanna chat used Flowise / ngrok endpoints — **now bypassed** by the baked dialogue; the API
  tokens have been redacted from `FlowiseAPI.cs`. Rotate them if they were ever live.

## Architecture
See **`docs/ARCHITECTURE.md`** — deep technical reference (scenes, systems, data flow, build/deploy).
