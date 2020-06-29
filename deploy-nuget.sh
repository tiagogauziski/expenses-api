#!/bin/bash
set -ev
set -x

TAG=$TRAVIS_TAG

# Remove a leading v from the major version number (e.g. if the tag was v1.0.0)
IFS='.' read -r -a tag_array <<< "$TAG"
MAJOR="${tag_array[0]//v}"
MINOR=${tag_array[1]}
BUILD=${tag_array[2]}
SEMVER="$MAJOR.$MINOR.$BUILD"

# Set dotnet nuget username and password
# dotnet nuget add source $NUGET_SOURCE --password $NUGET_PASSWORD --username $NUGET_USERNAME --configfile nuget.config --name GitHub --store-password-in-clear-text

# Create nuget packages
dotnet pack ./src/ -p:PackageVersion=$SEMVER --configuration Release --output ./nuget_packages/

# Push nuget packages to the server.
dotnet nuget push **/*.nupkg --source $NUGET_SOURCE --api-key $NUGET_APIKEY
