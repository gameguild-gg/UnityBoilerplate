# Unity 6 CI/CD Boilerplate with GitHub Actions

A Unity 6000.x project that teaches you proper CI/CD on GitHub:

- **Build once.** On every push and pull request, the project builds for WebGL on a GitHub-hosted runner using [GameCI](https://game.ci).
- **Deploy from artifact.** On pushes to the default branch, the build is published to GitHub Pages straight from the workflow artifact with GitHub's official `deploy-pages` action. **No `gh-pages` branch is pushed and the workflow never needs write access to your code** — each job only gets the permissions it declares.

WebGL published here (EDIT IT!): https://YOUR_GH_USERNAME.github.io/YOUR_REPO_NAME/

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
