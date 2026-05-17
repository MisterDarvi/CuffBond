# CuffBond

A plugin for [EXILED](https://github.com/ExMod-Team/EXILED) that allows players to bind and unbind each other using a single configurable key via Server Specific Settings.

---

## Features

- Single key to bind and unbind — press once to bind, press again to unbind
- Works on any player regardless of team, including teammates
- Binding is blocked at distances greater than 1.55m
- Target must have empty hands to be bound
- SCPs cannot be bound
- Binding key is displayed at the bottom of the Server Specific Settings menu
- All UI text is fully translatable via config
- Does not interfere with hints from other plugins

---

## Requirements

| Dependency | Version  |
|------------|----------|
| EXILED     | 9.13.1+  |
| SCP:SL     | Latest   |

---

## Installation

1. Download `CuffBond.dll` from [Releases](../../releases)
2. Place it in your EXILED plugins folder:
   - **Linux:** `~/.config/EXILED/Plugins/`
   - **Windows:** `%AppData%\EXILED\Plugins\`
3. Restart the server
4. Players open `Esc → Settings → Server` and assign a key under **BIND AN ALLY**

---

## How It Works

| Condition | Result |
|-----------|--------|
| Aim at a player within 1.55m + press key | Player is bound |
| Aim at an already bound player + press key | Player is unbound |
| No weapon in hands | Error message shown |
| Target has item in hands | Binding blocked |
| Target is SCP | Binding blocked |
| Distance greater than 1.55m | No effect |

Unbinding can be performed by **any** player — no weapon required.

---

## Configuration

```yaml
cuff_bond:
  is_enabled: true
  debug: false
  max_bind_distance: 1.55

  # Messages
  msg_no_weapon: '<color=red>You must hold a weapon in your hands!</color>'

  # User Interface
  sss_header: 'BIND AN ALLY'
  sss_key_label: 'Bind / Unbind player'
  sss_key_hint: 'Aim at a nearby player and press. If already bound — unbinds.'

```

---

## Notes

- Dummy players (NPCs) cannot be bound
- Bound status is cleared automatically when a player leaves or the round ends
- Plugin appends its keys to the bottom of the SSS menu without overwriting other plugins

---

