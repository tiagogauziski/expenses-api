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
dotnet nuget update source GitHub --password $NUGET_PASSWORD --username $NUGET_USERNAME --configfile nuget.config --source $NUGET_SOURCE

# Create nuget packages
dotnet pack ./src/ -p:PackageVersion=$SEMVER --configuration Release --output ./nuget_packages/

dotnet nuget push ./nuget_packages/*.nupkg --source $NUGET_SOURCE
