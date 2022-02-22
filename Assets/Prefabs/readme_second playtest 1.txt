Basic operations: 
Player 1 ：WASD to move, U to pick up weapons, J to attack, K to switch weapons, L to replenish bullets.
Player 2 ：↑↓←→ to move, 4 to pick up weapons, 1 to attack, 2 to switch weapons, 3 to replenish bullets

We now provide story mode(New) and endless mode for players.

1. Your game should allow for local multiplayer with at least 2 players.

	By tapping different keys on one keyboard, players can play local multiplayer mode. Players can fight together in a same maps and face zombies together.
	Players can combat zombies and earn points at the same time in the same scene.

2. Your game should have implemented at least 2 autonomous agents that move and exhibit distinct choices or sequences of actions.

	We have more than five different types of zombies, which will be generated at random. We have over five distinct types of props to help gamers improve their various skills. Players can choose from a variety of weapon types as well.

	We have variables for zombies such as score, health, damage, movement speed, model, and so on.

	The varied zombies' behavioural patterns are altered based on their distance from the player; they will locate the player's position and pick the best approach to it. At the same time, the zombies will avoid barriers to avoid hitting the wall, which would be embarrassing.
	
	Normal Zombies:Zombies with no special skills. Zombies with no special abilities can lock the player's position in order to avoid obstacles and advance forward. They will attack if players enter their attack range.
	Runner Zombies:Zombies with high movement values and the ability to lock the player's position in order to avoid obstacles and progress forward. The player will attack if they enter attack range.
	Tank Zombies: Zombies with a high health rating have the ability to lock the player's position in order to avoid obstacles and go forward. The player will attack if they enter attack range.
	Exploding Zombies: To avoid obstacles and progress forward, Zombies lock the player's position. If the player gets too close to the attack range, they will self-detonate and deal a lot of damage.
	Shooting Zombies: Zombies with long-range shooting ability are known as remote zombies. They may lock on to the player's position in order to go forward and avoid obstructions. If the player gets close enough, they'll start throwing damage-dealing goods.



3. Optionally your game can have an option to have an AI replace one of the players (either opponent or co-op player)
	
	“Not Finished”
	We'll make an AI prefab and use the same scripts as the player, like playerStats, PlayerBehavior, and so on. Instead of the script that receives player input, we'll develop another AI script to control the behavior of the AI prefab.