# LL.imb

## Developers (Abysswalkers)
* Kateřina Čepelková, 523965
* Vadim Goncearenco, 567818
* Matěj Paclt, 569016

---

## Plan


### Week 1 (19. 11. - 25. 11)
* Create unity project
* KC: Setup git repository configurations (+.gitignore)
* VG: Player controller (movement, basic shooting)
* Setup test level
* MP: basic enemy AI/movement
* KC: create different robots prefabs

### Week 2 (26. 11. - 2. 12)
* KC: robot combat mechanics
* VG: physics, destruction of robots
* VG: shooting mechanics + melee weapon
* KC + MP: Level design + Navmesh
* MP: Improve enemy AI, navigation, navmesh etc.

### Week 3 (3. 12 - 9. 12)
* MP: UI (heathbar + chargebar/cooldown for weapon)
* MP: Simple menu (restart game,...)
* KC->MP: Design 1 bigger level for play testing
* VG: Player - collision with terrain
* VG: Player - 2nd weapon
* VG: Player - change up 1st gun (charge up?)
* VG: Enemey - explosion, make more entertaining, turn off gravity
* KC: Enemy - refactor Enemy script into behaviour for everyone + unique attacks
* KC: Enemy - enhance attacks (Type 2 - going after player while shooting) (others - after ramming into player demage)
* KC: New enemy type - Machine gun (stands and shoots more/line/...)

### Week 4 (10. 12. - 16. 12)
* MP: Enemy field of view
* MP: Enemy remove obstacle see-through
* MP: Enemy notice they are damaged
* MP: Enemy keeps aggro
* VG: Increase cooldown time (add minimal cooldown)
* VG: Add melee weapon
* VG: Increase enemy detection range when player shoots
* VG: Enemies respect collision with player (don't fully overlap)
* MP: Enemy does continuous damage when inside player
* VG: (Optional) Make enemy attack collider in front of enemy instead of in center
* VG: Balance limbs fly speed and damage (and randomness)
* KC: Make level bigger, make camera follow player
* KC: balance enemies, and their unique abilities
* Polishing, play testing

---

## Production


### Week 1
* VG = Initial project was set up, with appropriate plugins (Input System, 2D packages) + adjusted project settings. In it created test level with implemented player + its controls.
	- 2 hours
* KC = Created 4 basic prefabs for enemies + simple EnemyController, that makes the enemy walk in random directions until they spot a player in which case they chase him, until he is too far.
	- 1:45 hours
* MP = Created Navmesh, Added obstacles into test level, set up state machine for enemy behaviour
	- 40 mins

### Week 2
* KC = implemented attacks/combat mechanics of 4 different types of enemies (dash, shooting, chasing, chainsaw rotation).
	- 4,5 hours
* MP = UI - Created and linked Health and Charge Bar, slightly improved gun charging, You Died and Main Menu screens with necessary buttons, player death. Made test level slightly larger, more obstacles and enemies. (R and Esc buttons dont work)
	- 3,5 hours

### Week 3
* KC = refactored Enemy script into behavior for everyone + enhanced some attacks (Type 2 - going after player while shooting, shooting improvement + Type 1 - after ramming into player deals damage), added new enemy type - Machine gun (stands and shoots after the player when it detects him)
	- 3 hours

### Week 4
* KC = made bigger map for testing with broader placement of enemies, used CinemachineCamera to follow player, adjusted some enemy abilities to make them more balanced
	- 1:45 hours
