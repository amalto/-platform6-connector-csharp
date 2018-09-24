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
sed -i '' -e "s/<version>.*<\/version>/<version>$1<\/version>/g" ./Library/.nuspec

# Update changelog
DATE=$(date +%Y-%m-%d)
TAG=$(echo v$1)
LINK=$(grep '\[Unreleased\]:' CHANGELOG.md)
ESCAPED_LINK=$(echo ${LINK} | sed -e 's/\[/\\[/g' -e 's/\]/\\]/g')
HEAD_LINK=$(echo ${LINK} | sed -E "s/v[0-9.]+$/$TAG/g" )
RELEASE_LINK=$(echo ${LINK} | sed -e s/Unreleased/$1/g -e s/HEAD/${TAG}/g)

sed -i '' "s/## \[Unreleased\]/## [Unreleased]\\
\\
## [$1] - $DATE/g" CHANGELOG.md
sed -i '' "s|$ESCAPED_LINK|$HEAD_LINK\\
$RELEASE_LINK|g" CHANGELOG.md

# Build the project
msbuild ./Library/Library.csproj

# Create the NuGet package
cd releases/
nuget pack ../Library/Library.csproj

# Tag the release
git commit -am "Bump to version $1"
git tag "v$1"