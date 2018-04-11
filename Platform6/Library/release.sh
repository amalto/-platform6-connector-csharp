#!/bin/sh

# Check for a valid version
if [[ $1 == "" ]]; then
	echo 'You must provide a valid version as a first argument.'
	exit 1
fi

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

# Update .nuspec file
sed -i '' -e "s/<version>.*<\/version>/<version>$1<\/version>/g" ./.nuspec

# Build the project
msbuild Library.csproj

# Create the NuGet package
cd releases/
nuget pack ../Library.csproj

# Tag the release
git commit -am "Bump to version $1"
git tag "v$1"