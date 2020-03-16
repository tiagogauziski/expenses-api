#!/bin/bash
set -ev

TAG=$1
DOCKER_USERNAME=$2
DOCKER_PASSWORD=$3
REPOSITORY_PROJECT=tiagogauziski/expenses-api

# Remove a leading v from the major version number (e.g. if the tag was v1.0.0)
IFS='.' read -r -a tag_array <<< "$TAG"
MAJOR="${tag_array[0]//v}"
MINOR=${tag_array[1]}
BUILD=${tag_array[2]}
SEMVER="$MAJOR.$MINOR.$BUILD"

# Build the Docker images
docker build -f src/Expenses.API/Dockerfile src/. -t $REPOSITORY_PROJECT:$TAG
docker tag $REPOSITORY_PROJECT:$TAG $REPOSITORY_PROJECT:latest

# Login to Docker Hub and upload images
docker login -u="$DOCKER_USERNAME" -p="$DOCKER_PASSWORD"
docker push $REPOSITORY_PROJECT:SEMVER
docker push $REPOSITORY_PROJECT:latest