# LL.imb

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
* 