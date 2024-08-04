# To contribute you are required to create issues and submit a pull request to the developmet branch
	

# Clone the repository

`git checkout development`

`git pull origin development`


# Ensure your local development branch is up to date
`git checkout -b issue-<issue-number>-description`

# Fix the issue then

```
git add .
git commit -m "Fixes #<issue-number>: <description of the fix>"
git push origin issue-<issue-number>-description
```
# Create A pull Request

- Go to the GitHub repository.
- Click `Pull requests` and then `New pull request`.
- Select `development` as the base and your branch as the compare.
- Provide a detailed description of your changes and link to the issue.

# Request reviews from team members
