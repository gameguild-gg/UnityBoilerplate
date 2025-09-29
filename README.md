# Unity WebGL CI/CD with GitHub Actions

WebGL published here (EDIT IT!) https://YOUR_GH_USERNAME.github.io/YOUR_REPO_NAME/

# Setup Steps:

- [ ] I understand FERPA laws. If I make the repository public, I will remove any student information, or I am waiving the requirement to remove student information. Otherwise, I am making the repository private;
- [ ] I have forked the repository to my own GitHub account;
- [ ] I have cloned it to my machine and edited the README.md file to include my own information on the url for the web build;
- [ ] I have followed the instructions to activate my personal licence here: https://game.ci/docs/github/activation/ ;
    - [ ] If I choose to make the repository private, I will follow this guide to add the instructor as a collaborator. https://docs.github.com/en/account-and-profile/setting-up-and-managing-your-github-user-account/managing-access-to-your-personal-repositories/inviting-collaborators-to-a-personal-repository and set up the keys here https://game.ci/docs/github/builder/#private-github-repositories
- [ ] I have visited the `Settings` > `Secrets and Variables` > `Actions`;
- [ ] I have added the `UNITY_SERIAL` secret to my repository with the Unity license key;
- [ ] I have added the `UNITY_EMAIL` secret to my repository with the Unity username;
- [ ] I have added the `UNITY_PASSWORD` secret to my repository with the Unity password;
- [ ] I changed the `Settings` > `Actions` > `General` > `Workflow Permissions` to `Read and write permissions for actions`;
- [ ] I cloned the repository to my local machine and opened the project in Unity and made changes to the project;
- [ ] I have committed and pushed the changes to the `main` or `master` branch of the repository;
- [ ] I understand that every time I push to the `main` or `master` branch, the project will be built and deployed to the `gh-pages` branch;
- [ ] I saw the GitHub Actions build the project;
- [ ] I changed the `Settings` > `Pages` > `Source` to `gh-pages` branch;
- [ ] I saw the GitHub Actions deploy the project to the `gh-pages` branch;
- [ ] I can open the web build in the browser at the url: https://YOUR_GH_USERNAME.github.io/YOUR_REPO_NAME/
- [ ] I have read the https://www.atlassian.com/git/tutorials/comparing-workflows/gitflow-workflow and understand the Gitflow workflow;
- [ ] I understand that I should create a new branch for each feature or fix I am working on;
- [ ] I have read the `.github/workflows/main.yml` file and understand how the GitHub Actions are working;
- [ ] If I want to customize my build, I will read the https://game.ci/docs/github/builder/ documentation; 
- [ ] I have read Semantic Versioning https://semver.org/ and understand how to version my project;
- [ ] I have read how Semantic versioning would work for unity here https://game.ci/docs/github/builder/#versioning 
- [ ] I have set my first git tag to `0.1.0` to my latest commit on the `main` or `master` branch;
