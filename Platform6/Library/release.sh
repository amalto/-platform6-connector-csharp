#!/bin/sh

# Check branch is master
if [ $(git symbolic-ref --short HEAD) != 'master' ]; then
	echo 'You must be on branch master to release a new version.'
	exit 1
fi

# Check branch is clean
if [ "$(git diff --shortstat)" != "" ]; then
	echo 'You have uncommitted changes.'
	exit 1
fi

# Check branch is up-to-date
git fetch

# Build the project
nuget pack

# Commit and tag the release
git commit -am "Bump to version ${TAG:1}"
git tag "$1"