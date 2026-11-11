# Unity 6 CI/CD Boilerplate with GitHub Actions

A Unity 6000.x project that teaches you proper CI/CD on GitHub:

- **Build once.** On every push and pull request, the project builds for WebGL on a GitHub-hosted runner using [GameCI](https://game.ci).
- **Deploy from artifact.** On pushes to the default branch, the build is published to GitHub Pages straight from the workflow artifact with GitHub's official `deploy-pages` action. **No `gh-pages` branch is pushed and the workflow never needs write access to your code** — each job only gets the permissions it declares.

WebGL published here (EDIT IT!): https://RougeLikeGame.github.io/RougeLikeUnityBoilerplate/

# Setup Steps:

- [ ] I understand FERPA laws. If I make the repository public, I will remove any student information, or I am waiving the requirement to remove student information. Otherwise, I am making the repository private;
- [ ] I have forked the repository to my own GitHub account;
- [ ] I have cloned it to my machine and edited the README.md file to include my own information on the url for the web build;
- [ ] I have followed the instructions to activate my personal licence here: https://game.ci/docs/github/activation/ ;
    - [ ] If I choose to make the repository private, I will follow this guide to add the instructor as a collaborator. https://docs.github.com/en/account-and-profile/setting-up-and-managing-your-github-user-account/managing-access-to-your-personal-repositories/inviting-collaborators-to-a-personal-repository and set up the keys here https://game.ci/docs/github/builder/#private-github-repositories
- [ ] I have visited the `Settings` > `Secrets and Variables` > `Actions` on my github repository and added the following secrets:
    - [ ] `UNITY_LICENSE` secret to my repository with the contents of my `Unity_lic.ulf` license file;
    - [ ] `UNITY_EMAIL` secret to my repository with my Unity account email;
    - [ ] `UNITY_PASSWORD` secret to my repository with my Unity account password;
- [ ] I changed the `Settings` > `Pages` > `Source` to **GitHub Actions**;
- [ ] I opened the project locally in Unity 6000.x, made a change, and committed and pushed it to the `main` or `master` branch of the repository;
- [ ] I saw and waited the GitHub Actions run execute on the `Actions` tab;
- [ ] I can open the web build in the browser at the url: https://YOUR_GH_USERNAME.github.io/YOUR_REPO_NAME/

# The pipeline:

Read `.github/workflows/main.yml` — it is heavily commented and is the main teaching artifact of this repository. It runs two jobs in order:

1. **build** — `game-ci/unity-builder@v4` builds WebGL. The build is uploaded as an artifact named `Build-WebGL` that you can download from any run page.
2. **deploy** — only runs on pushes to `main`/`master`. It downloads the `Build-WebGL` artifact and hands it to `actions/deploy-pages@v4`. GitHub Pages pulls the site from the artifact, so the job needs only `pages: write` + `id-token: write` permissions — nothing can push to your branches.

The Unity version is pinned by `ProjectSettings/ProjectVersion.txt`. GameCI reads it (`unityVersion: auto`) and picks the matching Docker image, so the local editor and CI always agree. To upgrade Unity, change the version locally and CI follows on the next push.

- [ ] I have read the `.github/workflows/main.yml` file and understand how the two jobs connect (`needs:`) into a pipeline;
- [ ] I understand that the deploy job has `if:` conditions limiting it to pushes on the default branch;
- [ ] I understand that pull requests run builds but never deploy;

# Workflow habits:

- [ ] I have read https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow and understand the Gitflow workflow;
- [ ] I understand that I should create a new branch for each feature or fix I am working on;
- [ ] I understand that every time I push to the `main` or `master` branch, the project will be built and deployed to GitHub Pages;
- [ ] If I want to customize my build, I will read the https://game.ci/docs/github/builder/ documentation;
- [ ] I have read Semantic Versioning https://semver.org/ and understand how to version my project;
- [ ] I have read how Semantic versioning would work for unity here https://game.ci/docs/github/builder/#versioning
- [ ] I have set my first git tag to `0.1.0` to my latest commit on the `main` or `master` branch;

# Smart Merge (optional but recommended):

`.gitattributes` asks git to use Unity's Smart Merge (`UnityYAMLMerge`) for `.unity`, `.prefab`, `.asset` and `.mat` files so scene and prefab merges produce valid YAML instead of broken files. It needs a one-time local setup — run once per machine:

```bash
git config merge.unityyamlmerge.name "Unity SmartMerge"
git config merge.unityyamlmerge.driver "<UNITY_INSTALL_PATH>/Editor/Data/Tools/UnityYAMLMerge merge -h -p --force --fallback none %O %A %B %P"
```

Replace `<UNITY_INSTALL_PATH>` with your editor install location (e.g. `/Applications/Unity/Hub/Editor/6000.6.2f1/Unity.app/Contents` on macOS or `C:\Program Files\Unity\Hub\Editor\6000.6.2f1` on Windows). See https://docs.unity3d.com/Manual/SmartMerge.html


PROTOTYPE CHALLENGE BRIEF
Champlain College / GPR340 — AI for Games
Project: ASCENDANT — Experimental 2D AI Prototype
Prototype Code: MIT License  |  September 2026
1. Project Goal
Build a small 2D gameplay prototype that tests reactive AI, movement constraints, encounter design, and procedural variation. The prototype is a technical and gameplay experiment rather than a production version of the commercial game.
The player begins with limited movement and abilities. Non-player entities can react to the player's current state, behavior, proximity, and position. The goal is to discover whether these systems create interesting, readable gameplay.
2. Core Technical Challenge
•	Finite state machines and/or behavior trees
•	Pathfinding and navigation
•	Proximity and state detection
•	Enemy positioning and approach behavior
•	Simple UI / player-state feedback
•	Procedural content or encounter variation where appropriate
3. Movement and Spatial Asymmetry
A key experiment is that the player and certain enemy entities do not necessarily share the same movement rules.
•	At this stage, the player is ground-bound and has conventional 2D movement.
•	Some enemy entities may move more freely through the play space and may approach from the sides, above, or below the player's position, subject to what is technically practical and visually readable in the chosen 2D implementation.
•	This creates an intentional asymmetry: enemies can occupy or approach from spaces the player cannot yet access.
•	The team should test how to communicate off-axis threats fairly through sound, shadows, screen-edge indicators, environmental cues, timing, or other readable telegraphing.
The design question is not simply how to spawn more enemies. It is how AI can create pressure from directions unavailable to the player without making the encounter feel arbitrary.
4. Conditional Perception and Hidden Threats
Some enemy entities may exist and influence gameplay before the player can fully perceive them. The purpose is to create discovery and increasing player agency—not to punish the player with invisible, unavoidable damage.
•	Unseen: At lower player-state values, an entity may be visually hidden while following, attaching to, or influencing the player. Its presence must still produce readable clues such as audio changes, movement resistance, environmental disturbance, NPC reactions, screen-edge effects, or other feedback.
•	Glimpsed: As the player's state improves, the entity becomes intermittently perceptible through silhouettes, shadows, distortions, sounds, or brief visual reveals.
•	Visible: At a higher threshold, the entity becomes clearly perceptible and the player gains more direct information and counterplay.
•	The AI remains active across all perception states; visibility changes what the player can perceive, not whether the entity exists.
•	The player must always have a path to improvement. Even while an entity is unseen, the player should be able to take accessible actions that improve their state, reduce its influence, create distance, gain protection, or trigger clearer clues.
•	Avoid irreversible downward spirals. Difficulty can increase when the player's state deteriorates, but the design should preserve recoverable opportunities so a struggling player can learn, improve, and re-enter a stronger state.
•	The team should playtest whether players interpret early clues as intriguing rather than unfair, and whether the transition from unseen to glimpsed to visible produces a satisfying realization.
Suggested systems: AI state machine or behavior tree, follow/attachment behavior, conditional rendering or visibility, player-state thresholds, status effects, telegraphing, and recovery/counterplay triggers.
5. Player-State Variable
Use one simple prototype variable (working name: State Meter) to represent changes in the player's current condition. NPC and enemy behavior can respond to this variable.
•	Different state ranges can trigger different NPC reactions.
•	Enemy aggression, distance, pursuit, or encounter pressure can change with player state.
•	The exact values and thresholds should be easy to tune during playtesting.
This is a prototype implementation variable only. It is not intended to define the final commercial game's terminology or progression system.
6. NPC and Enemy Behaviors
6.1 Civilian / Neutral NPC
•	Default: cautious or frightened
•	Can flee, hide, approach, follow, or provide a simple interaction depending on game state
•	Suggested systems: finite state machine, pathfinding, proximity detection
6.2 Mobile Enemy Entity
•	Can patrol, observe, approach, pursue, attack, retreat, or reposition
•	Behavior may respond to player state, proximity, and environmental conditions
•	May use movement options unavailable to the player, including approaches from above or below if feasible
•	Suggested systems: behavior tree and/or state machine, steering, pathfinding, spatial queries
6.3 Signature Encounter
Create one major enemy encounter built around a counter-intuitive feedback system. At first, the player should reasonably assume that attacking the enemy is the way to defeat it. However, aggressive attacks actually feed the encounter.
•	Repeated aggressive attacks cause the enemy to become larger, stronger, or more difficult to defeat.
•	As aggression continues, the player's state deteriorates: the player becomes more vulnerable to damage and the surrounding environment becomes darker or more threatening.
•	The world should provide opportunities for helpful or restorative actions during the encounter. Examples might include moving a civilian out of danger, finding a missing civilian, bringing food or supplies to someone, protecting an NPC, repairing something damaged, or completing another simple act that benefits someone in the environment.
•	Helpful actions have the opposite effect: the enemy becomes weaker, the player's condition improves, and the environment begins to recover.
•	The player should discover this relationship through gameplay rather than being directly told the solution. Environmental changes, NPC reactions, enemy behavior, visual cues, and other feedback can provide breadcrumbs.
•	The team should playtest how quickly players recognize the relationship and whether discovering it feels surprising, understandable, and rewarding.
•	Suggested systems: state machine or behavior tree, player/world-state variables, enemy scaling, NPC behaviors, environmental state changes, and event triggers.
7. Procedural Content Generation
Procedural generation should support replayability and AI experimentation without requiring a large world. It does not need to generate an entire level.
•	Vary enemy spawn or entry locations between runs.
•	Vary whether threats approach laterally, from above, or from below when the implementation supports it.
•	Vary patrol routes, encounter timing, or environmental obstacle placement.
•	Vary placement of neutral NPCs or simple interactive elements.
•	Preserve fairness: generated threats should have readable warning cues and should not appear in unavoidable positions.
This can be treated as controlled encounter generation rather than full procedural level generation. The objective is to demonstrate procedural techniques while keeping the prototype manageable.
8. Environment and Assets
One small 2D test environment is sufficient. Placeholder art and licensed/free development assets are acceptable. The prototype is proving AI behavior and gameplay systems, not final visual quality.
•	Starting / safe area
•	Small NPC interaction area
•	Enemy patrol or encounter zones
•	Simple interactive or changeable environmental elements
•	Final signature encounter area
The team may recommend a suitable 2D sprite/animation package or asset set. Any third-party assets should retain their own licenses and should not be represented as original project assets.
9. Student Deliverables
Deliverable	Minimum expectation
Playable prototype	Small 2D build demonstrating the core AI and movement experiments
NPC behaviors	Neutral NPC and enemy state changes tied to prototype variables
Pathfinding / movement	Appropriate navigation plus testing of asymmetric enemy movement
AI documentation	Documented FSMs and/or behavior trees
Procedural system	At least one controlled source of encounter or placement variation, if feasible
UI	Simple state/status feedback sufficient for testing
Process report	What was built, what was tested, challenges, findings, and recommendations
10. What the Creator Provides
•	This project brief
•	A 5–10 minute video explaining the desired player experience and prototype questions
•	Regular check-in meetings
•	Creative direction and feedback on prototype builds
•	Only the limited reference material needed to implement the assigned prototype systems
11. IP and Licensing Boundary
The software code created specifically for this academic prototype is intended to be released under the MIT License.
The MIT License applies only to software/code expressly included in the open-source prototype repository. It does not license the commercial game project, its title/branding, pre-existing documentation, world or story material, characters, terminology, artwork or concept art, audiovisual materials, design materials, or any other creator-provided proprietary content.
Creator-provided proprietary materials are supplied only as needed for the academic prototype and remain separate from the MIT-licensed code unless expressly licensed otherwise in writing.
Students may identify their participation in the academic prototype and display their own work in personal portfolios, subject to the course/Riipen agreement and any confidentiality restrictions applicable to unreleased creator-provided material.
Third-party assets, libraries, packages, and tools remain governed by their respective licenses.
12. Questions for the Student Team
•	What is the clearest 2D implementation for enemies that can approach from directions unavailable to the player?
•	How should those threats be telegraphed so they feel challenging rather than unfair?
•	Which AI techniques from the course are the best fit for this prototype?
•	Would controlled encounter generation be more useful than full procedural level generation?
•	What is a realistic scope for the available semester and team size?













Scope
Must (the game fails without it)
These are the things that, if we do not have them, I feel that we really do not have the prototype that the brief is asking for.
Player movement: Binding of Isaac-style top-down movement with walking, sprinting, and 4-direction animation. (This one is already working.)
Health and the State Meter: the player has health and one State Meter value, both shown on screen with a simple UI.
At least 2 enemy types, each with a documented FSM or behavior tree:
A ground chaser that uses A* pathfinding to get around rocks and walls.
An off-axis enemy that can come in from the sides, above, or below the room. It needs telegraphing like a shadow that grows as it gets closer, an arrow on the edge of the screen, and a sound, so that it feels fair.
Perception states (Unseen -> Glimpsed -> Visible): these are tied to the State Meter. The enemy is always there and always running its AI, and the only thing that changes is how much the player can see of it.
One civilian NPC: it has an FSM and is scared by default, and it can flee, hide, or follow depending on what is going on and on the State Meter.
The signature encounter: attacking the enemy actually feeds it and makes it bigger, and helping people is what weakens it.
Controlled encounter generation: the enemy spawn points and entry directions get picked from premade sets each run, with rules so nothing spawns right on top of the player and nothing attacks without a warning first.
The required deliverables: a WebGL build on itch.io, the AI documentation, and the process report.
Should (expected, but we can cut it)
3–4 enemy types in total instead of just 2.
A behavior tree version of one of the enemies. The FSM version is the backup, so if we run out of time it is okay.
The world getting darker when the State Meter is low and brighter when it goes back up.
3–5 premade rooms that connect with doors, kind of like how Isaac does it.
Sound effects for the telegraphs, hits, and State Meter changes.
Could (stretch goals)
5 or more enemy types plus a separate boss.
A swarm enemy that uses flocking.
Randomizing the obstacles inside the premade rooms.
Extra game feel stuff like screen shake, particles, and controller rumble.
What we cut first if we fall behind: the extra rooms and any enemy types past the first 2. We would put everything into one room and use that time on the signature encounter and the telegraphing, because I think that those are the core of what this prototype is supposed to test.









Roles 
Colin Skaarup: Player/ Player State
Charlotte Peterson: NPC
Ethan Matte: Enemies
Joshua Mathieson: Procedural
Aaron Archambault: Boss







Top 3 Risks
Risk 1: The off-axis enemies feel unfair (Design)
I think that this is our biggest risk. The whole idea of enemies coming from places the player cannot go is interesting, but if the player keeps getting hit by things they could not see or react to, it is going to feel cheap rather than challenging.
How we test it: the Alpha task for the off-axis enemy and the telegraph system. Once it is in, we will have at least 3 people outside our team play it and keep track of how many times they get hit by something they say they did not see coming.
Fallback if it fails: we make the enemies only come in from fixed spots on the edges or through doorways, with a long wind-up that is the same every time (shadow, edge arrow, and sound). If it still feels unfair after that, the off-axis attacks would only push the player back and not do damage.
Risk 2: Players never figure out the signature encounter (Design)
The encounter only works if players actually figure out on their own that helping is what wins and attacking makes it worse. That being said, most people are used to games where you just hit the thing until it dies, so there is a real chance that people just keep attacking and get frustrated.
How we test it: in the Beta playtest we will time how long it takes players to find the helpful action. Our goal is that most players figure it out within about 3 minutes.
Fallback if it fails: we make the hints stronger, like having the NPCs react more clearly, making the enemy visibly shrink when you help someone, and having a civilian say a one-line hint after the player has attacked a certain number of times.
Risk 3: Unity teamwork problems and WebGL build issues (Technical / Team)
Unity scenes are really easy to break when more than one person edits them, and WebGL builds can break in ways that you do not notice until you actually upload them. If we wait until the end to deal with either of these, I feel that it could really hurt us.
How we test it: one of our PoC tasks is getting a WebGL build up on itch.io in the first week, and then we rebuild it at every milestone so problems show up early. Each scene also has one owner, and everyone else works with prefabs.
Fallback if it fails: everyone works in their own test scene, and the scene owner is the one who brings the changes into the main scene. If a feature breaks the WebGL build (like a lighting or post-processing effect), we swap it out for a simpler version.








Team Agreement
I would say that the main point of this is to make sure that we are all on the same page and that nobody ends up stuck doing everything or feeling left out. 
Communication: We talk in our Google meet/chat that we have. We try to reply within 24 hours on school days, and if something is urgent or blocking someone, we say so in the channel.
Weekly meeting: Monday and Thursdays during class time and possibly on Thursday outside of class time at like 7 if we need it, about 30 minutes. We go over what got done, what is next, and if anyone is stuck on something, and we update the board during the meeting.
Branching and merging:
main should always open and play without errors.
Everyone works on their own branch for each issue, named like feature/<issue#>-short-name.
Nothing gets merged into main without a pull request that one other team member has reviewed and approved.
Only the owner of a scene edits that scene.
The board: everyone moves their own cards. "In Progress" means you are actually working on it that week, and when your pull request is open, the card goes to "Review".
When someone misses a task: if you know you are going to miss a deadline, you tell the group in the channel before the deadline and not after. If it happens once, we re-plan it together at the next meeting and help out where we can, because things happen and that is okay. If it keeps happening without anyone being told, we bring it up with the instructor.








ASCENDANT PoC Result & Scope Update
Outcome: PARTIALLY PASSED
I would say that the PoC passed for the most part. The main thing we wanted to test, the Scarcity Remnant boss that gets stronger when you attack it and weaker when you help, works the way we hoped it would, and it is readable and fair in the editor. The only part that did not fully pass is the WebGL build, which still needs some fixing before it runs the way it should in the browser. Since that is a technical problem and not a problem with the idea itself, we do not need to pivot, and the core of the project stays the same.
Evidence
We tested the boss in the Unity editor. We used the debug keys 8 (attack, which feeds the boss) and 9 (help, which starves the boss) 
#
Criterion (from POC.md)
Result
Evidence
1
Testers can tell whether the boss got stronger or weaker
Passed
Most people and players could tell from the boss growing or shrinking, the color changes, the animations, and the room getting darker or brighter.
2
Every attack is warned for at least 0.6 s and can be dodged
Passed
The slam (growing red circle) and the lunge (red lane) are always telegraphed before they hit, and testers were able to dodge them once they saw them a couple of times.
3
Attacking pushes it toward Ravenous, and helping makes it transform
Passed
Attacking made it reach Ravenous after some hits, and helping made it transform after 4 helpful actions.
4
No unwinnable spiral
Passed
Helping always interrupts the boss's attacks, and the boss waits longer between attacks when the player is low on health, so even after attacking a lot the player can still turn the fight around.
5
Runs as a WebGL build
 Partial
The game builds for WebGL, but it is not the full build with all of the branches like the boss one yet (mainly just have not gotten around to merging and fixing that) 


What we are changing
Because the boss and the core idea passed, we are not cutting anything or pivoting. The only change is that fixing the WebGL build becomes our first task for Alpha, because the final game has to run in the browser and we do not want to find more build problems later on. I think that it is better that we found this now, early, rather than right before a deadline.
To fix it, we plan to:
Check the WebGL publishing settings (Gzip compression with Decompression Fallback, or turning compression off) so that it loads on itch.io.
Make a Development Build so that the debug keys work in the browser for testing.
Make sure the boss room scene is in the build's scene list.
Rebuild and test it in more than one browser at each milestone, like we planned in Risk 3 of our PLAN.md.
We also already changed one thing while building the PoC: the signature encounter was planned for Beta, but we moved it up because it was our riskiest assumption. Because of that, the chaser enemy and the proximity detection have not been fully started yet, and they move to Alpha kind of.
One thing the PoC can't answer yet is whether players figure out on their own that helping is how you win, because helping is still a debug key. We will test that in Alpha once the first real helpful action is in the game.
Updated Must / Should / Could
Must
Player movement, animation, health, and State Meter UI (done)
The Scarcity Remnant signature encounter (first version done in the PoC)
A working WebGL build on itch.io (changed: now the first Alpha task)
At least one real helpful action in the world that weakens the boss, like escorting a civilian (new)
The player's attack feeds the boss (new: replaces debug key 8)
Ground chaser enemy with an FSM and A* pathfinding (moved to Alpha)
Off-axis enemy with telegraphing
Perception states (Unseen, Glimpsed, Visible)
One civilian NPC with an FSM
Controlled encounter generation
AI documentation and the process report
Should
3–4 enemy types in total
A behavior tree version of one enemy
The world getting darker or brighter with Scarcity and the State Meter
3–5 premade rooms connected with doors
Sound effects for telegraphs and boss reactions
Could
5+ enemy types and a separate boss
A flocking swarm enemy
Randomized obstacles inside rooms
Screen shake, particles, and controller rumble
What we cut first if we fall behind: the extra rooms and any enemy types past the first 2, so that we can focus on the signature encounter, the telegraphing, and keeping the WebGL build working.









Boss Demo Video:
https://youtu.be/oDLbD5kaubc


