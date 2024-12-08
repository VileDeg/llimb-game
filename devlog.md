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
* KC: Design 1 bigger level for play testing
* VG: Player - collision with terrain
* VG: Player - 2nd weapon
* VG: Player - change up 1st gun (charge up?)
* VG: Enemey - explosion, make more entertaining, turn off gravity
* KC: Enemy - refactor Enemy script into behaviour for everyone + unique attacks
* KC: Enemy - enhance attacks (Type 2 - going after player while shooting) (others - after ramming into player demage)
* KC: New enemy type - Machine gun (stands and shoots more/line/...)

### Week 4 (10. 12. - 16. 12)
* Polishing, play testing

---

## Production


### Week 1
* VG = Initial project was set up, with appropriate plugins (Input System, 2D packages) + adjusted project settings. In it created test level with implemented player + its controls.
	- 2 hours
* KC = Created 4 basic prefabs for enemies + simple EnemyController, that makes the enemy walk in random directions until they spot a player in which case they chase him, until he is too far.
	- 1:45 hours

### Week 2
* KC = implemented attacks/combat mechanics of 4 different types of enemies (dash, shooting, chasing, chainsaw rotation).
	- 4,5 hours