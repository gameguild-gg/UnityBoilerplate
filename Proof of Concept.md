## **The assumption under test**

We believe that a boss that gets stronger when you attack it and weaker when you help can be readable and fair, so that players can tell what is happening just from the boss and the world, and if it doesn't, the project should pivot to a more normal boss that you damage to win, where helping people only gives you an optional buff.

## 

## **Why this is our riskiest assumption**

I think that this is the riskiest part of the whole project because the signature encounter (the Scarcity Remnant) is basically the main hook of ASCENDANT. It is the one thing that makes the game feel different from other top-down action games, and if it does not work, a lot of the other systems lose their point too, like the State Meter, the helpful actions, and the world getting darker or brighter.

It is also risky because it goes against what players already expect. Most people are used to games where you just hit the boss until it dies. That being said, our boss does the opposite, so there is a real chance that players just keep attacking, the boss keeps growing, and they get frustrated instead of curious.

This connects directly to the risks in our Plan flie:

* **Risk 2: Players never figure out the signature encounter.** This is the main one. If the boss changing size, color, and behavior is not clear enough, players will not be able to connect "I attacked and it got worse" or "I helped and it got weaker."  
* **Risk 1: Threats feel unfair.** The boss gets bigger, faster, and hits harder when it is fed, so we need to make sure that its attacks are still telegraphed well enough to dodge, even at its strongest, and that the player can always recover instead of getting stuck in a downward spiral.  
* **Risk 3: Unity and WebGL build problems.** Since the PoC has to be a WebGL build, this also tests early on whether the boss, the sprites, and the effects actually run in the browser.

I feel that if we got everything else working but this did not work, we would not really have the game that the brief/client is asking for, which is why we kind of decided to test it first instead of the chaser enemy or A\* pathfinding.

## **Pass/fail criteria**

We wrote these criteria more for debugging the boss before any real true playtesting/testing. The code for the boss was/is built first, but these are the things the playtest/test has to show for the assumption to count as passed. For now, the attack and help actions are debug keys (**8 \= attack / feed the boss**, **9 \= help / starve the boss**), because the real player attack and NPC help actions are planned for Alpha or later.

**The PoC passes if all of these are true:**

1. **The boss is readable.** Without being told the rules, most can somewhat say whether the boss got stronger or weaker after we press 8 or 9, just from how it looks and acts (size, color, animation, the room getting darker or brighter).  
2. **The attacks are fair.** Every boss attack has a warning of at least 0.6 seconds, and it is assumable that someone can dodge the slam and the lunge more than half the time after a couple of tries.  
3. **The feedback loop works both ways.** Only attacking makes the boss reach its Ravenous phase in about 4 hits, and only helping makes it transform (the win) in about 4 helpful actions.  
4. **There is no unwinnable spiral.** Helping always interrupts the boss's attacks, and the boss gives the player more breathing room when their health is low, so even a player who attacked a lot can still turn it around.  
5. **It runs as a WebGL build** in the browser, with the boss, sprites, telegraphs, and HUD all working (mostly need to worry about in the next phase/step).

**Partial pass:** if the boss is fair and runs in WebGL, but players cannot clearly tell when it is getting stronger or weaker, then the core idea still works, and we would need to make the feedback stronger, for example, bigger reactions, sound, or NPC reactions.

**Fail:** if players cannot tell what is happening at all, or the fight feels unfair even when they are trying to dodge, then we might pivot to the normal-boss version described above.

**What this PoC can't prove yet:** whether players figure out on their own that helping is how you win. Right now, helping is a debug key and not an action in the world, so that part will be tested in Alpha, once the first real helpful action (like escorting a civilian) is in the game.

