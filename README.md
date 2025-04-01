# LL.imb - top-down shooter Unity game

## Description

An action top-down shooter game. In early stage of development.

- **Objective**: Kill all enemies to complete the level.
- **Gameplay**: Move around the level and shoot enemies.

## Code convention

* CamelCase for variable names.
* All private attributes start with `_` (and ones with `[SerializeField]`).
* Readonly attributes start with `_readonly_`.
* Use `[SerializeFiled] private` isntead of `public` where possible.
* The order in which members are defined: `type definitions, attributes, event declarations, methods definitions`
* Event handlers(callbacks) are defined at the bottom of the source file and their section must be marked as follows:
```
/*  ========================================== *
 *  EVENT HANDLERS                             *
 *  ========================================== */
```
* Put `[SerializeField]` on a separate line before declaration.